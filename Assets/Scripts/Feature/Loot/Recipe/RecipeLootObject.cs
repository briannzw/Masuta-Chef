using UnityEngine;

namespace Loot.Object
{
    using Cooking.Recipe;
    using Behaviour;

    public class RecipeLootObject : LootFollowObject
    {
        [Header("References")]
        public Recipe Recipe;

        protected override void ReachedTarget()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target != null) return;

            if (other.CompareTag(Tag))
            {
                target = other.transform;
                // Change amount(?)
                if (!GameManager.Instance.SaveManager.SaveData.Add(Recipe, 1)) Debug.Log("Recipe " + Recipe.name +" is already unlocked!");
            }
        }
    }
}
