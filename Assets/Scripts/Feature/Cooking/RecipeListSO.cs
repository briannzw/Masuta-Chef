using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Cooking.Recipe
{
    using Character.StatEffect;

    [CreateAssetMenu(menuName = "Recipe List", fileName = "Recipe List", order = 0)]
    public class RecipeListSO : ScriptableObject
    {
        [Header("List")]
        public List<Recipe> Recipes = new();
        public List<Ingredient> Ingredients = new();

        [Header("Settings")]
        public List<int> UnlockSettings;

        public int AutoCookUnlockSettings;

        public SerializedDictionary<Recipe, int> DefaultUniqueStat = new();
        public List<AddOnStat> UniqueStatList = new();

        public AddOnStat GetDefaultUniqueStat(Recipe recipe)
        {
            return UniqueStatList[DefaultUniqueStat[recipe]];
        }
    }
}