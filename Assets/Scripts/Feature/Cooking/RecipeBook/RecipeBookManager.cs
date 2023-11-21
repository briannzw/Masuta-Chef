using MyBox;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Cooking.RecipeBook
{
    using Character.Stat;
    using Character.StatEffect;
    using Recipe;
    using Recipe.UI;
    using Save;
    using UnityEngine.SceneManagement;

    public class RecipeBookManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SaveManager SaveManager;
        [SerializeField] private CookingManager CookingManager;

        [Header("Recipes")]
        [SerializeField] private RecipeListSO recipeSO;
        public int initialRecipeIndex = 0;

        [Header("Recipe File")]
        [SerializeField] private TMP_Text cookingPointText;
        [SerializeField] private TMP_Text recipeNameText;
        [SerializeField] private Image recipeIcon;
        [SerializeField] private Transform recipeIngredientParent;
        [SerializeField] private GameObject recipeIngredientItemPrefab;
        [SerializeField] private RecipeStatItem[] recipeStatItems;

        [Header("Recipe Notes")]
        [SerializeField] private Transform recipeNotesParent;
        [SerializeField] private int initialCount = 6;
        [SerializeField] private GameObject recipeNotePrefab;

        [Header("Locks")]
        [SerializeField] private GameObject RerollLock;
        [SerializeField] private GameObject AutoCookLock;
        [SerializeField] private TMP_Text AutoCookLockText;

        private Recipe currentRecipe;

        private void Awake()
        {
            // Load SaveData on Awake
            SaveManager.Load();
            FetchData();
        }

        private void Start()
        {
            // Add one to initialCount (initial Recipe being skipped)
            initialCount++;
            SetUI(recipeSO.Recipes[initialRecipeIndex]);
        }

        private void UpdateNoteUI()
        {
            foreach (Transform child in recipeNotesParent.transform) Destroy(child.gameObject);

            for (int i = 0; i < initialCount; i++)
            {
                if (i < recipeSO.Recipes.Count && currentRecipe == recipeSO.Recipes[i]) continue;

                var go = Instantiate(recipeNotePrefab, recipeNotesParent);
                var recipeItem = go.GetComponent<RecipeNoteItem>();
                recipeItem.manager = this;

                if (i < recipeSO.Recipes.Count)
                {
                    recipeItem.Set(recipeSO.Recipes[i]);

                    // Set then Lock
                    if (recipeSO.Recipes[i].data.IsLocked) recipeItem.Lock(recipeSO.Recipes[i].data);
                }
                else recipeItem.Lock();
            }
        }

        public void SetUI(Recipe recipe)
        {
            // Data
            currentRecipe = recipe;

            UpdateNoteUI();

            // Cooking Points
            cookingPointText.text = $"{recipe.data.CookingPoint} CP";

            // Title
            recipeNameText.text = recipe.name;

            // Icon
            recipeIcon.sprite = recipe.Icon;

            // Ingredients
            foreach (Transform child in recipeIngredientParent.transform) Destroy(child.gameObject);
            foreach (var ingredient in recipe.Ingredients)
            {
                var go = Instantiate(recipeIngredientItemPrefab, recipeIngredientParent);
                go.GetComponent<IngredientItem>().Set(ingredient.Key, ingredient.Value);
            }

            // Stats
            for (int i = 0; i < recipe.Stats.Length; i++)
            {
                // Replace Stat 3 with Unique Stat
                if (i >= 2 && recipe.data.UniqueStatIndex != -1)
                {
                    recipe.Stats[i] = recipeSO.UniqueStatList[recipe.data.UniqueStatIndex];
                }

                SetRecipeStatUI(recipeStatItems[i], recipe.Stats[i]);

                if (recipe.data.CookingDone >= recipeSO.UnlockSettings[i]) recipeStatItems[i].Unlock();
                else
                {
                    recipeStatItems[i].LockText.text = $"{recipe.data.CookingDone} / {recipeSO.UnlockSettings[i]} Cooking Done";
                }
            }

            // Locks
            RerollLock.SetActive(false);

            // Auto Cook
            AutoCookLock.SetActive(recipe.data.PerfectCookingDone < recipeSO.AutoCookUnlockSettings);
            AutoCookLockText.text = $"{recipe.data.PerfectCookingDone} / {recipeSO.AutoCookUnlockSettings}\nPerfect Cooking";
        }

        private void SetRecipeStatUI(RecipeStatItem item, AddOnStat stat)
        {
            stat.Stat.Initialize();

            item.StatsText.text = $"+{stat.Stat.Modifier.Value} ";
            item.StatsText.text += (stat.AffectedCharacter == "Player") ? "" : (stat.AffectedCharacter + " ");
            if (stat.Stat.StatType == StatsType.Character)
            {
                item.StatsText.text += (stat.Stat.AffectDynamicStat) ? StatAbbreviation.Get(stat.Stat.DynamicStatsAffected) : StatAbbreviation.Get(stat.Stat.StatsAffected);
            }
            else item.StatsText.text += StatAbbreviation.Get(stat.Stat.WeaponStatsAffected);
        }

        public void Cook()
        {
            DontDestroyOnLoad(CookingManager.gameObject);

            CookingManager.CurrentRecipe = currentRecipe;

            SceneManager.LoadScene(CookingManager.CookingScenes[currentRecipe.CookingType]);
        }

        public void Back()
        {
            Destroy(CookingManager.gameObject);

            SceneManager.LoadScene("MainMenu");
        }

        private void FetchData()
        {
            foreach(var recipe in recipeSO.Recipes)
            {
                recipe.data = SaveManager.SaveData.RecipeData[recipe.name];
            }

            foreach(var ingredient in recipeSO.Ingredients)
            {
                ingredient.data = SaveManager.SaveData.IngredientData[ingredient.name];
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
        }
    }
}