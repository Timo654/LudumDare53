using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Toggle fullscreenToggle;
    [SerializeField] GameObject resolutionOption;
    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            resolutionOption.SetActive(false);
        }
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new();
        int currentResolutionIndex = 0;
        for (int i=0; i < resolutions.Length;i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        // TODO
        //sfxSlider.value = 
        //musicSlider.value = 
        if (Screen.fullScreen)
        {
            fullscreenToggle.isOn = true;
        }
    }
    public void SetSFXVolume (float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        //SetSoundVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        //SetMusicVolume(volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
