
namespace Loot.Interactable
{
    using Player.Interaction;
    using UnityEngine;

    public class LootObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private float Lifetime = 20f;
        private void Start()
        {
            Destroy(gameObject, Lifetime);
        }

        public void Interact(GameObject other = null)
        {
            // TODO: other.WeaponSlot Exchange
            Debug.Log("Interacted with " + name + " (TODO: Weapon Exchange)");
        }
    }
}