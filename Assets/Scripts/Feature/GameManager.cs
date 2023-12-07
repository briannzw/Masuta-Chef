using Level;
using UnityEngine;
using Save;
using MyBox;
using Character;
using System;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else Instance = this;

        if (LoadOnAwake) LoadGame();
        if (LevelManager != null && SelectedLevel != null) LevelManager.CurrentLevel = SelectedLevel;
    }
    #endregion

    [Header("Settings")]
    public bool LoadOnAwake = true;
    public bool SaveOnGameOver = true;

    [Header("Static References")]
    public Transform PlayerTransform;
    public LevelManager LevelManager;
    public SaveManager SaveManager;
    public StatsManager StatsManager;
    public float AIAvoidancePredictionTime = 0.7f; //Default value is 2.0, Describes how far in the future the agents predict collisions for avoidance.

    [Header("Static Value")]
    public Action OnEnemiesKilled;
    public static LevelData SelectedLevel;

    [Header("Scene Load")]
    [SerializeField] private GameObject sceneLoadPrefab;

    private void Start()
    {
        NavMesh.avoidancePredictionTime = AIAvoidancePredictionTime;
        if (SaveOnGameOver)
        {
            LevelManager.OnLevelLose += SaveGame;
            LevelManager.OnLevelWin += SaveGame;
        }
    }

    public void BackToLevelSelection()
    {
        if (SceneManager.GetActiveScene().name == "LevelSelection") return;

        ResumeGame();
        LoadScene("LevelSelection");
    }

    public void BackToMenu()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;

        ResumeGame();
        LoadScene("MainMenu");
    }

    private void LoadScene(string sceneName, int panelIndex = -1)
    {
        GameObject go = Instantiate(sceneLoadPrefab);
        go.GetComponent<SceneLoad>().loadingIndex = panelIndex;
        go.GetComponent<SceneLoad>().LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        InputManager.ToggleActionMap(InputManager.PlayerAction.Gameplay);
    }

    [ButtonMethod]
    public void SaveGame()
    {
        SaveManager.Save();

        if (SaveOnGameOver)
        {
            LevelManager.OnLevelLose -= SaveGame;
            LevelManager.OnLevelWin -= SaveGame;
        }
    }

    [ButtonMethod]
    public void LoadGame()
    {
        if (!SaveManager.CheckDataExists()) NewGame();

        SaveManager.Load();
        // Load Recipe Book Stat Mods
        if(Application.isPlaying && StatsManager != null) StatsManager.Load();
    }

    [ButtonMethod]
    public void NewGame()
    {
        if(StatsManager.RecipeSO == null || StatsManager.RecipeSO.Recipes.Count == 0 || StatsManager.RecipeSO.Ingredients.Count == 0)
        {
            Debug.LogError("Please recheck if StatsManager RecipeSO is defined before proceeding.");
            return;
        }

#if UNITY_EDITOR
        Unsupported.SmartReset(SaveManager);
#endif
        foreach (var recipe in StatsManager.RecipeSO.Recipes)
        {
            recipe.data = new();
            SaveManager.SaveData.New(recipe);
        }

        foreach(var ingredient in StatsManager.RecipeSO.Ingredients)
        {
            ingredient.data = new();
            SaveManager.SaveData.New(ingredient);
        }

        SaveGame();
    }
}