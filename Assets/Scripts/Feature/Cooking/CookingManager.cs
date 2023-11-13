using UnityEngine;

namespace Cooking
{
    using AYellowpaper.SerializedCollections;

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
        }
        #endregion

        [Header("References")]
        public SerializedDictionary<CookingType, string> CookingScenes = new();

        [Header("Points")]
        public SerializedDictionary<CookingResult, int> CookingPoints = new();

        [Header("Gameplay")]
        public Recipe.Recipe CurrentRecipe;

        public void CookingDone(CookingResult result)
        {
            CurrentRecipe.data.CookingDone++;

            if (result == CookingResult.Perfect) CurrentRecipe.data.PerfectCookingDone++;
            else CurrentRecipe.data.ConsecutivePerfectCookingDone = 0;

            GameManager.Instance.SaveManager.SaveData.CookingPoints += CookingPoints[result];

            // Save RecipeData
            GameManager.Instance.SaveGame();
        }
    }
}