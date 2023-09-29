using System;
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
        public float rotationSpeed = 5.0f;
        private float rotationAngle = 0f;
        int floorMask;
    float camRayLength = 100f;
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

            // Aim
            // if (rawInputAimRotation != Vector2.zero)
            // {

            // }

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
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Vector3 camRay = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            // Debug.Log(camRay);
            RaycastHit floorHit;

            if(Physics.Raycast(camRay, out floorHit, Mathf.Infinity, floorMask))
            {
                Vector3 playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0;
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                transform.localRotation = newRotation;
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

        // // remove Cursor
        // private void OnApplicationFocus(bool focus)
        // {
        //     if (focus) 
        //     {
        //         Cursor.lockState = CursorLockMode.Locked;
        //     }
        //     else
        //     {
        //         Cursor.lockState = CursorLockMode.None;
        //     }
        // }
    }
}