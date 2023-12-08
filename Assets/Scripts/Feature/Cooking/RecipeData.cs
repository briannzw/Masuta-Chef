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
        public string StatsIndex;

        public int CookingPoint;

        [Header("Misc")]
        public bool IsNotified = false;

        [JsonIgnore] public bool IsAutoCookUnlocked => PerfectCookingDone > 10;
        [JsonIgnore] public bool IsStat3Assigned => !string.IsNullOrEmpty(StatsIndex);
    }
}