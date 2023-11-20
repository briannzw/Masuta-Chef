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
        public new string name;
        [TextArea] public string Description;
        public CookingType CookingType;
        public SerializedDictionary<Ingredient, int> Ingredients;

        [Header("Data")]
        public RecipeData data;

        [Header("Stats")]
        public AddOnStat[] Stats;
    }
}