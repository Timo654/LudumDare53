using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToStartScene : MonoBehaviour
{
    [SerializeField] private GameObject anyKeyText;
    bool pressed = false;
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
            anyKeyText.GetComponent<Animator>().Play("anytext_fadeout");
        }
    }

    public void OnLoadFinish()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
