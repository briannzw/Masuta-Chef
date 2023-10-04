using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Input; // Pastikan Anda mengimpor namespace yang benar

namespace Player.Controller
{
    public class IconManager : PlayerInputControl
    {
        public GameObject iconItem;
        public GameObject iconMedkit;
        public GameObject iconWeapon;
        public GameObject iconCompanion;

        private CrateArea crateArea; // Referensi ke skrip CombineCrate

        protected override void Start()
        {
            // Panggil method ini saat awalnya
            MatikanSemuaIkon();

            // Dapatkan referensi ke skrip CombineCrate
            crateArea = FindObjectOfType<CrateArea>();
            base.Start();
        }

        private void Update()
        {
            // Cek apakah pemain berada jauh dari grid
            if (!crateArea.IsPlayerNearGrid())
            {
                // Matikan semua ikon jika pemain jauh dari grid
                MatikanSemuaIkon();
            }
        }

        #region Callback
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Combine.performed += OnCombine;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Combine.performed -= OnCombine;
        }
        #endregion

        #region Callback Function
        private void OnCombine(InputAction.CallbackContext context)
        {
            MatikanSemuaIkon();
            // Implementasikan logika "combine" di sini.
            Debug.Log("Player has performed combine action.");

            // Dapatkan currentState dari CombineCrate
            CrateArea.ItemType currentState = crateArea.GetCurrentState();    

            // Tampilkan ikon sesuai dengan currentState hanya jika pemain berada di dekat grid
            if (crateArea.IsPlayerNearGrid())
            {
                switch (currentState)
                {
                    case CrateArea.ItemType.Item:
                        iconItem.SetActive(true);
                        break;
                    case CrateArea.ItemType.Medkit:
                        iconMedkit.SetActive(true);
                        break;
                    case CrateArea.ItemType.Weapon:
                        iconWeapon.SetActive(true);
                        break;
                    case CrateArea.ItemType.Companion:
                        iconCompanion.SetActive(true);
                        break;
                    default:
                        // Handle case when currentState is not recognized
                        break;
                }
            }
        }
        #endregion

        private void MatikanSemuaIkon()
        {
            // Matikan semua ikon
            iconItem.SetActive(false);
            iconMedkit.SetActive(false);
            iconWeapon.SetActive(false);
            iconCompanion.SetActive(false);
        }
    }
}