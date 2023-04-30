using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretEndTrig : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        gameManager.OnGameStateChanged(GameState.SecretWin);
        Debug.Log("Entered ending area ");
    }
}
