using UnityEngine;

namespace Loot.Interactable
{
    using Character;
    using Character.StatEffect;
    using System.Collections.Generic;

    public class MedkitLootObject : LootInteractObject, IApplyEffect
    {
        [field: SerializeField] public List<Effect> Effects { get; set; }

        public void ApplyEffect(Character character)
        {
            foreach (Effect effect in Effects)
            {
                effect.Initialize();
                character.AddEffect(effect);
            }
        }

        public override void Interact(GameObject other = null)
        {
            // Add player health
            if (other.CompareTag("Player"))
            {
                ApplyEffect(other.GetComponent<Character>());
                Destroy(gameObject);
            }
        }
    }
}
