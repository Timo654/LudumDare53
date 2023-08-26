using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference playerFootsteps {get; private set;}
    [field: SerializeField] public EventReference playerBrakes {get; private set;}
    [field: SerializeField] public EventReference playerBox {get; private set;}
    
    [field: Header("Music")]
    [field: SerializeField] public EventReference mainmusic { get; private set; }
    [field: SerializeField] public EventReference menumusic { get; private set; }
    [field: SerializeField] public EventReference creditmusic { get; private set; }
    [field: SerializeField] public EventReference GoodEndingMusic { get; private set; }
    [field: SerializeField] public EventReference BadEndingMusic { get; private set; }
    [field: SerializeField] public EventReference SecretEndingMusic { get; private set; }
    

    [field: Header("UI")]
    [field: SerializeField] public EventReference click { get; private set; }
    [field: SerializeField] public EventReference verbclick { get; private set; }
    
    public static FMODEvents instance {get; private set;}

    private void Awake() {
        if (instance != null) {
            return;
        }
        instance = this;
    }
}
