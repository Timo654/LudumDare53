using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Delivery Item", menuName = "Delivery Item")]
[Serializable]
public class DeliveryItem : ScriptableObject
{
    public string item_name;
    public Sprite artwork;
    public uint count;
    public string[] text;
}
