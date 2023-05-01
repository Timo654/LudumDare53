using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class ButtonClick : MonoBehaviour
{ 
    private EventInstance ClickButton;
    void Start()
    {
        ClickButton = AudioManager._instance.CreateInstance(FMODEvents.instance.click);
    }

    public void ButtonPressed() {
        ClickButton.start();
    }

}
