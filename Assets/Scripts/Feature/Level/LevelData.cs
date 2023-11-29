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
        [TextArea] public string Description;

        [Header("Level Parameters")]
        public Vector3 PlayerSpawnPoint;
        public LootChance EnemyLootDrop;

        [Header("Wave Info")]
        public List<Wave> Waves;
        public int WaveWaitTime = 15;
        public float CrateSpawnMinInterval;
        public float CrateSpawnMaxInterval;
        public float DisasterMinInterval;
        public float DisasterMaxInterval;
    }
}