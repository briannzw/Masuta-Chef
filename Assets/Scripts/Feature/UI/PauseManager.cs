using System.Collections;
using System.Collections.Generic;
using Player.Input;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : PlayerInputControl
{
    #region Events
    public delegate void OnPauseDelegate();
    public static event OnPauseDelegate OnPauseAction;

    public delegate void OnResumeDelegate();
    public static event OnResumeDelegate OnResumeAction;
    #endregion
    
    [SerializeField] private GameObject pauseUI;
    private bool isPaused;
    private float currentTimeScale = 1;
    

    protected override void Start() {
        base.Start();
        isPaused = pauseUI.activeSelf;
        Resume();
    }

    #region Callbacks
    protected override void RegisterInputCallbacks()
    {
        if (playerControls == null) return;

        playerControls.Gameplay.Pause.Enable();
        playerControls.Gameplay.Pause.started += OnInputActionStart;
        playerControls.Panel.Cancel.started += OnInputActionStart;
    }

    protected override void UnregisterInputCallbacks()
    {
        if (playerControls == null) return;

        playerControls.Panel.Cancel.started -= OnInputActionStart;
        playerControls.Gameplay.Pause.started -= OnInputActionStart;
        playerControls.Gameplay.Pause.Disable();
    }
    #endregion

    #region Method
    private void OnInputActionStart(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            playerControls.Panel.Disable();
            playerControls.Gameplay.Enable();
            Resume();
        }
        else
        {
            playerControls.Gameplay.Disable();
            playerControls.Panel.Enable();
            Pause();
        }

        pauseUI.SetActive(!isPaused);
        isPaused = !isPaused;
    }

    private void Pause()
    {
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;

        if(OnPauseAction != null)
        {
            OnPauseAction();
        }
    }

    private void Resume()
    {
        Time.timeScale = currentTimeScale;

        if(OnPauseAction != null)
        {
            OnResumeAction();
        }
    }
    #endregion
}
