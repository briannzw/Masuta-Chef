using UnityEngine;

namespace Cooking
{
    using AYellowpaper.SerializedCollections;
    using Save;
    using UnityEngine.SceneManagement;

    public enum CookingType { Slider, TapNumber, Circular }
    public class CookingManager : MonoBehaviour
    {
        #region Singleton
        public static CookingManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else Instance = this;

            initialScene = SceneManager.GetActiveScene().name;
        }
        #endregion

        [Header("References")]
        public SerializedDictionary<CookingType, string> CookingScenes = new();
        public SaveManager SaveManager;

        [Header("Points")]
        public SerializedDictionary<CookingResult, int> CookingPoints = new();

        [Header("Gameplay")]
        public Recipe.Recipe CurrentRecipe;

        public static int selectedIndex = 0;

        private string initialScene;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += LoadGame;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= LoadGame;
        }

        public void CookingDone(CookingResult result)
        {
            CurrentRecipe.data.CookingDone++;

            if (result == CookingResult.Perfect) CurrentRecipe.data.PerfectCookingDone++;
            else CurrentRecipe.data.ConsecutivePerfectCookingDone = 0;

            CurrentRecipe.data.CookingPoint += CookingPoints[result];

            // Replace Data
            SaveManager.SaveData.RecipeData[CurrentRecipe.name] = CurrentRecipe.data;

            // Save RecipeData
            SaveManager.Save();

            // TEMP
            BackToRecipeBook();
        }

        //Temp
        public void BackToRecipeBook()
        {
            SceneManager.LoadScene("RecipeBook");
        }

        public void LoadGame(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == initialScene) return;

            SaveManager.Load();
        }
    }
}