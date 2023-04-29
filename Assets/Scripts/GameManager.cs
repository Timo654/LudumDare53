using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public enum GameState
{
    Start,
    Running,
    Delivery,
    Win,
    Lose,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject MobileUI; // TODO
    [SerializeField] GameObject deliveryPanel;
    [SerializeField] GameObject hintPanel;
    public HouseObject currentHouse;
    private GameState _currentState;
    private int _currentHappiness = 1000;
    private DeliveryController deliveryController;
    public List<DeliveryItem> inventory = new();
    private EventSystem EVRef;

    // Start is called before the first frame update
    void Start()
    {
        deliveryController = GetComponent<DeliveryController>();
        EVRef = EventSystem.current; // get the current event system
        OnGameStateChanged(GameState.Start);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddToInventory(DeliveryItem item)
    {
        inventory.Add(item);
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
            case GameState.Lose:
                //TODO: Handle loss
                HandleLoss();
                Debug.Log($"switched game state to loss");
                break;
            case GameState.Win:
                //TODO: Handle win
                HandleWin();
                Debug.Log($"switched game state to win");
                break;
            default:
                Debug.Log("Unknown state passed to state manager");
                break;
        }
    }

    void HandleStart()
    {
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
        }
    }

    void HandleDelivery()
    {
        PauseGame();
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
    }

    public void HandOverItem(DeliveryItem deliveryItem)
    {
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
                StartCoroutine(currentHouse.SetTemporaryText("good"));
            }
            else
            {
                Debug.Log("wrong item.......");
                scoreToAdd = -200;
                StartCoroutine(currentHouse.SetTemporaryText("bad..."));
            }
            AddScore(scoreToAdd);
            Debug.Log("Current happiness is at " + GetHappiness());
        }
        deliveryItem.decrementCount();

        OnGameStateChanged(GameState.Running);
    }

    void HandleLoss()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(DelaySceneLoad(2, "BadEnd"));
    }

    void HandleWin()
    {
        PlayerPrefs.SetInt("Happiness", _currentHappiness);
        //SceneManager.LoadScene(5);
        if (_currentHappiness > 3000)
        {
            StartCoroutine(DelaySceneLoad(2, "GoodEnd"));
        }
        else
        {
            StartCoroutine(DelaySceneLoad(2, "BadEnd"));
        }
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