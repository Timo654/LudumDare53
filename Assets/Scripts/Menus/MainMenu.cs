using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitButton; // to disable in webgl

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isMobilePlatform)
        {
            exitButton.SetActive(false);
        }
    }

    public void PlayGame ()
    {
        LevelChangerScript._instance.FadeToLevel("LV1_Delivery");
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
