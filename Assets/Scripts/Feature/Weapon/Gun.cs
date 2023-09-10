using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Weapon.Gun
{
    public class Gun : Weapon
    {
        public UnityEvent ButtonDownEvent;
        public UnityEvent ButtonUpEvent;
        private PlayerAction playerControls;

        protected override void Start()
        {
            base.Start();
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

        #region Callbacks
        private void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Fire.Enable();
            playerControls.Gameplay.Fire.started += OnInputActionStart;
            playerControls.Gameplay.Fire.canceled += OnInputActionEnd;
        }

        private void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Fire.started -= OnInputActionStart;
            playerControls.Gameplay.Fire.canceled -= OnInputActionEnd;
            playerControls.Gameplay.Fire.Disable();
        }
        #endregion

        #region Button Event
        private void OnInputActionStart(InputAction.CallbackContext obj)
        {
            ButtonDownEvent.Invoke();
        }

        private void OnInputActionEnd(InputAction.CallbackContext obj)
        {
            ButtonUpEvent.Invoke();
        }
        #endregion
    }
}