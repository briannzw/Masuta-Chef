using UnityEngine;

namespace Loot
{
    using Cooking.Recipe;

    [System.Serializable]
    public class RecipeLoot : Loot
    {
        public Recipe Recipe;
        public int MinCount;
        public int MaxCount;
    }
}