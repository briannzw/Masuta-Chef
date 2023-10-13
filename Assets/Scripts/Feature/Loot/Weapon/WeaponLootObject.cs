using UnityEngine;

namespace Loot.Interactable
{
    public class WeaponLootObject : LootInteractObject
    {
        public override void Interact(GameObject other = null)
        {
            // other.GetComponent<WeaponController>().Exchange();
            Debug.Log("Interacted with " + name + " (TODO: Weapon Exchange)");
        }
    }
}
