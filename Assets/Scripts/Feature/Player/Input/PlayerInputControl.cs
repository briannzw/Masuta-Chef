using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
    public abstract class PlayerInputControl : MonoBehaviour
    {
        protected PlayerAction playerControls;

        protected virtual void Start()
        {
            playerControls = InputManager.PlayerAction;
            RegisterInputCallbacks();
        }

        protected virtual void OnEnable()
        {
            RegisterInputCallbacks();
        }

        protected virtual void OnDisable()
        {
            UnregisterInputCallbacks();
        }

        #region Callbacks
        protected abstract void RegisterInputCallbacks();
        protected abstract void UnregisterInputCallbacks();
        #endregion
    }
}