using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        BoxCollider2D b = gameObject.GetComponent<BoxCollider2D>();
        b.enabled = false;
        Debug.Log("Got near a house.");
    }
}
