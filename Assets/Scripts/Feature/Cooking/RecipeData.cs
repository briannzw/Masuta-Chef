using MyBox;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Cooking.Recipe
{
    [Serializable]
    public class RecipeData
    {
        [Header("Blueprint")]
        public int CurrentBlueprint;

        [Header("Cooking")]
        public int CookingDone;
        public int PerfectCookingDone;
        public int ConsecutivePerfectCookingDone;
        public int UniqueStatIndex = -1;

        public int CookingPoint;

        [JsonIgnore] public bool IsAutoCookUnlocked => PerfectCookingDone > 10;
    }
}