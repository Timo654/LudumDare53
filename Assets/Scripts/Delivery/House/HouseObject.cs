using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseObject : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private House _house;
    private void OnTriggerEnter2D(Collider2D col)
    {
        gameManager.currentHouse = _house;
        gameManager.OnGameStateChanged(GameState.Delivery);
        Debug.Log("Got to a house.");
    }

    public void SetHouse(House house)
    {
        _house = house;
    }

    public House GetHouse()
    {
        return _house;
    }

}
