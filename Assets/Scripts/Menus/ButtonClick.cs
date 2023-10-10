using FMOD.Studio;
using UnityEngine;


public class ButtonClick : MonoBehaviour
{
    private EventInstance ClickButton;
    void Start()
    {
        ClickButton = AudioManager._instance.CreateInstance(FMODEvents.instance.click);
    }

    public void ButtonPressed()
    {
        ClickButton.start();
    }

}
