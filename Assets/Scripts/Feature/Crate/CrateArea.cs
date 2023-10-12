using System.Collections.Generic;
using UnityEngine;

namespace Crate.Area
{
    using Interaction;
    using Pickup;

    public class CrateArea : MonoBehaviour, IInteractable, IPicker
    {
        [SerializeField] private int maxCrate = 4;
        [SerializeField] private List<Transform> GridTransform = new();
        private List<CrateController> CrateGrid;

        private int currentCrateCount = 0;

        private void Awake()
        {
            CrateGrid = new List<CrateController>();
            for(int i = 0; i < maxCrate; i++)
            {
                CrateGrid.Add(null);
            }
        }

        public void Interact(GameObject other = null)
        {
            // DO Combine
            Debug.Log("TODO : Combine");
        }

        private void OnTriggerStay(Collider other)
        {
            // Tidak menerima Crate lain
            if (currentCrateCount >= maxCrate) return;

            if (other.CompareTag("Crate"))
            {
                CrateController crate = other.GetComponent<CrateController>();
                if (crate.IsHeld) return;

                if (crate.StartPickup(gameObject))
                {
                    currentCrateCount++;
                    SetCratePos(crate);
                }
            }
        }

        private void SetCratePos(CrateController crateObj)
        {
            int i;
            // Set posisi crate ke Grid terkecil yang belum diisi
            for(i = 0; i < CrateGrid.Count; i++)
            {
                if (CrateGrid[i] == null) break;
            }
            CrateGrid[i] = crateObj;
            crateObj.transform.parent = GridTransform[i];
            crateObj.transform.localPosition = Vector3.zero;
            crateObj.transform.localRotation = Quaternion.identity;
        }

        public void OnStealed(IPickable pickable, GameObject stealer = null)
        {
            currentCrateCount--;
            CrateGrid[CrateGrid.IndexOf(pickable as CrateController)] = null;
        }
    }
}