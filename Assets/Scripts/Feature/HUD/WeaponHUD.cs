using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    using Player.Controller;

    public class WeaponHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerWeaponController weaponController;

        [Header("UI References")]
        [SerializeField] private Image weaponIcon;

        private void Awake()
        {
            Reset();
            weaponController.OnWeaponChanged += SetUI;
            if(weaponController.ActiveWeapon != null) SetUI();
        }

        private void Reset()
        {
            weaponIcon.sprite = null;
            weaponIcon.enabled = false;
        }

        private void SetUI()
        {
            weaponIcon.sprite = weaponController.ActiveWeapon.data.icon;
            weaponIcon.enabled = true;
        }
    }
}