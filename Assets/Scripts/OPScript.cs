using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using DG.Tweening;
using System.Collections;

public class OPScript : MonoBehaviour
{
    [SerializeField] Sprite[] openingSprites;
    [SerializeField] EventReference[] sfxRef;
    int currentSprite = 0;
    public string nextScene = "LV1_Delivery";
    bool inputBlocked = false;
    Image _nextImage;
    Image _image;

    void Start()
    {
        
        _image = transform.GetChild(1).GetComponent<Image>();
        _image.sprite = openingSprites[currentSprite];
        _nextImage = transform.GetChild(2).GetComponent<Image>();
        AudioManager._instance.InitializeMusic(FMODEvents.instance.FirstCutsceneMusic);
        if (currentSprite < sfxRef.Length)
            RuntimeManager.PlayOneShot(sfxRef[currentSprite]); 
    }
    void Update()
    {
        if ((Input.anyKeyDown || (Input.touchCount > 0)) & !inputBlocked)
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
        _image.DOFade(0.0f, 1.5f);
        _nextImage.DOFade(1.0f, 1f);
        inputBlocked = true;
        yield return new WaitForSecondsRealtime(1f);
        _nextImage.DOKill(); // just in case
        _image.DOKill();
        _image.sprite = newSprite;
        _image.DOFade(1.0f, 0f);
        _nextImage.DOFade(0.0f, 0f);
        inputBlocked = false;

    }
}
