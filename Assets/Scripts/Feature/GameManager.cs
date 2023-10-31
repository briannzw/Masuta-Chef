using Level;
using UnityEngine;
using Save;
using MyBox;
using Character;
using System;

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

    private void Start()
    {
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
}