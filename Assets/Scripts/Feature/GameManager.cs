using Level;
using UnityEngine;
using Save;
using MyBox;

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
    }
    #endregion

    [Header("Static References")]
    public Transform PlayerTransform;
    public LevelManager LevelManager;
    public SaveManager SaveManager;


    [ButtonMethod]
    public void SaveGame()
    {
        SaveManager.Save();
    }

    [ButtonMethod]
    public void LoadGame()
    {
        SaveManager.Load();
    }
}