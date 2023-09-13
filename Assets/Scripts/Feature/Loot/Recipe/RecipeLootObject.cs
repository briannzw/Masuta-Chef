using UnityEngine;

namespace Loot.Object
{
    using Cooking.Recipe;
    public class RecipeLootObject : LootObject
    {
        public int Count;
        public Recipe Recipe;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player got " + Count + " " + Recipe.name + " blueprint. (TODO : Save)");
            }
        }
    }
}