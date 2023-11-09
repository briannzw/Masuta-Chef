using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.AI;
using Player.CompanionSlot;
using NPC.Companion;

public class DebugCompanionSpawner : MonoBehaviour
{
    public enum CompanionSlot { First, Second, Third };
    [SerializeField] private SerializedDictionary<CompanionSlot, GameObject> Companions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnCompanion(CompanionSlot.First);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnCompanion(CompanionSlot.Second);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnCompanion(CompanionSlot.Third);
        }
    }

    void SpawnCompanion(CompanionSlot spawnOrder)
    {
        NavMesh.SamplePosition(transform.position + new Vector3 (4, 0, 4), out var hit, 10f, 1 << LayerMask.GetMask("Walkable"));
        GameObject companion;
        companion = Instantiate(Companions[spawnOrder], hit.position, Quaternion.identity);
        GameManager.Instance.PlayerTransform.GetComponent<CompanionSlotManager>().AddCompanion(companion.GetComponent<Companion>());
    }
}
