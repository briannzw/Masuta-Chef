using System.Collections.Generic;
using UnityEngine;

namespace Crate.Area
{
    using Crate.Combine;
    using Interaction;
    using Pickup;
    using Spawner;

    public class CrateArea : MonoBehaviour, IInteractable, IPicker
    {
        [Header("References")]
        [SerializeField] private CombineManager combineManager;
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
            List<CrateColor> colors = new List<CrateColor>();
            foreach(var crate in CrateGrid)
            {
                if (crate != null) colors.Add(crate.crateColor);
            }

            if (combineManager.Combine(colors))
            {
                currentCrateCount -= colors.Count;
                for (int i = 0; i < CrateGrid.Count; i++)
                {
                    if (CrateGrid[i] == null) continue;

                    if (CrateGrid[i].GetComponent<SpawnObject>() != null)
                        CrateGrid[i].GetComponent<SpawnObject>().Release();
                    else Destroy(CrateGrid[i].gameObject);

                    CrateGrid[i] = null;
                }
            }
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
            if (!CrateGrid.Contains(pickable as CrateController)) return;
            CrateGrid[CrateGrid.IndexOf(pickable as CrateController)] = null;
        }
    }
}