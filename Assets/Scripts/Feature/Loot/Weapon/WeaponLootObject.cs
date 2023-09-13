using UnityEngine;

namespace Loot.Object
{
    using Player.Interaction;
    public class WeaponLootObject : LootObject, IInteractable
    {
        public void Interact(GameObject other = null)
        {
            // TODO: other.WeaponSlot Exchange
            Debug.Log("Interacted with " + name + " (TODO: Weapon Exchange)");
        }
    }
}
