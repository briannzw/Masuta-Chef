using System.Collections.Generic;
using UnityEngine;

namespace HUD
{
    using Player.CompanionSlot;

    public class CompanionHUDManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CompanionSlotManager companionSlot;
        [SerializeField] private GameObject companionHUDItem;

        [Header("Parameters")]
        private int initialSlot;

        private List<CompanionHUDItem> hudItems = new();

        private void Awake()
        {
            initialSlot = companionSlot.maxSlot;

            for(int i = 0; i < initialSlot; i++)
            {
                GameObject itemGO = Instantiate(companionHUDItem, transform);
                hudItems.Add(itemGO.GetComponent<CompanionHUDItem>());
            }

            companionSlot.OnCompanionChanged += UpdateItems;
        }

        private void UpdateItems()
        {
            for(int i = 0; i < initialSlot; i++)
            {
                hudItems[i].UnsubToCompanion();

                if (i < companionSlot.companions.Count && companionSlot.companions[i] != null)
                    hudItems[i].SubToCompanion(companionSlot.companions[i]);
                else
                    hudItems[i].Reset();
            }
        }
    }
}