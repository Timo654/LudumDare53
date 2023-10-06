using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using DG.Tweening;
using System.Collections;
using UnityEngine.InputSystem;

public class OPScript : MonoBehaviour
{
    [SerializeField] Sprite[] openingSprites;
    [SerializeField] EventReference[] sfxRef;
    [SerializeField] EventReference music;
    private InputAction skipButton;
    int currentSprite = 0;
    public string nextScene = "LV1_Delivery";
    bool inputBlocked = false;
    Image _nextImage;
    Image _image;
    private PlayerControls playerControls;

    private void OnEnable()
    {
        playerControls = new PlayerControls();
        skipButton = playerControls.Menu.Escape;
        skipButton.Enable();
        skipButton.performed += OnSkipButton;
    }

    private void OnDisable()
    {
        skipButton.Disable();
    }
    public void OnSkipButton(InputAction.CallbackContext context)
    {
        inputBlocked = true;
        OnOpeningEnd();
    }

    void Start()
    {
        _image = transform.GetChild(2).GetComponent<Image>();
        _image.sprite = openingSprites[currentSprite];
        _nextImage = transform.GetChild(1).GetComponent<Image>();
        AudioManager._instance.InitializeMusic(music);
        if (currentSprite < sfxRef.Length)
            RuntimeManager.PlayOneShot(sfxRef[currentSprite]); 
    }
    void Update()
    {
        bool mobileTouch = false;
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began) mobileTouch = true;
        }
        if ((Input.anyKeyDown || mobileTouch) & !inputBlocked)
        {
            currentSprite += 1;
            if (currentSprite < openingSprites.Length)
            {
                StartCoroutine(FadeSprites(openingSprites[currentSprite]));
                if (currentSprite < sfxRef.Length)
                    RuntimeManager.PlayOneShot(sfxRef[currentSprite]); 
            }
            else
            {
                OnOpeningEnd();
            }
        }
    }
    public void OnOpeningEnd()
    {
        LevelChangerScript._instance.FadeToLevel(nextScene);
    }

    IEnumerator FadeSprites(Sprite newSprite)
    {
        _nextImage.sprite = newSprite;
        _image.DOFade(0.0f, 1f);
        inputBlocked = true;
        yield return new WaitForSecondsRealtime(1f);
        _image.DOKill();
        _image.sprite = newSprite;
        _image.DOFade(1.0f, 0f);
        inputBlocked = false;

    }
}
