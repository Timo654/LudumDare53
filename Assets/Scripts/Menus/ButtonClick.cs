using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class ButtonClick : MonoBehaviour
{ 
    [SerializeField] AudioManager audioManager;
    private EventInstance ClickButton;
    void Start()
    {
        ClickButton = AudioManager.instance.CreateInstance(FMODEvents.instance.click);
    }

    public void ButtonPressed() {
        ClickButton.start();
    }

}
