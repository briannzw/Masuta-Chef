namespace Save.Data
{
    using AYellowpaper.SerializedCollections;
    using Cooking;
    using Cooking.Recipe;
    using System;
    using System.Collections.Generic;

    [System.Serializable]
    public class SaveData
    {
        public SerializedDictionary<string, RecipeData> RecipeData;
        public SerializedDictionary<string, IngredientData> IngredientData;
        public Dictionary<string, TimeSpan> LevelBestTime = new();

        // Handle Recipe Blueprints
        public void New(Recipe recipe)
        {
            if (!RecipeData.ContainsKey(recipe.name))
                RecipeData.Add(recipe.name, recipe.data);

            RecipeData[recipe.name] = new();
        }

        public void New(Ingredient ingredient)
        {
            if (!IngredientData.ContainsKey(ingredient.name))
                IngredientData.Add(ingredient.name, ingredient.data);

            IngredientData[ingredient.name] = new();
        }

        public int Add(Recipe recipe, int count)
        {
            if (!RecipeData.ContainsKey(recipe.name)) New(recipe);

            RecipeData[recipe.name].CurrentBlueprint += count;
            if (RecipeData[recipe.name].CurrentBlueprint > recipe.NeededBlueprint)
            {
                int prevBlueprint = RecipeData[recipe.name].CurrentBlueprint;
                RecipeData[recipe.name].CurrentBlueprint = recipe.NeededBlueprint;
                return prevBlueprint - recipe.NeededBlueprint;
            }

            return 0;
        }

        public int Add(Ingredient ingredient, int count)
        {
            if (!IngredientData.ContainsKey(ingredient.name)) New(ingredient);

            IngredientData[ingredient.name].Count += count;
            if (IngredientData[ingredient.name].IsMaxed)
            {
                IngredientData[ingredient.name].Count = ingredient.data.MaxCount;
                return IngredientData[ingredient.name].Count - ingredient.data.MaxCount;
            }

            return 0;
        }

        public bool Add(string levelName, TimeSpan ts)
        {
            if (!LevelBestTime.ContainsKey(levelName))
            {
                LevelBestTime.Add(levelName, ts);
                return true;
            }

            if (LevelBestTime[levelName].CompareTo(ts) <= 0) return false;

            LevelBestTime[levelName] = ts;
            return true;
        }
    }
}