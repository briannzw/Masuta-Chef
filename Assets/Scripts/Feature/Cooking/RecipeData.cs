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
        [ReadOnly] public int CurrentBlueprint;
        public int NeededBlueprint;

        [Header("Cooking")]
         public int CookingDone;
        [ReadOnly] public int PerfectCookingDone;
        [ReadOnly] public int ConsecutivePerfectCookingDone;
        public bool UniqueValueUnlocked = false;

        [JsonIgnore] public bool IsLocked => CurrentBlueprint < NeededBlueprint;
        [JsonIgnore] public bool IsAutoCookUnlocked => PerfectCookingDone > 10;
    }
}