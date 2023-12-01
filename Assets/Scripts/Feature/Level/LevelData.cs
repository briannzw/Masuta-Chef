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
        [Range(0f, 1f)] public float LoseDropMultiplier = .5f;

        [Header("Wave Info")]
        public List<Wave> Waves;
        public int WaveWaitTime = 15;
        public float CrateSpawnMinInterval;
        public float CrateSpawnMaxInterval;

        [Header("Disaster Mode")]
        public float DisasterDamageScaling = 1f;
        public float DisasterStartTime = 30f;
        public float DisasterDuration = 10f;
        public float DisasterMinInterval;
        public float DisasterMaxInterval;
    }
}