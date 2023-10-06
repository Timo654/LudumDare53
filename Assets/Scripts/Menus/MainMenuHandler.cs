using UnityEngine;
using FMOD.Studio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject anyKeyText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject selectedUIElement;
    [SerializeField] private GameObject backroomsPlayBtn;
    private EventInstance VerbClick;
    bool pressed = false;
    bool inMenu = false;
    bool inBackrooms = false;
    bool isCapsLock = true;
    [SerializeField] private GameObject exitButton; // to disable in webgl

    public void PlayGame()
    {
        LevelChangerScript._instance.FadeToLevel("Opening");
    }
    public void OpenCredits()
    {
        LevelChangerScript._instance.FadeToLevel("Credits");
    }

    void Start() 
    {
        if (BuildConstants.isWebGL || BuildConstants.isMobile || BuildConstants.isExpo)
        {
            exitButton.SetActive(false);
        }
        VerbClick = AudioManager._instance.CreateInstance(FMODEvents.instance.verbclick);
    }
    void OnEnable()
    {
        anyKeyText.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    void OnGUI()
    {
        Event e = Event.current;
        // https://discussions.unity.com/t/how-to-get-the-keyboard-os-capslock-state/203094/2
        if (e.isKey & e.character != char.MinValue)
        {
            string UpperChar = e.character.ToString().ToUpper();
            isCapsLock = UpperChar == e.character.ToString();
        }
    }

        void Update()
    {
        if (BuildConstants.isExpo || BuildConstants.isDebug)
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {
                AudioManager._instance.FadeOutMusic();
                SceneManager.LoadScene("LV1_Delivery");
            }
            else if (Input.GetKeyDown(KeyCode.F7))
            {
                AudioManager._instance.FadeOutMusic();
                SceneManager.LoadScene("LV2_Delivery");
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                AudioManager._instance.FadeOutMusic();
                SceneManager.LoadScene("MainMenu");
            }
        }

        bool mobileTouch = false;
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) mobileTouch = true;
        }
        if ((Input.anyKeyDown || mobileTouch) && !pressed)
        {
            gameObject.GetComponent<Animator>().enabled = true;
            pressed = true;
            VerbClick.start();
            anyKeyText.GetComponent<Animator>().Play("anytext_fadeout");

        }
        else if ((Input.anyKeyDown || mobileTouch) && !inMenu)
        {
            gameObject.GetComponent<Animator>().Play("Menu");
        }
        else if (Input.GetKeyDown(KeyCode.B) && inMenu && !isCapsLock) { // TODO - alternative way to trigger
                if (inBackrooms)
                {
                    LeavePressed();
                }
                else
                {
                    mainMenu.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(backroomsPlayBtn);
                    gameObject.GetComponent<Animator>().Play("To_MenuB");
                    inBackrooms = true;
                }       
                
        }
    }

    public void LeavePressed()
    {
        gameObject.GetComponent<Animator>().Play("From_MenuB");
        inBackrooms = false;
    }

    public void BPlayPressed()
    {
        LevelChangerScript._instance.FadeToLevel("LvB_Delivery");
    }

    public void OnLoadFinish()
    {
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(selectedUIElement);
        if (!inMenu)
        {
            inMenu = true;          
            AudioManager._instance.InitializeMusic(FMODEvents.instance.menumusic);
        }      
    }
}