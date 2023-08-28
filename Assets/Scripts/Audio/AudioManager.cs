using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

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
    private Bus reverbBus;
    private Bus uiBus;

    private List<EventInstance> eventInstances;

    public static AudioManager _instance { get; private set; }
    // public static AudioManager instance;
    private EventInstance musicEventInstance;


    // public access for the Singleton
    // and lazy instantiation if not exists
    public static AudioManager Instance
    {
        get
        {
            // if exists directly return
            if (_instance) return _instance;

            // otherwise search it in the scene
            _instance = FindObjectOfType<AudioManager>();

            // found it?
            if (_instance) return _instance;

            // otherwise create and initialize it
            CreateInstance();

            return _instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateInstance()
    {
        // skip if already exists
        if (_instance) return;
        InitializeInstance(new GameObject(nameof(AudioManager)).AddComponent<AudioManager>());
    }

    private static void InitializeInstance(AudioManager instance)
    {
        _instance = instance;
        DontDestroyOnLoad(_instance.gameObject);
        _instance.eventInstances = new List<EventInstance>();
        _instance.musicBus = RuntimeManager.GetBus("bus:/Music");
        _instance.sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _instance.SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        _instance.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

    }

    private void Awake() {
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        InitializeInstance(this);
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

    public void FadeOutMusic()
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicEventInstance.release();

    }

    private void CleanUp() {
        if (eventInstances != null)
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
        
    }

    private void OnDestroy() {
        CleanUp();
    }
}
