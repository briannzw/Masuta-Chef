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
        [SerializeField] private Button recipeRerollButton;

        [Space]
        [SerializeField] private Image recipeStatRarity;
        [SerializeField] private TMP_Text recipeStatRarityText;
        [SerializeField] private Color[] recipeStatRarityBackgroundColor;
        [SerializeField] private Color[] recipeStatRarityTextColor;

        [Header("Recipe Notes")]
        [SerializeField] private Transform recipeNotesParent;
        [SerializeField] private int initialCount = 6;
        [SerializeField] private GameObject recipeNotePrefab;

        [Header("Locks")]
        [SerializeField] private GameObject RerollLock;
        [SerializeField] private GameObject AutoCookLock;
        [SerializeField] private TMP_Text AutoCookLockText;

        [Header("Reroll Panel")]
        [SerializeField] private TMP_Text prevStatText;
        [SerializeField] private Image prevStatRarityBackground;
        [SerializeField] private TMP_Text prevStatRarityText;
        [SerializeField] private TMP_Text nextStatText;
        [SerializeField] private Image nextStatRarityBackground;
        [SerializeField] private TMP_Text nextStatRarityText;

        private Recipe currentRecipe;
        private string currentReroll;

        private void Awake()
        {
            // Load SaveData on Awake
            SaveManager.Load();
            initialRecipeIndex = CookingManager.selectedIndex;
            recipeSO.PopulateData(SaveManager.SaveData);
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
                    if (recipeSO.Recipes[i].IsLocked) recipeItem.Lock(recipeSO.Recipes[i]);
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
                if (i >= 2)
                {
                    if(recipe.data.CookingDone >= recipeSO.UnlockSettings[i] && !recipe.data.IsStat3Assigned)
                    {
                        recipe.data.StatsIndex = Reroll();
                        // Save after First time Reroll
                        SaveManager.Save();
                    }
                    if (recipe.data.IsStat3Assigned)
                    {
                        recipe.Stats[i] = recipeSO.GetStatFromIndex(recipe.data.StatsIndex);
                        recipeStatRarityText.text = recipe.data.StatsIndex[0].ToString();
                        recipeStatRarity.color = recipeStatRarityBackgroundColor[(recipeStatRarityText.text == "C") ? 0 : 1];
                        recipeStatRarityText.color = recipeStatRarityTextColor[(recipeStatRarityText.text == "C") ? 0 : 1];
                    }

                    recipeStatRarity.gameObject.SetActive(recipe.data.IsStat3Assigned);
                }

                recipeStatItems[i].StatsText.text = GetStatInfo(recipe.Stats[i]);

                if (recipe.data.CookingDone >= recipeSO.UnlockSettings[i])
                {
                    recipeStatItems[i].Unlock();
                }
                else
                {
                    recipeStatItems[i].LockText.text = $"{recipe.data.CookingDone} / {recipeSO.UnlockSettings[i]} Cooking Done";
                }
            }

            // Locks
            RerollLock.SetActive(!(recipe.data.IsStat3Assigned));

            recipeRerollButton.GetComponentInChildren<TMP_Text>().text = $"Reroll ({recipeSO.RerollPointsNeeded} CP)";
            recipeRerollButton.interactable = recipe.data.CookingPoint >= recipeSO.RerollPointsNeeded;

            // Auto Cook
            AutoCookLock.SetActive(recipe.data.PerfectCookingDone < recipeSO.AutoCookUnlockSettings);
            AutoCookLockText.text = $"{recipe.data.PerfectCookingDone} / {recipeSO.AutoCookUnlockSettings}\nPerfect Cooking";
        }

        private string GetStatInfo(AddOnStat stat, bool reverseSign = false)
        {
            string value = "";

            stat.Stat.Initialize();

            if(!reverseSign) value = $"+{stat.Stat.Modifier.Value} ";

            value += (stat.AffectedCharacter == "Player") ? "" : (stat.AffectedCharacter + " ");
            if (stat.Stat.StatType == StatsType.Character)
            {
                value += (stat.Stat.AffectDynamicStat) ? StatAbbreviation.Get(stat.Stat.DynamicStatsAffected) : StatAbbreviation.Get(stat.Stat.StatsAffected);
            }
            else value += StatAbbreviation.Get(stat.Stat.WeaponStatsAffected);

            if(reverseSign) value += $" +{stat.Stat.Modifier.Value}";

            return value;
        }

        public void Cook()
        {
            DontDestroyOnLoad(CookingManager.gameObject);

            CookingManager.CurrentRecipe = currentRecipe;

            SceneManager.LoadScene(CookingManager.CookingScenes[currentRecipe.CookingType]);
        }

        private string Reroll()
        {
            string statIndex;

            float randValue = Random.value;
            if(randValue < recipeSO.UniqueStatChance)
            {
                statIndex = 'U' + Random.Range(0, recipeSO.UniqueStatList.Stats.Count).ToString("000");
            }
            else
            {
                statIndex = 'C' + Random.Range(0, recipeSO.CommonStatList.Stats.Count).ToString("000");
            }

            return statIndex;
        }

        public void OnReroll()
        {
            currentReroll = Reroll();

            currentRecipe.data.CookingPoint -= recipeSO.RerollPointsNeeded;
            SetUI(currentRecipe);
            SaveManager.Save();

            prevStatText.text = GetStatInfo(currentRecipe.Stats[2], true);
            nextStatText.text = GetStatInfo(recipeSO.GetStatFromIndex(currentReroll), true);

            // Rarity
            prevStatRarityText.text = (recipeStatRarityText.text[0] == 'C') ? "COMMON" : "UNIQUE"; ;
            prevStatRarityText.color = recipeStatRarityText.color;
            prevStatRarityBackground.color = recipeStatRarity.color;
            nextStatRarityText.text = (currentReroll[0] == 'C') ? "COMMON" : "UNIQUE";
            nextStatRarityText.color = recipeStatRarityTextColor[(currentReroll[0] == 'C') ? 0 : 1];
            nextStatRarityBackground.color = recipeStatRarityBackgroundColor[(currentReroll[0] == 'C') ? 0 : 1];
        }

        public void OnRerollAccepted()
        {
            currentRecipe.data.StatsIndex = currentReroll;
            SaveManager.Save();

            SetUI(currentRecipe);
        }

        public void Back()
        {
            Destroy(CookingManager.gameObject);

            SceneManager.LoadScene("Menu");
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


        [ButtonMethod]
        public void NewGame()
        {
            if (recipeSO == null || recipeSO.Recipes.Count == 0 || recipeSO.Ingredients.Count == 0)
            {
                Debug.LogError("Please recheck if recipeSO is defined before proceeding.");
                return;
            }

            foreach (var recipe in recipeSO.Recipes)
            {
                SaveManager.SaveData.Add(recipe, 0);
            }

            foreach (var ingredient in recipeSO.Ingredients)
            {
                SaveManager.SaveData.Add(ingredient, 0);
            }

            SaveGame();
        }
    }
}