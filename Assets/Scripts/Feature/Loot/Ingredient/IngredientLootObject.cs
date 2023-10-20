using UnityEngine;

namespace Loot.Object
{
    using Cooking;
    using Behaviour;

    public class IngredientLootObject : LootFollowObject
    {
        [Header("References")]
        public Ingredient Ingredient;

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
                if (!GameManager.Instance.SaveManager.SaveData.Add(Ingredient, 1)) Debug.Log("Ingredient " + Ingredient.name + " reached max amount!");
            }
        }
    }
}
