using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private int count;

        public void Add(int amount = 1)
        {
            count += amount;
        }
    }
}