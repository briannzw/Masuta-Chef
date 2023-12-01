using Cooking;
using Player.Input;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : PlayerInputControl
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text backToButtonText;
    [SerializeField] private TMP_Text backToText;

    [SerializeField] private GameObject settingsPanel;

    [Header("Type")]
    [SerializeField] private bool isCooking = false;

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        InputManager.ToggleActionMap(playerControls.Panel);

        backToButtonText.text = isCooking ? "Recipe Book" : "Level Selection";
        backToText.text = "Return to " + (isCooking ? "Recipe Book" : "Level Selection");
    }

    public void Resume()
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            return;
        }

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        InputManager.ToggleActionMap(!isCooking ? InputManager.PlayerAction.Gameplay : InputManager.PlayerAction.Cooking);
    }

    public void Back()
    {
        Time.timeScale = 1f;
        if (!isCooking) GameManager.Instance.BackToLevelSelection();
        else CookingManager.Instance.BackToRecipeBook();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        InputManager.ToggleActionMap(InputManager.PlayerAction.Gameplay);
        SceneManager.LoadScene("MainMenu");
    }

    protected override void RegisterInputCallbacks()
    {
        if (playerControls == null) return;
        if(!isCooking) playerControls.Gameplay.Pause.performed += (ctx) => Pause();
        else playerControls.Cooking.Pause.performed += (ctx) => Pause();
        playerControls.Panel.Cancel.performed += (ctx) => Resume();
    }

    protected override void UnregisterInputCallbacks()
    {
        if (playerControls == null) return;
        if(!isCooking) playerControls.Gameplay.Pause.performed -= (ctx) => Pause();
        else playerControls.Cooking.Pause.performed -= (ctx) => Pause();
        playerControls.Panel.Cancel.performed -= (ctx) => Resume();
    }
}
