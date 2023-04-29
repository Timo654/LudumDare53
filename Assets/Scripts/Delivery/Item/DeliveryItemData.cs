using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Delivery Item", menuName = "Delivery Item")]
[Serializable]
public class DeliveryItemData : ScriptableObject
{
    public string itemName;
    public Sprite artwork;
    public string[] text;
}
