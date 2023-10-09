using UnityEngine;

namespace Loot.Object
{
    using Cooking;
    using Behaviour;

    public class IngredientLootObject : LootFollowObject
    {
        [Header("References")]
        //[SerializeField] private SaveManager saveManager;
        public Ingredient Ingredient;

        protected override void ReachedTarget()
        {
            // TODO: saveManager Handle Ingredient Save Data
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target != null) return;

            if (other.CompareTag(Tag))
            {
                target = other.transform;
                // TODO: Add Ingredient Count AND Save
                Ingredient.Add();
            }
        }
    }
}
