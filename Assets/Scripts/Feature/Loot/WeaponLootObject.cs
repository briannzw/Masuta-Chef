using UnityEngine;

namespace Loot.Interactable
{
    using Interaction;
    public class WeaponLootObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private float lifetime = 20f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        public void Interact(GameObject other = null)
        {
            // other.GetComponent<WeaponController>().Exchange();
            Debug.Log("Interacted with " + name + " (TODO: Weapon Exchange)");
        }
    }
}
