using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data Container")]
[Serializable]
public class DataContainer : ScriptableObject
{
    public Sprite[] houseSprites;
    public Sprite[] personSprites;
    public DeliveryItemData[] Items;
    public Sprite[] treeSprites;
    public Sprite[] cloudSprites;
    public string goodEndScene;
    public string badEndScene;
    public string secretEndScene;
    public EventReference gameplayMusic;
    public EventReference badEndMusic;
    public EventReference goodEndMusic;
    public EventReference secretEndMusic;

}
