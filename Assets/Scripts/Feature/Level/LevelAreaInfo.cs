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
        public SerializedDictionary<GameObject, List<NavMeshSpawner>> EnemySpawners;
    }
}