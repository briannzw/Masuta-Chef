using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Player.Controller
{
    public class PlayerWeaponController : MonoBehaviour
    {
        #region Properties
        [Header("Weapon Setting")]
        public Weapon.Weapon activeWeapon;
        public List<Weapon.Weapon> weaponSlot;
        public int maxSlot;

        [Header("Shoot Event")]
        public UnityEvent FireEvent;

        [Header("Settings")]
        [SerializeField] private LayerMask groundMask;

        private Camera maincamera;
        private PlayerAction playerControls;
        #endregion

        #region Lifecycle
        // Start is called before the first frame update
        void Start()
        {
            maincamera = Camera.main;
            playerControls = InputManager.PlayerAction;
            RegisterInputCallbacks();
        }

        // Update is called once per frame
        void Update()
        {
            Aim();
        }

        private void OnEnable()
        {
            RegisterInputCallbacks();
        }

        private void OnDisable()
        {
            UnregisterInputCallbacks();
        }
        #endregion

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

        #region Method
        private void OnInputActionStart(InputAction.CallbackContext context)
        {
            activeWeapon.StartAttack();
        }

        private void OnInputActionEnd(InputAction.CallbackContext context)
        {
            activeWeapon.StopAttack();
        }

        private void OnEquip (InputAction.CallbackContext context)
        {
            
        }

        private void OnUnequip(InputAction.CallbackContext context)
        {

        }

        private void Aim()
        {
            var (success, position) = GetMousePosition();
            if (success)
            {
                var direction = position - transform.position;
                direction.y = 0;
                transform.forward = direction;
            }
        }
        #endregion

        #region Helper Function
        private (bool success, Vector3 position) GetMousePosition()
        {
            var ray = maincamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo,Mathf.Infinity, groundMask))
            {
                return (success: true, position: hitInfo.point);
            }
            else 
            { 
                return (success: false, position: Vector3.zero); 
            }
        }
        #endregion
    }
}
