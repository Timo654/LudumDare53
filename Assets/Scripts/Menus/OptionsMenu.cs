using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class OptionsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Toggle fullscreenToggle;
    public Button backButton;
    private PlayerControls playerControls;
    private InputAction settingsBack;
    [SerializeField] GameObject resolutionOption;
    private void Start()
    {
        playerControls = new PlayerControls();
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
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        if (Screen.fullScreen)
        {
            fullscreenToggle.isOn = true;
        }

        settingsBack = playerControls.Menu.MenuBack;
        settingsBack.Enable();
        settingsBack.started += OnBack;

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

    private void OnBack(InputAction.CallbackContext context)
    {
        backButton.onClick.Invoke();
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
