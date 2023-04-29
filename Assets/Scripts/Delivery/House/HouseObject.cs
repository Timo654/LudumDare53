using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseObject : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private DataContainer gameData;
    public Sprite artwork;
    public Person resident;
    public DeliveryItem item;
    private void Awake()
    {
        item = new DeliveryItem(gameData.Items[Random.Range(0, gameData.Items.Length)], 1);
        resident = new Person(gameData.personSprites[Random.Range(0, gameData.personSprites.Length)], item);
        artwork = gameData.houseSprites[Random.Range(0, gameData.houseSprites.Length)];
        GetComponent<SpriteRenderer>().sprite = artwork;
        GetComponent<SpriteRenderer>().color = Color.white;
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
