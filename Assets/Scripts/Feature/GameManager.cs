using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnLocation; // Serialized private variable for spawn location
    [SerializeField] private List<GameObject> companionObj;
    [SerializeField] private List<Transform> slotFormation;
    public static Transform playerTransform;

    private List<GameObject> spawnedCompanions = new List<GameObject>(); // List to keep track of spawned companions

    private void Awake()
    {
        // Set the static playerTransform variable to this player's transform
        playerTransform = player;
    }

    void Update()
    {
        // Instantiate companions based on input keys
        for (int i = 1; i <= companionObj.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                if (spawnedCompanions.Count < 4) // Check if the limit is not exceeded
                {
                    GameObject newCompanion = Instantiate(companionObj[i - 1], spawnLocation.position, spawnLocation.rotation);
                    Companion companionComponent = newCompanion.GetComponent<Companion>();

                    // Set the companionSlotPosition based on the current slotFormation index
                    if (i - 1 < slotFormation.Count)
                    {
                        companionComponent.companionSlotPosition = slotFormation[spawnedCompanions.Count];
                    }

                    UpdateSlotFormation(); // Update slot formation for existing companions
                    spawnedCompanions.Insert(0, newCompanion); // Add the spawned companion to the beginning of the list (oldest position)
                }
                else
                {
                    // Replace the oldest companion
                    GameObject oldestCompanion = spawnedCompanions[spawnedCompanions.Count - 1];
                    Companion oldestCompanionComponent = oldestCompanion.GetComponent<Companion>();
                    int currentSlot = spawnedCompanions.Count;

                    // Set the companionSlotPosition of the oldest companion to the specified slotFormation index
                    if (currentSlot - 1 < slotFormation.Count)
                    {
                        oldestCompanionComponent.companionSlotPosition = slotFormation[currentSlot - 1];
                    }

                    Destroy(oldestCompanion); // Destroy the oldest companion
                    spawnedCompanions.RemoveAt(spawnedCompanions.Count - 1); // Remove it from the list

                    GameObject newCompanion = Instantiate(companionObj[i - 1], spawnLocation.position, spawnLocation.rotation);
                    Companion newCompanionComponent = newCompanion.GetComponent<Companion>();

                    // Set the companionSlotPosition of the new companion to the current slotFormation index
                    if (currentSlot - 1 < slotFormation.Count)
                    {
                        newCompanionComponent.companionSlotPosition = slotFormation[currentSlot - 1];
                    }

                    UpdateSlotFormation(); // Update slot formation for existing companions
                    spawnedCompanions.Insert(0, newCompanion); // Add the new companion to the beginning of the list
                }
            }
        }
    }

    void UpdateSlotFormation()
    {
        if (spawnedCompanions.Count >= 4)
        {
            for (int i = 0; i < spawnedCompanions.Count - 1; i++)
            {
                Companion companionComponent = spawnedCompanions[i].GetComponent<Companion>();
                if (i < slotFormation.Count)
                {
                    companionComponent.companionSlotPosition = slotFormation[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < spawnedCompanions.Count; i++)
            {
                Companion companionComponent = spawnedCompanions[i].GetComponent<Companion>();
                if (i < slotFormation.Count)
                {
                    companionComponent.companionSlotPosition = slotFormation[i];
                }
            }
        }
    }

}
