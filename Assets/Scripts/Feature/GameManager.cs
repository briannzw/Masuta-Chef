using Level;
using UnityEngine;
using Save;
using MyBox;
using Character;
using System;
using UnityEngine.AI;
using UnityEditor;

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

        if(LoadOnAwake) LoadGame();
    }
    #endregion

    [Header("Settings")]
    public bool LoadOnAwake;
    public bool SaveOnGameOver;

    [Header("Static References")]
    public Transform PlayerTransform;
    public LevelManager LevelManager;
    public SaveManager SaveManager;
    public StatsManager StatsManager;
    public float AIAvoidancePredictionTime = 0.7f; //Default value is 2.0, Describes how far in the future the agents predict collisions for avoidance.

    [Header("Static Value")]
    public Action OnEnemiesKilled;


    private void Start()
    {
        NavMesh.avoidancePredictionTime = AIAvoidancePredictionTime;
        if (SaveOnGameOver)
        {
            LevelManager.OnLevelLose += SaveGame;
            LevelManager.OnLevelWin += SaveGame;
        }
    }

    [ButtonMethod]
    public void SaveGame()
    {
        SaveManager.Save();
    }

    [ButtonMethod]
    public void LoadGame()
    {
        SaveManager.Load();
        // Load Recipe Book Stat Mods
        if(Application.isPlaying) StatsManager.Load();
    }

    [ButtonMethod]
    public void NewGame()
    {
        if(StatsManager.RecipeSO == null || StatsManager.RecipeSO.Recipes.Count == 0 || StatsManager.RecipeSO.Ingredients.Count == 0)
        {
            Debug.LogError("Please recheck if StatsManager RecipeSO is defined before proceeding.");
            return;
        }

        Unsupported.SmartReset(SaveManager);

        foreach (var recipe in StatsManager.RecipeSO.Recipes)
        {
            recipe.data = new();
            SaveManager.SaveData.Add(recipe, 0);
        }

        foreach(var ingredient in StatsManager.RecipeSO.Ingredients)
        {
            ingredient.data = new();
            SaveManager.SaveData.Add(ingredient, 0);
        }
    }
}