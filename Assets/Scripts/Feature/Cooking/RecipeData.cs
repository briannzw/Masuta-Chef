using MyBox;
using Newtonsoft.Json;
using System;

namespace Cooking.Recipe
{
    [Serializable]
    public class RecipeData
    {
        [ReadOnly] public int CurrentBlueprint;
        public int NeededBlueprint;
        [JsonIgnore] public bool IsLocked => CurrentBlueprint < NeededBlueprint;
    }
}