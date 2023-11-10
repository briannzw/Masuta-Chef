using System.Collections.Generic;
using UnityEngine;

namespace Cooking.Recipe
{
    using AYellowpaper.SerializedCollections;
    using Stat;

    [CreateAssetMenu(menuName = "Cooking/Recipe", fileName = "New Recipe")]
    public class Recipe : ScriptableObject
    {
        [Header("Information")]
        public Sprite Icon;
        public new string name;
        [TextArea] public string Description;
        public SerializedDictionary<Ingredient, int> Ingredients;

        [Header("Data")]
        public RecipeData data;

        [Header("Unique Stats")]
        public RecipeStat Stats1;
        public RecipeStat Stats2;
        public RecipeStat Stats3;
        public RecipeStat Stats3Unique;
        public RecipeStat CurrentStats => (data.CookingDone < 5) ? null : (data.CookingDone < 10) ? Stats1 : (data.CookingDone < 15) ? Stats2 : (!data.UniqueValueUnlocked) ? Stats3 : Stats3Unique;
    }
}