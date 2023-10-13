using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    using Loot;
    using Wave;
    [CreateAssetMenu(menuName = "Level/Level Data", fileName = "New Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        public new string name;
        public GameObject LevelAreaPrefab;
        public Vector3 PlayerSpawnPoint;
        public List<Wave> Waves;
        public LootChance EnemyLootDrop;
        public float CrateSpawnMinInterval;
        public float CrateSpawnMaxInterval;
    }
}