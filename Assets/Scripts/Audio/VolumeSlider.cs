using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioManager audioManager;

    private enum VolumeType {
        MUSIC,
        SFX,
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake() {
        volumeSlider = this.GetComponentInChildren<Slider>();
        audioManager = AudioManager.instance;
    }

    private void Update() {
        switch(volumeType) {
            case VolumeType.MUSIC:
                volumeSlider.value = audioManager.musicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = audioManager.SFXVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

public void OnSliderValueChanged()  {
    switch(volumeType) {
        case VolumeType.MUSIC:
            audioManager.musicVolume = volumeSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
            break;
        case VolumeType.SFX:
            audioManager.SFXVolume = volumeSlider.value;
            PlayerPrefs.SetFloat("SFXVolume", volumeSlider.value);
            break;
        default:
            Debug.LogWarning("Volume Type not supported: " + volumeType);
            break;
    }
}
}



