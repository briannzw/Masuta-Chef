using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Interaction
{
    using Module.Detector;

    public class PlayerInteraction : MonoBehaviour
    {
        [Header("References")]
        public Animator Animator;
        private PlayerAction playerControls;

        [Header("Parameters")]
        public float InteractRadius = 5;
        [Range(0, 360)] public float InteractAngle = 125;

        public LayerMask TargetMask;

        /*public bool faceInteractable;
        public float RotateSpeed = 70;
        */

        public GameObject NearestInteractable;

        private List<GameObject> detectedInteractables;

        private void Start()
        {
            playerControls = InputManager.PlayerAction;
            RegisterInputCallback();
        }

        private void OnEnable()
        {
            RegisterInputCallback();
        }

        private void OnDisable()
        {
            UnregisterInputCallback();
        }

        #region Callbacks
        private void RegisterInputCallback()
        {
            if (playerControls == null) return;
            playerControls.Gameplay.Interact.performed += OnInteract;
        }

        private void UnregisterInputCallback()
        {
            if (playerControls == null) return;
            playerControls.Gameplay.Interact.performed -= OnInteract;
        }

        private void Update()
        {
            if (NearestInteractable == null) return;
            if (!playerControls.Gameplay.enabled) return;

            if (playerControls.Gameplay.Move.IsPressed())
            {
                StopAllCoroutines();
                CancelInteraction();
            }
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (!playerControls.Gameplay.Interact.enabled) return;

            detectedInteractables = ColliderDetector.Find<GameObject>(transform.position - transform.forward, InteractRadius + transform.forward.magnitude, TargetMask, transform.forward, InteractAngle);
            NearestInteractable = null;

            if (detectedInteractables.Count > 0)
            {
                NearestInteractable = detectedInteractables.OrderBy(
                    obj => (transform.position - obj.transform.position).sqrMagnitude).ToArray()[0];

                IInteractable interactable = NearestInteractable.GetComponent<IInteractable>();
                if (interactable == null)
                {
                    Debug.Log(NearestInteractable.name + " does not contain IInteractable interface.");
                    return;
                }

                interactable.Interact(gameObject);

                StopAllCoroutines();
            }
        }
        #endregion

        private void CancelInteraction()
        {
            if (NearestInteractable)
            {
                IInteractable interactable = NearestInteractable.GetComponent<IInteractable>();
                if (interactable == null)
                {
                    Debug.Log(NearestInteractable.name + " does not contain IInteractable interface.");
                    return;
                }

                interactable.ExitInteract();
            }

            NearestInteractable = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, InteractRadius);
        }
    }
}