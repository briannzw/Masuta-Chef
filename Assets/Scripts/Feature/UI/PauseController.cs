using Cooking;
using Player.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : PlayerInputControl
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text backToButtonText;
    [SerializeField] private TMP_Text backToText;

    [SerializeField] private GameObject settingsPanel;

    [Header("Type")]
    [SerializeField] private bool isCooking = false;

    [Header("Scene Load")]
    [SerializeField] private GameObject sceneLoadPrefab;

    [Header("How TO")]
    [SerializeField] private GameObject howToPanel;

    protected override void Start()
    {
        base.Start();
        if (!isCooking) return;
        HowToPause();
        howToPanel.SetActive(true);
    }

    public void HowToPause()
    {
        Time.timeScale = 0f;
        InputManager.ToggleActionMap(playerControls.Panel);
    }

    public void HowToResume()
    {
        Time.timeScale = 1f;
        InputManager.ToggleActionMap(!isCooking ? InputManager.PlayerAction.Gameplay : InputManager.PlayerAction.Cooking);
    }

    public void Pause(InputAction.CallbackContext context)
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

        if (howToPanel != null && howToPanel.activeSelf)
        {
            howToPanel.SetActive(false);
            HowToResume();
            return;
        }

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        InputManager.ToggleActionMap(!isCooking ? InputManager.PlayerAction.Gameplay : InputManager.PlayerAction.Cooking);
    }

    public void Resume(InputAction.CallbackContext context)
    {
        Resume();
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
        LoadScene("MainMenu");
    }

    private void LoadScene(string sceneName, int panelIndex = -1)
    {
        GameObject go = Instantiate(sceneLoadPrefab);
        go.GetComponent<SceneLoad>().loadingIndex = panelIndex;
        go.GetComponent<SceneLoad>().LoadScene(sceneName);
    }


    protected override void RegisterInputCallbacks()
    {
        if (playerControls == null) return;
        if(!isCooking) playerControls.Gameplay.Pause.performed += Pause;
        else playerControls.Cooking.Pause.performed += Pause;
        playerControls.Panel.Cancel.performed += Resume;
    }

    protected override void UnregisterInputCallbacks()
    {
        if (playerControls == null) return;
        if(!isCooking) playerControls.Gameplay.Pause.performed -= Pause;
        else playerControls.Cooking.Pause.performed -= Pause;
        playerControls.Panel.Cancel.performed -= Resume;
    }
}
