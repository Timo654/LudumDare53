using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        gameManager.OnGameStateChanged(GameState.Win);
        Debug.Log("Entered ending area ");
    }
}
