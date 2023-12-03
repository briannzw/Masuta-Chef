using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    using LevelSelection;
    using Loot;
    using NPC.Data;
    using NPC.Enemy;
    using System.Linq;
    using Wave;
    [CreateAssetMenu(menuName = "Level/Level Data", fileName = "New Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        public new string name;
        public GameObject LevelAreaPrefab;
        public LevelSelectionInfo Info;
        public List<NPCData> EnemiesInLevel
        {
            get
            {
                HashSet<NPCData> list = new HashSet<NPCData>();
                foreach(var wave in Waves)
                {
                    foreach(var enemy in wave.EnemyList)
                    {
                        if (!enemy.Key.Prefab.CompareTag("Enemy")) continue;

                        if (!list.Contains(enemy.Key.Prefab.GetComponent<Enemy>().Data))
                            list.Add(enemy.Key.Prefab.GetComponent<Enemy>().Data);
                    }
                }
                return list.ToList();
            }
        }

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