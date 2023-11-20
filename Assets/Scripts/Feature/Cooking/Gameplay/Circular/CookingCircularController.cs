using Player.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cooking.Gameplay.Circular
{
    public class CookingCircularController : PlayerInputControl
    {
        [Header("References")]
        public RectTransform soupTransform;

        [Header("Sensitivity")]
        public float sensitivity = .5f;

        [Header("Parameters")]
        public float TurnSmoothTime;
        public float MaxVelocity = 100f;
        public float VelocityDrop = 20f;
        public float CurrentDirection;
        public float CurrentVelocity => currentVelocity;

        private float targetAngle;
        private float currentVelocity;
        private Vector3 lastRotation;
        private float turnSmoothVelocity;

        private bool isHeld = false;

        protected override void Start()
        {
            base.Start();
            InputManager.ToggleActionMap(playerControls.Cooking);
        }

        private void Update()
        {
            if (currentVelocity > 0f)
            {
                currentVelocity -= VelocityDrop * Mathf.Sign(currentVelocity) * Time.deltaTime;
                currentVelocity = Mathf.Clamp(currentVelocity, -MaxVelocity, MaxVelocity);
            }
            
            soupTransform.Rotate(new Vector3(0f, 0f, currentVelocity * Time.deltaTime));
            
            if (!isHeld) return;

            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = -Camera.main.transform.position.z;
            Vector2 direction = new Vector2(mousePosition.x - soupTransform.position.x, mousePosition.y - soupTransform.position.y).normalized;

            targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(lastRotation.z, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);

            // Update Last Touch
            // Clamp (like steer wheel)
            lastRotation.z = targetAngle;

            // Can't stir if the current stir direction isn't the desired one
            //if (Mathf.Sign(Mathf.DeltaAngle(angle, targetAngle)) != -Mathf.Sign(CurrentDirection)) return;
            // Accumulate Velocity
            currentVelocity += Mathf.DeltaAngle(angle, targetAngle) * sensitivity;
            currentVelocity = Mathf.Clamp(currentVelocity, -MaxVelocity, MaxVelocity);
        }

        private void OnTouch(InputAction.CallbackContext context)
        {
            if (context.performed) isHeld = true;
            else if (context.canceled)
            {
                isHeld = false;
                lastRotation = Vector3.zero;
            }
        }

        private void OnTouchStarted(InputAction.CallbackContext context)
        {
            // Rotation for Velocity
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = -Camera.main.transform.position.z;
            Vector2 direction = new Vector2(mousePosition.x - soupTransform.position.x, mousePosition.y - soupTransform.position.y).normalized;
            lastRotation.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Cooking.Touch.started += OnTouchStarted;
            playerControls.Cooking.Touch.performed += OnTouch;
            playerControls.Cooking.Touch.canceled += OnTouch;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Cooking.Touch.performed -= OnTouch;
            playerControls.Cooking.Touch.canceled -= OnTouch;
        }
    }
}