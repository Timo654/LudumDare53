using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New House", menuName = "House")]
[Serializable]
public class House : ScriptableObject
{
    public Sprite artwork;
    public Person resident;
}
