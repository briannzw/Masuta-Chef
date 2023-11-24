using UnityEngine;

namespace Cooking.Recipe
{
    using AYellowpaper.SerializedCollections;
    using Character.StatEffect;

    [CreateAssetMenu(menuName = "Cooking/Recipe", fileName = "New Recipe")]
    public class Recipe : ScriptableObject
    {
        [Header("Information")]
        public Sprite Icon;
        public Sprite LockedIcon;
        public new string name;
        [TextArea] public string Description;
        public CookingType CookingType;
        public SerializedDictionary<Ingredient, int> Ingredients;

        [Header("Data")]
        public RecipeData data;
        public int NeededBlueprint;

        [Header("Stats")]
        public AddOnStat[] Stats;

        public bool IsLocked => data.CurrentBlueprint < NeededBlueprint;
        public bool HasEnoughIngredients()
        {
            bool value = true;
            foreach(var ingredient in Ingredients)
            {
                // Make sure to fetch data before this.
                if (ingredient.Key.data.Count < ingredient.Value) return false;
            }
            return value;
        }
    }
}