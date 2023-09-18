using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cooking.Gameplay.Circular {
    public class CookingCircularHandleController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CookingCircularBarController barController;
        [SerializeField] private RectTransform handleTransform;

        [Header("Parameters")]
        [SerializeField] private float rotateSpeed;
        
        private PlayerAction cookingControls;

        private void Start()
        {
            cookingControls = InputManager.PlayerAction;
            // Could be source of input bug on other scenes
            InputManager.ToggleActionMap(cookingControls.Cooking);
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

        private void OnTouch(InputAction.CallbackContext context)
        {
            Vector2 touchDelta = context.ReadValue<Vector2>().normalized;
            Debug.Log(touchDelta);
            handleTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, handleTransform.rotation.z + Mathf.Atan2(touchDelta.y, touchDelta.x) * Mathf.Rad2Deg));
        }

        #region Callbacks
        private void RegisterInputCallbacks()
        {
            if (cookingControls == null) return;

            cookingControls.Cooking.Touch.performed += OnTouch;
        }

        private void UnregisterInputCallbacks()
        {
            if (cookingControls == null) return;

            cookingControls.Cooking.Touch.performed -= OnTouch;
        }
        #endregion
    }
}