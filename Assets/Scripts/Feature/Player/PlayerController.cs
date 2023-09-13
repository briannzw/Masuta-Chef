using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    using Character;
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("External")]
        public Character CharacterStats;

        [Header("References")]
        public Animator Animator;

        private PlayerAction playerControls;
        private CharacterController controller;

        [Header("Shooting")]
        public float rotationSpeed = 5.0f;
        private float rotationAngle = 0f;

        [Header("Movement")]
        public bool IsSprintDefault = false;

        public float moveSpeed = 5f;
        public float sprintMultiplier = 1.5f;
        public float gravity = -9.8f;
        public float turnSmoothTime = 0.1f;

        private Vector3 rawInputMovement = Vector3.zero;
        private Vector3 velocity;
        private Vector3 moveDirection;
        private Vector3 initialPosition;

        // Movement Parameters
        private float speed;
        private bool isSprint;
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
        }

        private void Start()
        {
            initialPosition = transform.position;
            if (CharacterStats != null) moveSpeed = CharacterStats.Speed.Value / 10;
            speed = IsSprintDefault ? moveSpeed * sprintMultiplier : moveSpeed;
            isSprint = IsSprintDefault;

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
            if (CharacterStats != null) moveSpeed = CharacterStats.Speed.Value / 10;
            speed = isSprint ? moveSpeed * sprintMultiplier : moveSpeed;
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

        #region Aim
        /// <summary>
        /// Turn player based on the mouse movement
        /// </summary>
        private void OnAim(InputAction.CallbackContext context)
        {
            rotationAngle += Input.GetAxis("Mouse X");
            transform.localRotation = Quaternion.Euler(0, rotationAngle, 0);
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
            playerControls.Gameplay.Aim.performed += OnAim;
        }
        private void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Move.performed -= OnMove;
            playerControls.Gameplay.Move.canceled -= OnMoveCanceled;
            playerControls.Gameplay.Sprint.performed -= OnSprint;
            playerControls.Gameplay.Sprint.canceled -= OnSprintCanceled;
            playerControls.Gameplay.Aim.performed -= OnAim;
        }
        #endregion

        #region Movement
        // Return Vector3 Move Input Direction
        private Vector3 GetMovementInputDirection()
        {
            if (rawInputMovement.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(rawInputMovement.x, rawInputMovement.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

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
            isSprint = !IsSprintDefault;
        }

        private void OnSprintCanceled(InputAction.CallbackContext context)
        {
            isSprint = IsSprintDefault;
        }
        #endregion

        // Handling
        private void CheckOutOfBound()
        {
            if(transform.position.y < -5f)
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
        /*private void OnApplicationFocus(bool focus)
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
        */
    }
}