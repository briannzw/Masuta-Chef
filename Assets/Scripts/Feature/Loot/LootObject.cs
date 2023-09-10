
namespace Loot.Interactable
{
    using Player.Interaction;
    using UnityEngine;

    public class LootObject : MonoBehaviour, IInteractable
    {
        public void Interact(GameObject other = null)
        {
            // TODO: other.WeaponSlot Exchange
            Debug.Log("Interacted with " + name + " (TODO: Weapon Exchange)");
        }
    }
}