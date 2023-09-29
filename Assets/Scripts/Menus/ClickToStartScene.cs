using UnityEngine;
using FMOD.Studio;
using UnityEditor;
using UnityEngine.EventSystems;

public class ClickToStartScene : MonoBehaviour
{
    [SerializeField] private GameObject anyKeyText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject selectedUIElement;
    private EventInstance VerbClick;
    bool pressed = false;
    bool inMenu = false;

    void Start() 
    {
        VerbClick = AudioManager._instance.CreateInstance(FMODEvents.instance.verbclick);
    }
    void OnEnable()
    {
        anyKeyText.SetActive(true);
    }

    void Update()
    {
        if ((Input.anyKeyDown || (Input.touchCount > 0)) && !pressed)
        {
            gameObject.GetComponent<Animator>().enabled = true;
            pressed = true;
            VerbClick.start();
            anyKeyText.GetComponent<Animator>().Play("anytext_fadeout");

        }
        else if ((Input.anyKeyDown || (Input.touchCount > 0)) && !inMenu)
        {
            gameObject.GetComponent<Animator>().Play("Menu");
        }
    }

    public void OnLoadFinish()
    {
        inMenu = true;
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(selectedUIElement);
        AudioManager._instance.InitializeMusic(FMODEvents.instance.menumusic);
    }
}
