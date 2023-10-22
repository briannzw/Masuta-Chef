using Newtonsoft.Json;
using UnityEngine;

namespace Cooking
{

    [CreateAssetMenu(menuName = "Cooking/Ingredient", fileName = "New Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public Sprite Icon;
        public new string name;
        [TextArea] public string Description;


        [Header("Data")]
        public IngredientData data;
    }
}