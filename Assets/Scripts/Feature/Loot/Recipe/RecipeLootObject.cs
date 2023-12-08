using UnityEngine;

namespace Loot.Object
{
    using Cooking.Recipe;
    using Behaviour;
    using Player.Controller;
    public class RecipeLootObject : LootFollowObject
    {
        [Header("References")]
        private Recipe recipe;
        private int count = 1;
        private PlayerAudioController audioController = GameManager.Instance.PlayerAudioController;

        protected override void ReachedTarget()
        {
            if(!audioController.LootCollectedAudio.isPlaying)
            {
                audioController.LootCollectedAudio.PlayOneShot(audioController.LootCollectedClip);
            }
            Destroy(gameObject);
        }

        public void Set(Recipe recipe, int count)
        {
            this.recipe = recipe;
            this.count = count;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target != null) return;

            if (other.CompareTag(Tag))
            {
                target = other.transform;
                // Change amount(?)
                GameManager.Instance.LevelManager.BlueprintCollected(recipe, count);
            }
        }
    }
}
