using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Running,
    Delivery,
    Win,
    Lose,
    SecretWin
}

public enum EndingType
{
    Bad,
    Good,
    Secret
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject MobileUI;
    [SerializeField] GameObject deliveryPanel;
    [SerializeField] GameObject hintPanel;
    [SerializeField] GameObject timerCounter;
    [SerializeField] GameObject tutorialUI;
    [SerializeField] public DataContainer gameData;
    public TextMeshPro helpText;
    private Timer timer;
    private EventInstance Brakes;
    private EventInstance BadEnding;
    private EventInstance GoodEnding;
    private EventInstance SecretEnding;
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
    const string sid = "00000000-0000-0000-0000-000000000000";
    static readonly Guid nullGuid = new Guid(sid);
    // Start is called before the first frame update
    void Awake()
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
        Box = AudioManager._instance.CreateInstance(FMODEvents.instance.playerBox);
        counterText = timerCounter.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playerWalk = player.GetComponent<Player_Walk>();
        if (gameData.badEndMusic.Guid != nullGuid) BadEnding = AudioManager._instance.CreateInstance(gameData.badEndMusic);
        if (gameData.goodEndMusic.Guid != nullGuid) GoodEnding = AudioManager._instance.CreateInstance(gameData.goodEndMusic);
        if (gameData.secretEndMusic.Guid != nullGuid) SecretEnding = AudioManager._instance.CreateInstance(gameData.secretEndMusic);
        OnGameStateChanged(GameState.Start);
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

    void UpdateTimerText(string time)
    {
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
            int randomIndex = UnityEngine.Random.Range(i, inventory.Count);
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

    private void Update()
    {
        if (BuildConstants.isExpo || BuildConstants.isDebug)
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {
                Time.timeScale = 1f;
                AudioManager._instance.FadeOutMusic();
                SceneManager.LoadScene("LV1_Delivery");
            }
            else if (Input.GetKeyDown(KeyCode.F7))
            {
                Time.timeScale = 1f;
                AudioManager._instance.FadeOutMusic();
                SceneManager.LoadScene("LV2_Delivery");
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                Time.timeScale = 1f;
                AudioManager._instance.FadeOutMusic();
                SceneManager.LoadScene("MainMenu");
            }
        }
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
        UnpauseGame(); // just in case
        if (!AudioManager.IsPlaying())
        {
            if (gameData.gameplayMusic.Guid != nullGuid) AudioManager._instance.InitializeMusic(gameData.gameplayMusic);
        }
        Debug.Log($"switched game state to start");
        PlayerPrefs.SetInt("Happiness", 0);
        OnGameStateChanged(GameState.Running);
    }

    void HandleRunning()
    {
        if (BuildConstants.isMobile && helpText != null)
        {
            Debug.Log("mobile!");

            helpText.text = "Move by tapping the sides of your screen. \r\nGo make those deliveries!";
        }
        MobileUI.SetActive(true);
    }

    void HandleDelivery()
    {
        DisablePlayerMovementInput();
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
        hintPanel.GetComponent<TextMeshProUGUI>().text = currentHouse.resident.GetDesiredItem().GetRandomLine();
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
            if (GoodEnding.isValid()) StartCoroutine(DelayAudio(2.5f, GoodEnding));
            StartCoroutine(DelaySceneLoad(2, gameData.goodEndScene));

        }
        else
        {
            if (BadEnding.isValid()) StartCoroutine(DelayAudio(2.5f, BadEnding));
            StartCoroutine(DelaySceneLoad(2, gameData.badEndScene));
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
        if (SecretEnding.isValid()) StartCoroutine(DelayAudio(2.5f, SecretEnding));
        StartCoroutine(DelaySceneLoad(2, gameData.secretEndScene));
    }

    IEnumerator DelaySceneLoad(float delay, string scene)
    {
        yield return new WaitForSeconds(delay);
        LevelChanger._instance.FadeToLevel(scene);
    }

    public void AddScore(int scoreToAdd)
    {
        _currentHappiness += scoreToAdd;
    }

    public int GetHappiness()
    {
        return _currentHappiness;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }
}