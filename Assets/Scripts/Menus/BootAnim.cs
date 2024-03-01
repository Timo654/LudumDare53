using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootAnim : MonoBehaviour
{
    public Sprite[] logos;
    private float delayTime;
    private bool isReady = false;
    private bool animOver = false;
    private int logoIndex = 0;
    private Image imageRenderer;
    private float startTime;
    private LogoState logoState = LogoState.Start;
    private bool mobileTouch = false;
    // Start is called before the first frame update
    void Start()
    {
        imageRenderer = GetComponent<Image>();
        UIFader.InitializeFader();
    }

    private void OnEnable()
    {
        LoadBankAndScene.OnBanksLoaded += OnReady;
    }

    private void OnDisable()
    {
        LoadBankAndScene.OnBanksLoaded -= OnReady;
    }

    void OnReady()
    {
        isReady = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isReady && animOver)
        {
            isReady = false; // no need to call it multiple times
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) mobileTouch = true;
            else mobileTouch = false;
        }

        if (Input.anyKeyDown || mobileTouch)
        {
            animOver = true;
        }

        if (animOver) return;
        // handle state change
        if (Time.time > startTime + delayTime)
        {
            startTime = Time.time;
            switch (logoState)
            {
                case LogoState.Start:
                    logoState = LogoState.FadeIn;
                    delayTime = 0f;
                    break;
                case LogoState.FadeIn:
                    logoState = LogoState.FadeOut;
                    imageRenderer.sprite = logos[logoIndex];
                    UIFader.FadeInImage(imageRenderer);
                    delayTime = 2f;
                    break;
                case LogoState.FadeOut:
                    logoState = LogoState.End;
                    UIFader.FadeOutImage(imageRenderer);
                    delayTime = 1f;
                    break;
                case LogoState.End:
                    delayTime = 0f;
                    logoState = LogoState.Start;
                    logoIndex++;
                    if (logoIndex >= logos.Length)
                    {
                        animOver = true;
                    }
                    break;
            }
        }
    }

    enum LogoState
    {
        Start,
        FadeIn,
        FadeOut,
        End
    }
}
