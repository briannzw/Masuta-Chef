using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    using Input;
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : PlayerInputControl
    {
        [Header("References")]
        public Animator Animator;
        private CharacterController controller;

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
        }

        protected override void Start()
        {
            initialPosition = transform.position;
            speed = isSprintDefault ? sprintSpeed : moveSpeed;
            base.Start();
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

        #region Callbacks
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Move.performed += OnMove;
            playerControls.Gameplay.Move.canceled += OnMoveCanceled;
            playerControls.Gameplay.Sprint.performed += OnSprint;
            playerControls.Gameplay.Sprint.canceled += OnSprintCanceled;
        }
        protected override void UnregisterInputCallbacks()
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
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                return moveDirection;
            }

            return Vector3.zero;
        }
        #endregion

        #region Movement Handling
        private void CheckOutOfBound()
        {
            if(transform.position.y < -5f)
            {
                velocity = Vector3.zero;
                transform.position = initialPosition;
            }
        }
        #endregion

        #region Callback Functions
        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputMovement = context.ReadValue<Vector2>();
            rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
            if(Animator) Animator.SetBool("IsWalking", true);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            rawInputMovement = Vector3.zero;
            if (Animator) Animator.SetBool("IsWalking", false);
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            speed = isSprintDefault ? moveSpeed : sprintSpeed;
            if (Animator) Animator.SetBool("IsRunning", true);
        }

        private void OnSprintCanceled(InputAction.CallbackContext context)
        {
            speed = isSprintDefault ? sprintSpeed : moveSpeed;
            if (Animator) Animator.SetBool("IsRunning", false);
        }
        #endregion

    }
}