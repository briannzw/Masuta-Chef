using System;

namespace Cooking.Recipe
{
    [Serializable]
    public class RecipeData
    {
        [ReadOnly] public int CurrentBlueprint;
        public int NeededBlueprint;
        public bool IsUnlocked => CurrentBlueprint >= NeededBlueprint;
    }
}