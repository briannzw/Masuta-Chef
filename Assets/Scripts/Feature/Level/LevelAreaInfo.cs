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
        public float ColorTemperature = 5000f;
        public float LightIntensity = 1f;
    }
}