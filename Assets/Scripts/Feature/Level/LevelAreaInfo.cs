using System.Collections.Generic;
using UnityEngine;

namespace Level.Area
{
    using AYellowpaper.SerializedCollections;
    using Spawner;
    using Spawner.Crate;

    public class LevelAreaInfo : MonoBehaviour
    {
        [Header("Spawners")]
        public CrateSpawner CrateSpawner;
        public NavMeshSpawner DisasterSpawner;
        public SerializedDictionary<GameObject, List<NavMeshSpawner>> EnemySpawners;

        [Header("Lighting")]
        public Vector3 sunRotation = new Vector3(50f, -30f, 0f);
    }
}