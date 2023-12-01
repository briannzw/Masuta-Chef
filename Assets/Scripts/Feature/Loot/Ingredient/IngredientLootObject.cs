using UnityEngine;

namespace Loot.Object
{
    using Cooking;
    using Behaviour;
    using Level;

    public class IngredientLootObject : LootFollowObject
    {
        [Header("References")]
        private Ingredient ingredient;
        private int count = 1;

        protected override void ReachedTarget()
        {
            Destroy(gameObject);
        }

        public void Set(Ingredient ingredient, int count)
        {
            this.ingredient = ingredient;
            this.count = count;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target != null) return;

            if (other.CompareTag(Tag))
            {
                target = other.transform;
                // Change amount(?)
                GameManager.Instance.LevelManager.IngredientCollected(ingredient, count);
            }
        }
    }
}
