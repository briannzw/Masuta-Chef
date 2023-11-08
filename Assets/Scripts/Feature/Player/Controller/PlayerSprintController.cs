using Player.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    public class PlayerSprintController : PlayerInputControl
    {
        [Header("References")]
        public Animator Animator;
        [SerializeField] private PlayerMovementController playerMovementController;

        [Header("Parameters")]
        public float sprintMultiplier = 1.1f;
        public float sprintDuration = 0.5f;
        public float sprintCooldown = 2.5f;

        private Coroutine sprintCoroutine;

        #region Callbacks
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Sprint.performed += OnSprint;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Sprint.performed -= OnSprint;
        }
        #endregion

        #region Callback Functions
        private void OnSprint(InputAction.CallbackContext context)
        {
            if (!playerMovementController.enabled) return;

            if (sprintCoroutine != null) return;

            sprintCoroutine = StartCoroutine(DoSprint());
        }
        #endregion

        private IEnumerator DoSprint()
        {
            // Sprint
            playerMovementController.speedMultiplier = sprintMultiplier;
            if (Animator) Animator.SetBool("IsRunning", true);

            yield return new WaitForSeconds(sprintDuration);

            // Reset
            playerMovementController.speedMultiplier = 1f;
            if (Animator) Animator.SetBool("IsRunning", false);

            yield return new WaitForSeconds(sprintCooldown);

            sprintCoroutine = null;
        }
    }
}