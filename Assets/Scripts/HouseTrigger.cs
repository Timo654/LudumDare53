using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D col)
    {
        gameManager.OnGameStateChanged(GameState.Delivery);
        Debug.Log("Got to a house.");
    }
}
