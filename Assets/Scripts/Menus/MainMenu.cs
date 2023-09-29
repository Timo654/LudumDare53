using UnityEngine;

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

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
