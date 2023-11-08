using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    using Input;
    using Pickup;
    using Module.Detector;
    using System;

    public class PlayerPickupController : PlayerInputControl
    {
        [Header("References")]
        public Animator Animator;
        [SerializeField] private Transform pickupPos;

        [Header("Pickup Settings")]
        public float PickupRadius = 3f;
        [Range(0, 360)] public float PickupAngle = 125;

        public LayerMask TargetMask;

        [Header("Throw Settings")]
        public float ThrowForce = 10f;
        public float ThrowLowAngle = 0f;
        public float ThrowHighAngle = 300f;

        private IPickable nearestPickable;

        protected override void Start()
        {
            base.Start();
        }

        #region Callback
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.PickUp.performed += OnPickUp;
            //playerControls.Gameplay.PickUp.canceled += OnPickUpCancel;
            playerControls.Gameplay.Throw.performed += OnThrow;
            playerControls.Gameplay.Throw_High.performed += OnThrow;
        }
        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.PickUp.performed -= OnPickUp;
            //playerControls.Gameplay.PickUp.canceled -= OnPickUpCancel;
            playerControls.Gameplay.Throw.performed -= OnThrow;
            playerControls.Gameplay.Throw_High.performed -= OnThrow;
        }
        #endregion

        #region Callback Function
        private void OnPickUp(InputAction.CallbackContext context)
        {
            if (!playerControls.Gameplay.PickUp.enabled) return;

            if (nearestPickable != null)
            {
                OnPickUpCancel(context);
                return;
            }

            GameObject go = ColliderDetector.FindNearest<GameObject>(transform.position - transform.forward, PickupRadius + 1f, TargetMask, transform.forward, PickupAngle);
            
            if (go == null) return;
            if (go.GetComponent<IPickable>() == null) return;
            nearestPickable = go.GetComponent<IPickable>();

            if (nearestPickable.StartPickup(gameObject))
            {
                go.transform.parent = pickupPos;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
            }
            else nearestPickable = null;
        }

        private void OnPickUpCancel(InputAction.CallbackContext context)
        {
            if (nearestPickable == null) return;

            pickupPos.DetachChildren();
            nearestPickable.ExitPickup();
            nearestPickable = null;
        }

        private void OnThrow(InputAction.CallbackContext context)
        {
            if (!playerControls.Gameplay.Throw.enabled) return;

            if (nearestPickable == null) return;
            if (nearestPickable is not IThrowable) return;

            // Hitung vektor gaya yang diperlukan untuk melempar ke atas dengan sudut
            Vector3 throwDirection = Quaternion.Euler((context.action == playerControls.Gameplay.Throw) ? ThrowLowAngle : ThrowHighAngle, 0f, 0f) * Vector3.forward;

            pickupPos.DetachChildren();
            nearestPickable.ExitPickup();
            (nearestPickable as IThrowable).rb.AddRelativeForce(throwDirection * ThrowForce, ForceMode.Impulse);
            nearestPickable = null;
        }
        #endregion

        private void OnDrawGizmos()
        {
            // Gambar gizmo untuk menunjukkan jangkauan pickup
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PickupRadius);
        }
    }
}
