using UnityEngine;

namespace Pickup
{
    public interface IPicker
    {
        public void OnStealed(IPickable pickable, GameObject stealer = null);
    }
}