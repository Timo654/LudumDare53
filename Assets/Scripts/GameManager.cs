using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public enum GameState
{
    Start,
    Running,
    Delivery,
    Win,
    Lose,
    SecretWin
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject MobileUI; // TODO
    [SerializeField] GameObject deliveryPanel;
    [SerializeField] GameObject hintPanel;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameObject timerCounter;
    public TextMeshPro helpText;
    private Timer timer;

    public HouseObject currentHouse;
    private GameState _currentState;
    private int _currentHappiness = 1000;
    private DeliveryController deliveryController;
    public List<DeliveryItem> inventory = new();
    private EventSystem EVRef;
    private EventInstance Box;
    private TextMeshProUGUI counterText;
    // Start is called before the first frame update
    void Start()
    {
        deliveryController = GetComponent<DeliveryController>();
        EVRef = EventSystem.current; // get the current event system
        OnGameStateChanged(GameState.Start);
        Box = AudioManager.instance.CreateInstance(FMODEvents.instance.playerBox);
        counterText = timerCounter.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        timer = new GameObject().AddComponent<Timer>();
        timer.OnZero += RanOutOfTime;
        timer.UpdateGUI += UpdateTimerText;
    }

    private void OnDisable()
    {
        timer.OnZero -= RanOutOfTime;
        timer.UpdateGUI -= UpdateTimerText;
    }

    void UpdateTimerText(string time) {
        counterText.SetText(time);
    }

    void RanOutOfTime()
    {
        timer.EndTimer(false);
        timerCounter.SetActive(false);
        int scoreToAdd = 0;
        deliveryPanel.SetActive(false);
        hintPanel.SetActive(false);
        if (currentHouse != null)
        {
                Debug.Log("wrong item.......");
                scoreToAdd = -200;
                StartCoroutine(currentHouse.SetTemporaryText(":("));
            }
        AddScore(scoreToAdd);
        Box.start();
        EnablePlayerMovementInput();
        OnGameStateChanged(GameState.Running);
    }

    public void ShuffleInventory()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            DeliveryItem temp = inventory[i];
            int randomIndex = Random.Range(i, inventory.Count);
            inventory[i] = inventory[randomIndex];
            inventory[randomIndex] = temp;
        }
    }

    public void AddToInventory(DeliveryItem item)
    {
        inventory.Add(item);
    }

    public void DisablePlayerMovementInput()
    {
        player.GetComponent<Player_Walk>().DisableBinds();
    }

    public void EnablePlayerMovementInput()
    {
        player.GetComponent<Player_Walk>().EnableBinds();
    }



    public EventInstance GetFootsteps()
    { 
        return player.GetComponent<Player_Walk>().playerFootsteps;
    }


    public void OnGameStateChanged(GameState newState)
    {
        if (_currentState.Equals(GameState.Win) || _currentState.Equals(GameState.Lose))
        {
            Debug.Log("Tried to change state after hitting an ending, ignoring.");
            return;
        }
        _currentState = newState;
        switch (_currentState)
        {
            case GameState.Start:
                //TODO: Setup stuff for starting game
                HandleStart();
                break;
            case GameState.Running:
                //TODO: Handle running animation etc.
                Debug.Log($"switched game state to running");
                HandleRunning();
                break;
            case GameState.Delivery:
                Debug.Log($"switched game state to delivery");
                HandleDelivery();
                break;
            case GameState.Win:
                //TODO: Handle win
                HandleWin();
                Debug.Log($"switched game state to win");
                break;
            case GameState.SecretWin:
                //TODO: Handle win
                HandleSecretWin();
                Debug.Log($"switched game state to win");
                break;
            default:
                Debug.Log("Unknown state passed to state manager");
                break;
        }
    }

    void HandleStart()
    {
        audioManager.InitializeMusic(FMODEvents.instance.mainmusic);
        Debug.Log($"switched game state to start");
        PlayerPrefs.SetInt("Happiness", 0);
        OnGameStateChanged(GameState.Running);
    }

    void HandleRunning()
    {
        UnpauseGame();
        if (Application.isMobilePlatform)
        {
            Debug.Log("mobile!");
            MobileUI.SetActive(true);
            helpText.text = "Move by tapping either side of your screen. Go make those deliveries!";
        }
    }

    void HandleDelivery()
    {
        DisablePlayerMovementInput();
        //PauseGame();
        Box.start();
        if (Application.isMobilePlatform)
        {
            Debug.Log("mobile!");
            MobileUI.SetActive(false);
        }
        deliveryController.UpdateItems();
        deliveryPanel.SetActive(true);
        hintPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentHouse.resident.GetDesiredItem().GetRandomLine();
        hintPanel.SetActive(true);
        EVRef.SetSelectedGameObject(deliveryPanel.transform.GetChild(0).transform.GetChild(0)
            .gameObject); // set current selected button
        timerCounter.SetActive(true);
        timer.StartTimer(7f);
    }

    public void HandOverItem(DeliveryItem deliveryItem)
    {
        timer.EndTimer(false);
        timerCounter.SetActive(false);
        int scoreToAdd;
        Debug.Log("selected" + deliveryItem.GetName());
        deliveryPanel.SetActive(false);
        hintPanel.SetActive(false);
        if (currentHouse != null)
        {
            if (currentHouse.resident.GetDesiredItem().GetName() == deliveryItem.GetName())
            {
                Debug.Log("got the correct item!");
                scoreToAdd = 500;
                StartCoroutine(currentHouse.SetTemporaryText(":)"));
            }
            else
            {
                Debug.Log("wrong item.......");
                scoreToAdd = -200;
                StartCoroutine(currentHouse.SetTemporaryText(":("));
            }
            AddScore(scoreToAdd);
            Debug.Log("Current happiness is at " + GetHappiness());
        }
        deliveryItem.decrementCount();
        Box.start();
        EnablePlayerMovementInput();
        OnGameStateChanged(GameState.Running);
    }

    void HandleWin()
    {
        DisablePlayerMovementInput();
        PlayerPrefs.SetInt("Happiness", _currentHappiness);
        //SceneManager.LoadScene(5);
        if (_currentHappiness > 2900)
        {
            StartCoroutine(DelaySceneLoad(2, "GoodEnd"));
        }
        else
        {
            StartCoroutine(DelaySceneLoad(2, "BadEnd"));
        }
    }

    void HandleSecretWin()
    {
        DisablePlayerMovementInput();
        PlayerPrefs.SetInt("Happiness", _currentHappiness);
        //SceneManager.LoadScene(5);
            StartCoroutine(DelaySceneLoad(2, "SecretEnd"));
    }

    IEnumerator DelaySceneLoad(float delay, string scene)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    public void AddScore(int scoreToAdd)
    {
        _currentHappiness += scoreToAdd;
    }

    public int GetHappiness()
    {
        return _currentHappiness;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }
}