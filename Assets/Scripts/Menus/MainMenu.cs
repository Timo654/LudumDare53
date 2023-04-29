using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitButton; // to disable in webgl

    void Start()
    {
        //SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.2f));
        //SetSoundVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        // play music here
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isMobilePlatform)
        {
            exitButton.SetActive(false);
        }
    }

    public void PlayGame ()
    {
        //AudioManager.FadeMusicOut(1);
        StartCoroutine(DelaySceneLoad(2, "Main"));
    }
    public void OpenCredits ()
    {
        //AudioManager.FadeMusicOut(1);
        StartCoroutine(DelaySceneLoad(2, "Credits"));
    }
    IEnumerator DelaySceneLoad(float delay, string scene)
    {
        //AudioManager.FadeMusicOut(delay);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
