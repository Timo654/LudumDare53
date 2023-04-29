using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HouseObject : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private DataContainer gameData;
    public Sprite artwork;
    public Person resident;
    public DeliveryItem item;
    private TextMeshPro mText;
    private void Awake()
    {
        item = new DeliveryItem(gameData.Items[Random.Range(0, gameData.Items.Length)], 1);
        resident = new Person(gameData.personSprites[Random.Range(0, gameData.personSprites.Length)], item);
        artwork = gameData.houseSprites[Random.Range(0, gameData.houseSprites.Length)];
        GetComponent<SpriteRenderer>().sprite = artwork;
        GetComponent<SpriteRenderer>().color = Color.white;
        mText = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
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
        BoxCollider2D p = gameObject.GetComponent<BoxCollider2D>();
        p.enabled = false;
        gameManager.currentHouse = this;
        gameManager.OnGameStateChanged(GameState.Delivery);
        Debug.Log("Got to a house.");
    }


}
