using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    using Input;
    using System;
    using Weapon;
    public class PlayerWeaponController : PlayerInputControl
    {
        #region Properties
        [Header("References")]
        public Weapon ActiveWeapon;
        public Transform WeaponTransform;
        [SerializeField] private Animator animator;

        [Header("Weapon Setting")]
        public List<Weapon> WeaponSlot;
        public int MaxSlot;

        [Header("Shoot Event")]
        public UnityEvent FireEvent;

        [Header("Settings")]
        [SerializeField] private LayerMask groundMask;
        #endregion

        #region C# Events
        public Action OnWeaponChanged;
        #endregion

        #region Lifecycle
        protected override void Start()
        {
            base.Start();
            if(ActiveWeapon != null) ActiveWeapon.OnEquip(GetComponent<Character.Character>());
        }

        private void Update()
        {
            Aim();
        }

        #endregion

        #region Callbacks
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Fire.Enable();
            playerControls.Gameplay.Fire.started += OnInputActionStart;
            playerControls.Gameplay.Fire.canceled += OnInputActionEnd;
            playerControls.Gameplay.Ultimate.Enable();
            playerControls.Gameplay.Ultimate.started += OnUltimateInputActionStart;
            playerControls.Gameplay.Ultimate.canceled += OnUltimateInputActionEnd;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Fire.started -= OnInputActionStart;
            playerControls.Gameplay.Fire.canceled -= OnInputActionEnd;
            playerControls.Gameplay.Fire.Disable();
            playerControls.Gameplay.Ultimate.started -= OnUltimateInputActionStart;
            playerControls.Gameplay.Ultimate.canceled -= OnUltimateInputActionEnd;
            playerControls.Gameplay.Ultimate.Disable();
        }
        #endregion

        #region Method
        private void OnInputActionStart(InputAction.CallbackContext context)
        {
            if(ActiveWeapon == null) return;
            ActiveWeapon.StartAttack();
        }

        private void OnInputActionEnd(InputAction.CallbackContext context)
        {
            if(ActiveWeapon == null) return;
            ActiveWeapon.StopAttack();
        }

        private void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            //Check num pressed
        }

        private void OnUltimateInputActionStart(InputAction.CallbackContext context)
        {
            if (ActiveWeapon == null) return;
            ActiveWeapon.StartUltimateAttack();
        }

        private void OnUltimateInputActionEnd(InputAction.CallbackContext context)
        {
            if (ActiveWeapon == null) return;
            // ActiveWeapon.StopUltimateAttack();
        }
        private void Aim()
        {
            if (Time.timeScale == 0f) return;

            var (success, position) = GetMousePosition();
            if (success)
            {
                var direction = position - transform.position;
                direction.y = 0;
                transform.forward = direction;
            }
        }

        public void Equip(Weapon newWeapon)
        {
            Transform weapOrigin;

            if (ActiveWeapon != null)
            {
                weapOrigin = ActiveWeapon.transform;
                // Old Weapon
                ActiveWeapon.OnUnequip();
                if(ActiveWeapon.transform.parent != WeaponTransform) weapOrigin = ActiveWeapon.transform.parent;
                weapOrigin.transform.SetParent(null);
            }

            // New Weapon
            ActiveWeapon = newWeapon;

            OnWeaponChanged?.Invoke();

            // For Weapon as child (Models)
            weapOrigin = newWeapon.transform;
            if(newWeapon.transform.parent != null)
            {
                weapOrigin = newWeapon.transform.parent.transform;
            }
            weapOrigin.SetParent(WeaponTransform);
            weapOrigin.localPosition = Vector3.zero;
            weapOrigin.localRotation = Quaternion.identity;
            animator.SetBool("RunGun", ActiveWeapon.IsGun);
        }
        #endregion

        #region Helper Function
        private (bool success, Vector3 position) GetMousePosition()
        {
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo,Mathf.Infinity, groundMask))
            {
                return (success: true, position: hitInfo.point);
            }
            else 
            { 
                return (success: false, position: Vector3.zero); 
            }
        }

        public void SetWeapon(bool value)
        {
            WeaponTransform.gameObject.SetActive(value);
        }
        #endregion
    }
}
