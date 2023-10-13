using System.Collections.Generic;
using UnityEngine;

namespace Level.Area
{
    using AYellowpaper.SerializedCollections;
    using Spawner;

    public class LevelAreaInfo : MonoBehaviour
    {
        public SerializedDictionary<GameObject, List<NavMeshSpawner>> Spawners;
    }
}