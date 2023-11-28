using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Cooking.Recipe
{
    using Character.StatEffect;
    using Save;
    using Save.Data;

    [CreateAssetMenu(menuName = "Recipe List", fileName = "Recipe List", order = 0)]
    public class RecipeListSO : ScriptableObject
    {
        [Header("List")]
        public List<Recipe> Recipes = new();
        public List<Ingredient> Ingredients = new();

        [Header("Settings")]
        public List<int> UnlockSettings;
        public int RerollPointsNeeded;

        [Header("Stats Reroll")]
        [Range(0, 1)] public float UniqueStatChance;

        public int AutoCookUnlockSettings;

        public SerializedDictionary<Recipe, int> DefaultUniqueStat = new();
        public AddOnStatsSO CommonStatList;
        public AddOnStatsSO UniqueStatList;

        public AddOnStat GetDefaultUniqueStat(Recipe recipe)
        {
            return UniqueStatList.Stats[DefaultUniqueStat[recipe]];
        }

        public void PopulateData(SaveData saveData)
        {
            foreach (var recipe in Recipes)
            {
                recipe.data = saveData.RecipeData[recipe.name];
            }

            foreach (var ingredient in Ingredients)
            {
                ingredient.data = saveData.IngredientData[ingredient.name];
            }
        }

        public AddOnStat GetStatFromIndex(string index)
        {
            int.TryParse(index.Substring(1, index.Length - 1), out var id);

            if (index[0] == 'C')
            {
                return CommonStatList.Stats[id];
            }
            else if (index[0] == 'U')
            {
                return UniqueStatList.Stats[id];
            }

            return null;
        }
    }
}