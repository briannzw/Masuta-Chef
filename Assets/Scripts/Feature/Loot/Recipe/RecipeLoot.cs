using Cooking.Recipe;
using System.Collections.Generic;

namespace Loot
{
    public class RecipeLoot : Loot
    {
        public List<Recipe> Recipes;

        public RecipeLoot()
        {
            Type = LootType.Recipe;
        }
    }
}