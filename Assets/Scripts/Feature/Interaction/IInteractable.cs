using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        public abstract void Interact(GameObject other = default);
        public virtual void ExitInteract() { }
    }
}