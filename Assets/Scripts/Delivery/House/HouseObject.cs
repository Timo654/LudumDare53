using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class HouseObject : MonoBehaviour
{
    private DataContainer gameData;
    public Sprite artwork;
    public Person resident;
    public DeliveryItem item;
    private TextMeshPro mText;
    private SpriteRenderer personSprite;

    [SerializeField] private AudioManager audioManager;
    private EventInstance footsteps;
    private EventInstance brake;

    private void Awake()
    {
        gameData = GameManager._instance.gameData;
        item = new DeliveryItem(gameData.Items[Random.Range(0, gameData.Items.Length)], 1);
        resident = new Person(gameData.personSprites[Random.Range(0, gameData.personSprites.Length)], item);
        artwork = gameData.houseSprites[Random.Range(0, gameData.houseSprites.Length)];
        GetComponent<SpriteRenderer>().sprite = artwork;
        GetComponent<SpriteRenderer>().color = Color.white;
        mText = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        personSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        personSprite.sprite = resident.GetArtwork();
        footsteps = GameManager._instance.GetFootsteps();
        brake = AudioManager._instance.CreateInstance(FMODEvents.instance.playerBrakes);
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
        GameManager._instance.currentHouse = this;
        StartCoroutine(BrakeAtHouse());
        Debug.Log("Got to a house.");
        
    }

    private IEnumerator BrakeAtHouse()
    {
        footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        brake.start();
        GameManager._instance.DisablePlayerMovementInput();
        GameManager._instance.CreateDust();
        yield return new WaitForSecondsRealtime(1f);
        GameManager._instance.OnGameStateChanged(GameState.Delivery);
    }

}
