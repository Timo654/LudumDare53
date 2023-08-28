using UnityEngine;
using FMOD.Studio;
using UnityEditor;

public class ClickToStartScene : MonoBehaviour
{
    [SerializeField] private GameObject anyKeyText;
    [SerializeField] private GameObject mainMenu;
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
        if (Input.anyKeyDown && !pressed)
        {
            gameObject.GetComponent<Animator>().enabled = true;
            pressed = true;
            VerbClick.start();
            anyKeyText.GetComponent<Animator>().Play("anytext_fadeout");

        }
        else if (Input.anyKeyDown && !inMenu)
        {
            gameObject.GetComponent<Animator>().Play("Menu");
        }
    }

    public void OnLoadFinish()
    {
        inMenu = true;
        mainMenu.SetActive(true);
        AudioManager._instance.InitializeMusic(FMODEvents.instance.menumusic);
    }
}
