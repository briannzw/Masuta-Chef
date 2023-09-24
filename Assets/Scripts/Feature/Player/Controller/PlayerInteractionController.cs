using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    using Input;
    using Interaction;
    using Module.Detector;

    public class PlayerInteractionController : PlayerInputControl
    {
        [Header("References")]
        public Animator Animator;

        [Header("Parameters")]
        public float InteractRadius = 5;
        [Range(0, 360)] public float InteractAngle = 125;

        public LayerMask TargetMask;

        public IInteractable NearestInteractable;

        private void Update()
        {
            if (!playerControls.Gameplay.enabled) return;
            if (NearestInteractable == null) return;

            if (playerControls.Gameplay.Move.IsPressed())
            {
                StopAllCoroutines();
                CancelInteraction();
            }
        }

        #region Callbacks
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;
            playerControls.Gameplay.Interact.performed += OnInteract;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;
            playerControls.Gameplay.Interact.performed -= OnInteract;
        }
        #endregion

        #region Callback Functions
        private void OnInteract(InputAction.CallbackContext context)
        {
            if (!playerControls.Gameplay.Interact.enabled) return;

            NearestInteractable = ColliderDetector.FindNearest<IInteractable>(transform.position - transform.forward, InteractRadius + 1f, TargetMask, transform.forward, InteractAngle);
            if (NearestInteractable == null)
            {
                Debug.Log("No GameObject with IInteractable interface found.");
                return;
            }

            NearestInteractable.Interact(gameObject);

            StopAllCoroutines();
        }
        #endregion

        private void CancelInteraction()
        {
            if (NearestInteractable == null) return;

            NearestInteractable.ExitInteract();
            NearestInteractable = null;
        }
    }
}