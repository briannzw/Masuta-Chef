namespace Cooking.Recipe.Data
{
    [System.Serializable]
    public class RecipeData
    {
        public Recipe Recipe;
        public int RequiredBlueprint = 0;
        [ReadOnly] public int Blueprint = 0;
    }
}