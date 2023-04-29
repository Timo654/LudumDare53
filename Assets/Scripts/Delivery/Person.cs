using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Person", menuName = "Person")]
[Serializable]
public class Person : ScriptableObject
{
    public Sprite artwork;
    public DeliveryItem desiredItem;
}
