using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cooking
{
    using AYellowpaper.SerializedCollections;
    using Cooking.Gameplay;
    using Save;

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

        [SerializeField] private CookingEndResult cookingEndResult;

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

        public void CookingDone(CookingResult result, bool showPanel = true)
        {
            CurrentRecipe.data.CookingDone++;

            if (result == CookingResult.Perfect) CurrentRecipe.data.PerfectCookingDone++;
            else CurrentRecipe.data.ConsecutivePerfectCookingDone = 0;

            CurrentRecipe.data.CookingPoint += CookingPoints[result];
            
            // Reduce Ingredients
            foreach(var ingredient in CurrentRecipe.Ingredients)
            {
                ingredient.Key.data.Count -= ingredient.Value;
                SaveManager.SaveData.IngredientData[ingredient.Key.name] = ingredient.Key.data;
            }

            // Replace Data
            if(SaveManager.SaveData.RecipeData.Count == 0) SaveManager.Load();
            SaveManager.SaveData.RecipeData[CurrentRecipe.name] = CurrentRecipe.data;

            // Save RecipeData
            SaveManager.Save();

            cookingEndResult.gameObject.SetActive(showPanel);
            cookingEndResult.Set(result);
        }
        
        public void CookingFailed()
        {
            cookingEndResult.gameObject.SetActive(true);
            cookingEndResult.SetFail();
        }

        public void BackToRecipeBook()
        {
            if (SceneManager.GetActiveScene().name == "RecipeBook") return;

            Destroy(gameObject);
            InputManager.ToggleActionMap(InputManager.PlayerAction.Gameplay);
            SceneManager.LoadScene("RecipeBook");
        }

        public void LoadGame(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == initialScene) return;

            SaveManager.Load();
        }
    }
}