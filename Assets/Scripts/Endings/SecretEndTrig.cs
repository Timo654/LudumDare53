using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretEndTrig : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameManager._instance.OnGameStateChanged(GameState.SecretWin);
        Debug.Log("Entered ending area ");
    }
}
