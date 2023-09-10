using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking.Recipe
{
    using AYellowpaper.SerializedCollections;
    [CreateAssetMenu(menuName = "Cooking/Recipe", fileName = "New Recipe")]
    public class Recipe : ScriptableObject
    {
        public Sprite Icon;
        public new string name;
        [TextArea] public string Description;
        public SerializedDictionary<Ingredient, int> Ingredients;
    }
}