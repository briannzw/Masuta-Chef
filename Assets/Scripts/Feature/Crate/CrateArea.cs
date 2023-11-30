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

        private bool isSameColors = false;  
        private bool isCombined = false;

        private void Awake()
        {
            CrateGrid = new List<CrateController>();
            for(int i = 0; i < maxCrate; i++)
            {
                CrateGrid.Add(null);
            }

            combineManager.UpdateCombine(null);
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

                    CrateGrid[i].ExitPickup();
                    if (CrateGrid[i].GetComponent<SpawnObject>() != null)
                        CrateGrid[i].GetComponent<SpawnObject>().Release();
                    else Destroy(CrateGrid[i].gameObject);

                    CrateGrid[i] = null;
                    isCombined = true;
                    if (isCombined)
                    {
                        Debug.Log("COMBINEEEDD");
                    }
                }

                UpdateCombineUI();
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

            UpdateCombineUI();

            isSameColors = AreCratesSameColor();
        }

        public void OnStealed(IPickable pickable, GameObject stealer = null)
        {
            currentCrateCount--;
            if (!CrateGrid.Contains(pickable as CrateController)) return;
            CrateGrid[CrateGrid.IndexOf(pickable as CrateController)] = null;

            UpdateCombineUI();
        }

        public bool AreCratesSameColor()
        {
            if (CrateGrid.Count == 0 || CrateGrid[0] == null)
            {
                return false;
            }

            CrateColor firstColor = CrateGrid[0].crateColor;

            for (int i = 1; i < CrateGrid.Count; i++)
            {
                if (CrateGrid[i] != null && CrateGrid[i].crateColor != firstColor)
                {
                    return false;
                }
            }

            return true;
        }


        private void UpdateCombineUI()
        {
            List<CrateColor> colors = new List<CrateColor>();
            foreach (var crate in CrateGrid)
            {
                if (crate != null) colors.Add(crate.crateColor);
            }
            combineManager.UpdateCombine(colors);
        }

        public int GetCurrentCrateCount()
        {
            return currentCrateCount;
        }
        public bool GetInteractStatus()
        {
            return isCombined;
        }
        public bool GetCurrentCrateColor()
        {
            return isSameColors;
        }
    }
}