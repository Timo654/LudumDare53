using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangerScript : MonoBehaviour
{
    public Animator animator;
    private string levelToLoad;

    public static LevelChangerScript _instance { get; private set; }

    public void Awake()
    {
        _instance = this;
    }
    public void FadeToLevel(string levelName)
    {
        AudioManager._instance.FadeOutMusic();
        levelToLoad = levelName;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete ()
    {
        AudioManager._instance.StopSFX();
        SceneManager.LoadScene(levelToLoad);
    }
}
