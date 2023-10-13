using UnityEngine;

namespace Loot.Interactable
{
    using Interaction;
    public abstract class LootInteractObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private float lifetime = 20f;

        protected virtual void Start()
        {
            Destroy(gameObject, lifetime);
        }

        public virtual void Interact(GameObject other = null)
        {
            Debug.Log("Interacted with " + name);
        }
    }
}
