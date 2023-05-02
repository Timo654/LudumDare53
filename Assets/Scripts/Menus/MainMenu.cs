using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitButton; // to disable in webgl

    private EventInstance musicEventInstance;

    void Start()
    {
        //SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.2f));
        //SetSoundVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        // play music here
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isMobilePlatform)
        {
            exitButton.SetActive(false);
        }
        AudioManager._instance.InitializeMusic(FMODEvents.instance.menumusic);
    }

    public void PlayGame ()
    {
        LevelChangerScript._instance.FadeToLevel("Delivery");
    }
    public void OpenCredits ()
    {
        LevelChangerScript._instance.FadeToLevel("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
