using UnityEngine;
using UnityEngine.InputSystem;

public class Credits : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction escape;
    private InputAction interact;

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
        AudioManager._instance.InitializeMusic(FMODEvents.instance.creditmusic);
    }

    public void OnCreditsEnd()
    {
        LevelChanger._instance.FadeToLevel("MainMenu");
    }
}
