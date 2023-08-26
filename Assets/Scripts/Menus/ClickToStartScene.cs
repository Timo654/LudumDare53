using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class ClickToStartScene : MonoBehaviour
{
    [SerializeField] private GameObject anyKeyText;
    private EventInstance VerbClick;
    bool pressed = false;

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
        if (Input.anyKey && !pressed)
        {
            gameObject.GetComponent<Animator>().enabled = true;
            pressed = true;
            VerbClick.start();
            anyKeyText.GetComponent<Animator>().Play("anytext_fadeout");

        }
    }

    public void OnLoadFinish()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
