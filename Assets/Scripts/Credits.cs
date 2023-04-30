using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class Credits : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction escape;
    private InputAction interact;

    [SerializeField] private AudioManager audioManager;
    private EventInstance creditMusicEventInstance;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        escape = playerControls.Menu.Escape;
        escape.Enable();
        interact = playerControls.Menu.Interact;
        interact.Enable();
        escape.performed += Pause;
        interact.performed += Pause;
    }
    private void OnDisable()
    {
        escape.Disable();
        interact.Disable();
    }

    public void Pause(InputAction.CallbackContext context) // we're just going to skip credits when esc is pressed
    {
        OnCreditsEnd();
    }

    private void Start()
    {
        creditMusicEventInstance = AudioManager.instance.CreateInstance(FMODEvents.instance.creditmusic);
        creditMusicEventInstance.start();
    }

    // Start is called before the first frame update
    public void OnCreditsEnd()
    {
        StartCoroutine(CreditsEnd());
    }

    IEnumerator CreditsEnd()
    {
        creditMusicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        creditMusicEventInstance.release();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }
}
