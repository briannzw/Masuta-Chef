using UnityEngine;

namespace Pickup
{
    public interface IPickable
    {
        public Transform Holder { get; set; }

        public bool StartPickup(GameObject picker);
        public void ExitPickup();
    }

    public interface IThrowable : IPickable
    {
        public Rigidbody rb { get; set; }
    }
}