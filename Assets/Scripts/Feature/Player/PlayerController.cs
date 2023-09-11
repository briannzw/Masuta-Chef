using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        public Animator Animator;

        private PlayerAction playerControls;
        private CharacterController controller;

        [Header("Shooting")]
        private Camera mainCamera;
        private int floorMask;
        [SerializeField] private GameObject shootTargetObject;

        [Header("Movement")]
        public bool isSprintDefault = false;

        public float moveSpeed = 5f;
        public float sprintSpeed = 8f;
        public float gravity = -9.8f;
        public float turnSmoothTime = 0.1f;

        private Vector3 rawInputMovement = Vector3.zero;
        private Vector3 velocity;
        private Vector3 moveDirection;
        private Vector3 initialPosition;

        // Pickup Crate
        [Header("PickUp")]
        private bool isHoldingObject = false;
        private GameObject heldObject;
        public float pickupDistance = 3f;

        // Drop Crate
        private bool hasDroppedObject = false;

        // Capsule
        private float capsuleHeight = 2f;
        private float capsuleRadius = 2f;
        [SerializeField]
        private Transform holdPosition;

        // Movement Parameters
        private float speed;
        private float turnSmoothVelocity = 0;

        public float Speed => speed;
        public float SpeedMultiplier => speedMultiplier;

        [Header("Environment")]
        public float PushForce;

        [Header("Modifiers")]
        [Range(.1f, 10), SerializeField] private float speedMultiplier = 1;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            floorMask = LayerMask.GetMask("Floor");
        }

        private void Start()
        {
            initialPosition = transform.position;
            speed = isSprintDefault ? sprintSpeed : moveSpeed;

            mainCamera = Camera.main;

            playerControls = InputManager.PlayerAction;
            RegisterInputCallbacks();
        }

        private void OnEnable()
        {
            RegisterInputCallbacks();
        }

        private void OnDisable()
        {
            UnregisterInputCallbacks();
        }

        private void Update()
        {
            moveDirection = GetMovementInputDirection();
            velocity = new Vector3(moveDirection.x * speed * speedMultiplier, velocity.y, moveDirection.z * speed * speedMultiplier);

            // Gravity
            if (controller.isGrounded)
            {
                if (velocity.y < 0f)
                    velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            CheckOutOfBound();

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isHoldingObject)
                {
                    Debug.Log("Trying to pick up an object...");
                    PickUpObject();
                }
                else
                {
                    Debug.Log("Trying to drop the object...");
                    DropObject();
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                if (isHoldingObject)
                {
                    Debug.Log("Throwing the held object...");
                    ThrowHeldObject();
                }
            }

            // Memastikan objek yang dipegang selalu berada di depan pemain
            if (isHoldingObject && heldObject != null)
            {
                Vector3 offset = transform.forward * 1.3f;

                heldObject.transform.position = transform.position + offset;
            }

            // Periksa apakah objek yang dipegang masih ada
            if (isHoldingObject && heldObject == null && !hasDroppedObject)
            {
                Debug.Log("The held object has been destroyed. Automatically dropping it.");
                DropObject();
                isHoldingObject = false;
                heldObject = null;
            }

        }



        // PickUp Object
        private void PickUpObject()
        {
            if (!isHoldingObject)
            {
                Collider[] colliders = Physics.OverlapCapsule(transform.position - Vector3.up * capsuleHeight / 2f, transform.position + Vector3.up * capsuleHeight / 2f, capsuleRadius);

                foreach (Collider col in colliders)
                {
                    if (col.CompareTag("crate"))
                    {
                        // Perhitungan jarak antara pemain dan objek "crate"
                        float distance = Vector3.Distance(transform.position, col.transform.position);

                        if (distance <= pickupDistance)
                        {
                            CrateController crate = col.GetComponent<CrateController>();
                            if (crate != null && !crate.IsHeld)
                            {
                                crate.PickupObject(transform);
                                isHoldingObject = true;
                                heldObject = crate.gameObject;
                                break; // Hentikan iterasi setelah menemukan objek untuk diambil
                            }
                        }
                    }
                }
            }
        }

        private void DropObject()
        {
            if (isHoldingObject && heldObject != null)
            {
                CrateController crate = heldObject.GetComponent<CrateController>();
                if (crate != null)
                {
                    crate.DropObject();
                    isHoldingObject = false;
                    heldObject = null;
                }
            }
        }

        private void ThrowHeldObject()
        {
            if (isHoldingObject && heldObject != null)
            {
                CrateController crate = heldObject.GetComponent<CrateController>();
                if (crate != null)
                {
                    // Kalkulasi arah dorongan berdasarkan arah pandangan pemain
                    Vector3 throwDirection = transform.forward;

                    // Panggil metode ThrowObject pada CrateController dengan arah dorongan yang tepat
                    crate.ThrowObject(throwDirection);

                    isHoldingObject = false;
                    heldObject = null;
                }
            }
        }


        #region Rotation
        private void FixedUpdate()
        {
            Aim();
        }

        /// <summary>
        /// Turn player based on the mouse movement
        /// </summary>
        private void Aim()
        {
            var (isSuccess, position) = GetMousePosition();
            if (isSuccess)
            {
                var direction = position - transform.position;
                direction.y = 0;
                shootTargetObject.transform.position = transform.position + direction.normalized;
                transform.forward = direction;
            }
        }

        private (bool isSuccess, Vector3 position) GetMousePosition()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, floorMask))
            {
                return (isSuccess: true, position: hitInfo.point);
            }
            else
            {
                return (isSuccess: false, position: Vector3.zero);
            }
        }
        #endregion

        #region Callbacks
        private void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Move.performed += OnMove;
            playerControls.Gameplay.Move.canceled += OnMoveCanceled;
            playerControls.Gameplay.Sprint.performed += OnSprint;
            playerControls.Gameplay.Sprint.canceled += OnSprintCanceled;
        }
        private void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Move.performed -= OnMove;
            playerControls.Gameplay.Move.canceled -= OnMoveCanceled;
            playerControls.Gameplay.Sprint.performed -= OnSprint;
            playerControls.Gameplay.Sprint.canceled -= OnSprintCanceled;
        }
        #endregion

        #region Movement
        // Return Vector3 Move Input Direction
        private Vector3 GetMovementInputDirection()
        {
            if (rawInputMovement.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(rawInputMovement.x, rawInputMovement.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                // transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                return moveDirection;
            }

            return Vector3.zero;
        }
        #endregion

        #region Callback Functions
        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputMovement = context.ReadValue<Vector2>();
            rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            rawInputMovement = Vector3.zero;
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            speed = isSprintDefault ? moveSpeed : sprintSpeed;
        }

        private void OnSprintCanceled(InputAction.CallbackContext context)
        {
            speed = isSprintDefault ? sprintSpeed : moveSpeed;
        }
        #endregion

        // Handling
        private void CheckOutOfBound()
        {
            if (transform.position.y < -5f)
            {
                velocity = Vector3.zero;
                transform.position = initialPosition;
            }
        }

        #region Environment
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb && !rb.isKinematic)
            {
                rb.velocity = hit.moveDirection * PushForce * (speed / moveSpeed);
            }
        }
        #endregion

        // remove Cursor
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}