using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class OPScript : MonoBehaviour
{
    [SerializeField] Sprite[] openingSprites;
    [SerializeField] EventReference[] sfxRef;
    int currentSprite = 0;
    bool isFinished = false;
    public string nextScene = "LV1_Delivery";
    Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = openingSprites[currentSprite];
        //AudioManager._instance.InitializeMusic(FMODEvents.instance.mainmusic);
        //RuntimeManager.PlayOneShot(sfxRef[currentSprite]); // do we really want to play a sound right away?
    }
    void Update()
    {
        if (Input.anyKeyDown & !isFinished)
        {
            currentSprite += 1;
            if (currentSprite < openingSprites.Length)
            {
                _image.sprite = openingSprites[currentSprite];
                //RuntimeManager.PlayOneShot(sfxRef[currentSprite]);
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
}
