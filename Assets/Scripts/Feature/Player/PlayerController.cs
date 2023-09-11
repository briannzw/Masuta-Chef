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