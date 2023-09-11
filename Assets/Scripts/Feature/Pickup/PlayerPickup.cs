using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

namespace Group8.TrashDash.Player.Pickup
{
    using Module.Detector;

    public class PlayerPickup : MonoBehaviour
    {
        [Header("References")]
        public Animator Animator;
        private PlayerAction playerControls;

        [Header("Parameters")]
        public float PickUpRadius = 5;
        [Range(0, 360)] public float PickUpAngle = 125;

        public LayerMask TargetMask;
        public GameObject NearestPickup;

        private List<GameObject> detectedPickups;

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
            playerControls.Gameplay.Pickup.performed += OnPickup;
        }
        private void UnregisterInputCallback()
        {
            if (playerControls == null) return;
            playerControls.Gameplay.Pickup.performed -= OnPickup;
        }

        private void OnPickup(InputAction.CallbackContext context)
        {
            detectedPickups = ColliderDetector.Find<GameObject>(transform.position - transform.forward, PickUpRadius + transform.forward.magnitude, TargetMask, transform.forward, PickUpAngle);
            NearestPickup = null;

            if (detectedPickups.Count > 0)
            {
                NearestPickup = detectedPickups.OrderBy(
                    obj => (transform.position - obj.transform.position).sqrMagnitude).ToArray()[0];
                // nearestPickup will return nearest GameObject in TargetMask Layer
                // Can check if that nearest GameObject has script (to prevent other objects in same layer)
                /* ex:
                    IInteractable interactable = NearestInteractable.GetComponent<IInteractable>();
                    if (interactable == null)
                    {
                        Debug.Log(NearestInteractable.name + " does not contain IInteractable interface.");
                        return;
                    }

                    interactable.Interact(gameObject);
                 */
            }
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PickUpRadius);
        }
    }
}