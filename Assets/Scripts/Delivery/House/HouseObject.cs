using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class HouseObject : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private DataContainer gameData;
    public Sprite artwork;
    public Person resident;
    public DeliveryItem item;
    private TextMeshPro mText;
    private SpriteRenderer personSprite;

    [SerializeField] private AudioManager audioManager;
    private EventInstance Footsteps;
    private EventInstance Brakes;
    

    private void Start() {
        Footsteps = gameManager.GetFootsteps();
        Brakes = AudioManager._instance.CreateInstance(FMODEvents.instance.playerBrakes);
    }


    private void Awake()
    {
        item = new DeliveryItem(gameData.Items[Random.Range(0, gameData.Items.Length)], 1);
        resident = new Person(gameData.personSprites[Random.Range(0, gameData.personSprites.Length)], item);
        artwork = gameData.houseSprites[Random.Range(0, gameData.houseSprites.Length)];
        GetComponent<SpriteRenderer>().sprite = artwork;
        GetComponent<SpriteRenderer>().color = Color.white;
        mText = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        personSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        personSprite.sprite = resident.GetArtwork();
    }

    public void SetText(string text)
    {
        mText.text = text;
    }

    public void ToggleText()
    {
        mText.enabled = !mText.enabled;
    }

    public IEnumerator SetTemporaryText(string text)
    {
        SetText(text);
        yield return new WaitForSecondsRealtime(5f);
        SetText("");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        BoxCollider2D b = gameObject.GetComponent<BoxCollider2D>();
        b.enabled = false;
        gameManager.currentHouse = this;
        StartCoroutine(BrakeAtHouse());
        Debug.Log("Got to a house.");
        
    }

    private IEnumerator BrakeAtHouse()
    {
        Footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Brakes.start();
        gameManager.DisablePlayerMovementInput();
        gameManager.CreateDust();
        // play brake sound here?
        yield return new WaitForSecondsRealtime(1f);
        gameManager.OnGameStateChanged(GameState.Delivery);
    }

}
