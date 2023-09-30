using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitButton; // to disable in webgl

    void Start()
    {
        if (BuildConstants.isWebGL|| BuildConstants.isMobile || BuildConstants.isExpo)
        {
            exitButton.SetActive(false);
        }
    }

    public void PlayGame ()
    {
        LevelChangerScript._instance.FadeToLevel("Opening");
    }
    public void OpenCredits ()
    {
        LevelChangerScript._instance.FadeToLevel("Credits");
    }
    private void Update()
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
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
