namespace Save.Data
{
    using AYellowpaper.SerializedCollections;
    using Cooking;
    using Cooking.Recipe;
    [System.Serializable]
    public class SaveData
    {
        public SerializedDictionary<string, RecipeData> RecipeData;
        public SerializedDictionary<string, IngredientData> IngredientData;

        // Handle Recipe Blueprints
        public bool Add(Recipe recipe, int value)
        {
            if (!RecipeData.ContainsKey(recipe.name))
                RecipeData.Add(recipe.name, recipe.data);

            if (!recipe.IsLocked) return false;

            RecipeData[recipe.name].CurrentBlueprint += value;
            return true;
        }

        public bool Add(Ingredient ingredient, int value)
        {
            if (!IngredientData.ContainsKey(ingredient.name))
                IngredientData.Add(ingredient.name, ingredient.data);

            if (ingredient.data.IsMaxed) return false;

            IngredientData[ingredient.name].Count += value;
            return true;
        }
    }
}