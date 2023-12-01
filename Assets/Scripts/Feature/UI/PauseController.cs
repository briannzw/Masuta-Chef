using Player.Input;
using UnityEngine;

public class PauseController : PlayerInputControl
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;

    [Header("Type")]
    [SerializeField] private bool isCooking = false;

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        InputManager.ToggleActionMap(playerControls.Panel);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        InputManager.ToggleActionMap(!isCooking ? InputManager.PlayerAction.Gameplay : InputManager.PlayerAction.Cooking);
    }

    protected override void RegisterInputCallbacks()
    {
        if (playerControls == null) return;
        playerControls.Gameplay.Pause.performed += (ctx) => Pause();
        playerControls.Panel.Cancel.performed += (ctx) => Resume();
    }

    protected override void UnregisterInputCallbacks()
    {
        if (playerControls == null) return;
        playerControls.Gameplay.Pause.performed -= (ctx) => Pause();
        playerControls.Panel.Cancel.performed -= (ctx) => Resume();
    }
}
