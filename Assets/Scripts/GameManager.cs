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
    [SerializeField] private GameObject MobileUI;
    [SerializeField] GameObject deliveryPanel;
    [SerializeField] GameObject hintPanel;
    [SerializeField] GameObject timerCounter;
    [SerializeField] GameObject tutorialUI;
    public TextMeshPro helpText;
    private Timer timer;
    private EventInstance Brakes;
    private EventInstance BadEnding;
    private EventInstance GoodEnding;
    private EventInstance SecretEnding;
    private EventInstance SecretEnding2;
    public HouseObject currentHouse;
    private GameState _currentState;
    public bool isInputDisabled = false;
    private int _currentHappiness = 1000;
    private DeliveryController deliveryController;
    public List<DeliveryItem> inventory = new();
    private EventSystem EVRef;
    private EventInstance Box;
    private TextMeshProUGUI counterText;
    private Player_Walk playerWalk;
    bool isFirstTime;
    public float timerLength = 7f;
    public static GameManager _instance;
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        if (BuildConstants.isExpo)
        {
            isFirstTime = true;
        }
        else
        {     
            isFirstTime = PlayerPrefs.GetInt("isFirstTime", 1) == 1;
        }
        Brakes = AudioManager._instance.CreateInstance(FMODEvents.instance.playerBrakes);
        deliveryController = GetComponent<DeliveryController>();
        EVRef = EventSystem.current; // get the current event system
        OnGameStateChanged(GameState.Start);
        Box = AudioManager._instance.CreateInstance(FMODEvents.instance.playerBox);
        counterText = timerCounter.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playerWalk = player.GetComponent<Player_Walk>();
        BadEnding = AudioManager._instance.CreateInstance(FMODEvents.instance.BadEndingMusic);
        GoodEnding = AudioManager._instance.CreateInstance(FMODEvents.instance.GoodEndingMusic);
        SecretEnding = AudioManager._instance.CreateInstance(FMODEvents.instance.SecretEndingMusic);
        SecretEnding2 = AudioManager._instance.CreateInstance(FMODEvents.instance.SecretEndingMusic); // TODO
    }

    private void OnEnable()
    {
        timer = new GameObject().AddComponent<Timer>();
        timer.OnZero += RanOutOfTime;
        timer.UpdateGUI += UpdateTimerText;
    }

    public GameState GetCurrentState()
    {
        return _currentState;
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
        isInputDisabled = true;
        playerWalk.DisableBinds();
    }

    public void EnablePlayerMovementInput()
    {
        isInputDisabled = false;
        playerWalk.EnableBinds();
    }



    public EventInstance GetFootsteps()
    { 
        return playerWalk.playerFootsteps;
    }

    public void CreateDust()
    {
        playerWalk.CreateDust();
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
                HandleStart();
                break;
            case GameState.Running:
                Debug.Log($"switched game state to running");
                HandleRunning();
                break;
            case GameState.Delivery:
                Debug.Log($"switched game state to delivery");
                HandleDelivery();
                break;
            case GameState.Win:
                HandleWin();
                Debug.Log($"switched game state to win");
                break;
            case GameState.SecretWin:
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
        if (!AudioManager.IsPlaying())
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "LV2_Delivery")
            {
                AudioManager._instance.InitializeMusic(FMODEvents.instance.CityMainMusic); 
            }
            else
            {
                AudioManager._instance.InitializeMusic(FMODEvents.instance.mainmusic);
            }
        }
        Debug.Log($"switched game state to start");
        PlayerPrefs.SetInt("Happiness", 0);
        OnGameStateChanged(GameState.Running);
    }

    void HandleRunning()
    {
        UnpauseGame();
        if (BuildConstants.isMobile)
        {
            Debug.Log("mobile!");
            
            helpText.text = "Move by tapping either side of your screen. Go make those deliveries!";
        }
        MobileUI.SetActive(true);
    }

    void HandleDelivery()
    {
        DisablePlayerMovementInput();
        //PauseGame();
        Box.start();
        MobileUI.SetActive(false);
        deliveryController.UpdateItems();
        deliveryPanel.SetActive(true);
        float _timerLength = timerLength;
        if (isFirstTime && tutorialUI != null)
        {
            tutorialUI.SetActive(true);
            _timerLength = 60f;
        }
        hintPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentHouse.resident.GetDesiredItem().GetRandomLine();
        EVRef.SetSelectedGameObject(deliveryPanel.transform.GetChild(0).transform.GetChild(0)
            .gameObject); // set current selected button
        timerCounter.SetActive(true);
        timer.StartTimer(_timerLength);
    }

    public void HandOverItem(DeliveryItem deliveryItem)
    {
        timer.EndTimer(false);
        timerCounter.SetActive(false);
        int scoreToAdd;
        Debug.Log("selected" + deliveryItem.GetName());
        if (isFirstTime && tutorialUI != null)
        {
            PlayerPrefs.SetInt("isFirstTime", 0);
            tutorialUI.SetActive(false);
            isFirstTime = false;
        }
        deliveryPanel.SetActive(false);
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
        CreateDust();
        Brakes.start();
        PlayerPrefs.SetInt("Happiness", _currentHappiness);
        if (_currentHappiness > 2900)
        {
            if (SceneManager.GetActiveScene().name == "LV2_Delivery")
            {
                StartCoroutine(DelayAudio(2.5f, GoodEnding));
                StartCoroutine(DelaySceneLoad(2, "GoodEnd"));
            }
            else
            {
                StartCoroutine(DelaySceneLoad(2, "LV1_End"));
            }
        }
        else
        {
            StartCoroutine(DelayAudio(1.5f, BadEnding));
            StartCoroutine(DelaySceneLoad(2, "BadEnd"));
        }
    }

    IEnumerator DelayAudio(float delay, EventInstance ev)
    {
        yield return new WaitForSeconds(delay); // add a delay
        ev.start();
    }

    void HandleSecretWin()
    {
        DisablePlayerMovementInput();
        CreateDust();
        Brakes.start();
        PlayerPrefs.SetInt("Happiness", _currentHappiness);
        if (SceneManager.GetActiveScene().name == "LV2_Delivery")
        {
            //StartCoroutine(DelayAudio(1.5f, SecretEnding2)); // TODO
            StartCoroutine(DelaySceneLoad(2, "SecretEnd2"));
        }
        else
        {
            StartCoroutine(DelayAudio(1.5f, SecretEnding));
            StartCoroutine(DelaySceneLoad(2, "SecretEnd"));
        }    
    }

    IEnumerator DelaySceneLoad(float delay, string scene)
    {
        yield return new WaitForSeconds(delay);
        LevelChangerScript._instance.FadeToLevel(scene);
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