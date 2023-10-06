using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameManager._instance.OnGameStateChanged(GameState.Win);
        Debug.Log("Entered ending area ");
    }
}
