using System.Collections.Generic;
using UnityEngine;

namespace Player.CompanionSlot
{
    using NPC.Companion;
    public class CompanionSlotManager : MonoBehaviour
    {
        [Header("References")]
        public List<Companion> companions = new List<Companion>();
        public List<Transform> slots = new List<Transform>();
        public int maxSlot = 4;

        public void AddCompanion(Companion companion)
        {
            if (companions.Count >= maxSlot)
            {
                Destroy(companions[0].gameObject);
                companions.RemoveAt(0);
            }

            companions.Add(companion);
            UpdateSlotFormation();
        }

        public void DeleteCompanion(Companion companion)
        {
            companions.RemoveAt(companion.GetComponent<Companion>().companionSpawnOrder);
            Destroy(companion.gameObject);
            UpdateSlotFormation();
        }

        private void UpdateSlotFormation()
        {
            for (int i = 0; i < companions.Count; i++)
            {
                if (i < slots.Count)
                {
                    companions[i].companionSlotPosition = slots[i];
                    companions[i].GetComponent<Companion>().companionSpawnOrder = i;
                }
            }
        }
    }
}