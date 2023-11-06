using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Rollback
{
    using Input;
    using Player.Controller;

    public class PlayerRollbackController : PlayerInputControl
    {
        [Header("References")]
        [SerializeField] private PlayerMovementController controller;
        private CharacterController charaController;

        [Header("Parameters")]
        [SerializeField] private float rollbackDistance = 5f;
        [SerializeField] private float rollbackDuration = .5f;
        [SerializeField] private AnimationCurve rollbackCurve;

        [SerializeField] private float rollbackCooldown = 5f;

        private float timer;

        private Vector3 forward;

        private Coroutine rollbackCoroutine;

        private void Awake()
        {
            charaController = controller.GetComponent<CharacterController>();
        }

        private IEnumerator LeapToPosition()
        {
            forward = transform.forward;
            timer = 0f;

            while (true)
            {
                timer += Time.deltaTime/ rollbackDuration;
                if(timer < 1f)
                {
                    charaController.Move(rollbackCurve.Evaluate(timer) * rollbackDistance * Time.deltaTime * forward);
                }
                else
                    break;

                yield return null;
            }

            controller.enabled = true;

            yield return new WaitForSeconds(rollbackCooldown);

            rollbackCoroutine = null;
        }

        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Rollback.performed += OnRollback;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Rollback.performed -= OnRollback;
        }

        private void OnRollback(InputAction.CallbackContext context)
        {
            if(rollbackCoroutine != null) return;
            rollbackCoroutine = StartCoroutine(LeapToPosition());
        }
    }
}