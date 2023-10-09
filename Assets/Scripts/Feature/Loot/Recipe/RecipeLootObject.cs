using UnityEngine;

namespace Loot.Object
{
    using Cooking.Recipe;
    using Behaviour;

    public class RecipeLootObject : LootFollowObject
    {
        [Header("References")]
        //[SerializeField] private SaveManager saveManager;
        public Recipe Recipe;

       protected override void ReachedTarget()
        {
            // TODO: saveManager Handle Recipe Save Data
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(target != null) return;

            if (other.CompareTag(Tag))
            {
                target = other.transform;
                // TODO: Add Blueprint AND Save
                Recipe.AddBlueprint();
            }
        }
    }
}
