using UnityEngine;

namespace Cooking.Recipe
{
    using AYellowpaper.SerializedCollections;
    using Loot;

    [CreateAssetMenu(menuName = "Cooking/Recipe", fileName = "New Recipe")]
    public class Recipe : ScriptableObject
    {
        [Header("Information")]
        public Sprite Icon;
        public new string name;
        [TextArea] public string Description;
        public SerializedDictionary<Ingredient, int> Ingredients;

        [Header("Data")]
        [SerializeField] private RecipeData data;

        public void AddBlueprint(int amount = 1)
        {
            data.CurrentBlueprint += amount;
        }
    }
}