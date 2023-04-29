using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New House", menuName = "House")]
[Serializable]
public class House : ScriptableObject
{
    public Sprite artwork;
    public DeliveryItem desired_item;
    public string[] dialogue; // maybe, not sure how to do it yet
}
