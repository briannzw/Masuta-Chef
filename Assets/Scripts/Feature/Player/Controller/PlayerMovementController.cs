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
        public float gravity = -9.8f;
        public float turnSmoothTime = 0.1f;

        private Vector3 rawInputMovement = Vector3.zero;
        private Vector3 velocity;
        private Vector3 moveDirection;
        private Vector3 initialPosition;

        // Movement Parameters
        private float speed;
        private float turnSmoothVelocity = 0;

        [Header("Environment")]
        public float PushForce;

        [Header("Modifiers")]
        public float speedMultiplier = 1;

        private Character.Character character;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        protected override void Start()
        {
            initialPosition = transform.position;
            character = GetComponent<Character.Character>();
            if (character != null)
            {
                speed = character.Stats.StatList[StatsEnum.Speed].Value / 10;
                character.OnSpeedChanged += () => speed = character.Stats.StatList[StatsEnum.Speed].Value / 10;
            }
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
        }
        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Move.performed -= OnMove;
            playerControls.Gameplay.Move.canceled -= OnMoveCanceled;
        }
        #endregion

        #region Movement
        // Return Vector3 Move Input Direction
        private Vector3 GetMovementInputDirection()
        {
            if (rawInputMovement.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(rawInputMovement.x, rawInputMovement.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //transform.rotation = Quaternion.Euler(0f, angle, 0f);

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
        #endregion

    }
}