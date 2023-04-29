using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data Container", menuName = "Data Container")]
[Serializable]
public class DataContainer : ScriptableObject
{
    public Sprite[] houseSprites;
    public Sprite[] personSprites;
    public string[] personNames;
    public DeliveryItem[] Items;


}
