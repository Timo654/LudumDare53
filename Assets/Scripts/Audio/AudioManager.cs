using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]

    public float musicVolume = 1;
    [Range(0, 1)]

    public float SFXVolume = 1;
    [Range(0, 1)]

    private Bus musicBus;

    private Bus sfxBus;


    private List<EventInstance> eventInstances;

    public static AudioManager instance { get; private set; }
    // public static AudioManager instance;
    private EventInstance musicEventInstance;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
    }

    public EventInstance CreateInstance(EventReference eventReference) {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void Start() {
        
    }

    private void Update() {
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(SFXVolume);
    }

    public void InitializeMusic(EventReference musicEventReference) {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();

    }

    private void CleanUp() {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
            
        }
    }

    private void OnDestroy() {
        CleanUp();
    }
}
