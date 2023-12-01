using Cooking;
using Cooking.Recipe;
using System;
using System.Collections.Generic;

namespace Result
{
    public class BattleResult
    {
        public Dictionary<Ingredient, int> ingredientsCollected = new();
        public Dictionary<Recipe, int> blueprintsCollected = new();

        public DateTime startTime;
        public DateTime clearTime;
        public TimeSpan clearDuration;

        public bool isBestTime;
    }
}