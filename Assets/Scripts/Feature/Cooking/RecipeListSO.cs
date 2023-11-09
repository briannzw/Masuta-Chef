using System.Collections.Generic;
using UnityEngine;

namespace Cooking.Recipe
{
    [CreateAssetMenu(menuName = "Recipe List", fileName = "Recipe List", order = 0)]
    public class RecipeListSO : ScriptableObject
    {
        public List<Recipe> Recipes = new();
    }
}