using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HUD
{
    using Player.Controller;

    public class WeaponHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerWeaponController weaponController;

        [Header("UI References")]
        [SerializeField] private Image weaponIcon;
        [SerializeField] private Image weaponCooldownImage;
        [SerializeField] private TMP_Text weaponCooldownText;
        [SerializeField] private GameObject weaponUltimateHint;

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

            weaponCooldownImage.fillAmount = 0f;
            weaponCooldownText.text = "";
            weaponUltimateHint.SetActive(false);
        }

        private void Update()
        {
            if (weaponController.ActiveWeapon == null) return;

            if (weaponController.ActiveWeapon.isCooldownUltimate)
            {
                weaponUltimateHint.SetActive(false);
                weaponCooldownImage.fillAmount = 1 - weaponController.ActiveWeapon.UltimateCooldownRatio;
                weaponCooldownText.text = Mathf.RoundToInt(weaponController.ActiveWeapon.UltTimer).ToString();
            }
            else
            {
                weaponUltimateHint.SetActive(true);
                if(!string.IsNullOrEmpty(weaponCooldownText.text)) weaponCooldownText.text = "";
            }
        }

        private void SetUI()
        {
            weaponIcon.sprite = weaponController.ActiveWeapon.data.icon;
            weaponIcon.enabled = true;
        }
    }
}