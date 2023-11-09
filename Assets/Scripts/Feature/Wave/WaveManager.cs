using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wave
{
    using AYellowpaper.SerializedCollections;
    using Level;
    using Loot;
    using Spawner;
    using Character;
    using Character.Stat;

    public class WaveManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelManager levelManager;
        protected LevelData LevelData;
        public SerializedDictionary<GameObject, List<NavMeshSpawner>> Spawners = new SerializedDictionary<GameObject, List<NavMeshSpawner>>();

        private float gameTime = 0f;
        private int currentWaveIndex = 0;

        private List<GameObject> spawnedEnemies = new List<GameObject>();

        public int TotalEnemies
        {
            get
            {
                int total = 0;
                foreach (var wave in LevelData.Waves)
                {
                    total += wave.TotalEnemies;
                }
                return total;
            }
        }

        private void Awake()
        {
            LevelData = levelManager.CurrentLevel;
        }

        private void Update()
        {
            if (currentWaveIndex >= LevelData.Waves.Count) return;

            gameTime += Time.deltaTime;

            if (gameTime >= LevelData.Waves[currentWaveIndex].Time)
            {
                StartWave(LevelData.Waves[currentWaveIndex++]);
            }
        }

        private void StartWave(Wave wave)
        {
            foreach (var enemy in wave.EnemyList)
            {
                int spawnMod = enemy.Value % Spawners[enemy.Key.Prefab].Count;
                int spawnCount = enemy.Value / Spawners[enemy.Key.Prefab].Count;
                for (int i = 0; i < Spawners[enemy.Key.Prefab].Count; i++)
                {
                    if (i < spawnMod) StartCoroutine(SpawnEnemies(Spawners[enemy.Key.Prefab][i], spawnCount + 1, wave.SpawnPerInterval, wave.SpawnInterval, enemy.Key.StatsPreset));
                    else StartCoroutine(SpawnEnemies(Spawners[enemy.Key.Prefab][i], spawnCount, wave.SpawnPerInterval, wave.SpawnInterval, enemy.Key.StatsPreset));
                }
            }
        }

        private IEnumerator SpawnEnemies(NavMeshSpawner spawner, int total, int spawnCount, float interval, StatsPreset enemyPreset)
        {
            float timer = 0f;
            while (total > 0)
            {
                timer += Time.deltaTime;
                if(timer >= interval)
                {
                    List<GameObject> enemies = spawner.Spawn(total < spawnCount ? total : spawnCount);
                    spawnedEnemies.AddRange(enemies);

                    foreach (var enemy in enemies)
                    {
                        if(enemy.GetComponent<Character>() == null) enemy.AddComponent<Character>();
                        enemy.GetComponent<Character>().StatsPreset = enemyPreset;
                        enemy.GetComponent<Character>().InitializeStats();
                        if(enemy.GetComponent<LootDropController>() == null) enemy.AddComponent<LootDropController>();
                        enemy.GetComponent<LootDropController>().lootChance = LevelData.EnemyLootDrop;
                    }
                    total -= spawnCount;
                    timer = 0f;
                }
                yield return null;
            }
        }

        #region NavMeshAgent
        public void DisableNavMeshAgents()
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null && enemy.CompareTag("Enemy"))
                {
                    UnityEngine.AI.NavMeshAgent navMeshAgent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (navMeshAgent != null)
                    {
                        navMeshAgent.enabled = false;
                    }
                }
            }
        }
        public void EnableNavMeshAgents()
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null && enemy.CompareTag("Enemy"))
                {
                    UnityEngine.AI.NavMeshAgent navMeshAgent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (navMeshAgent != null)
                    {
                        navMeshAgent.enabled = true;
                    }
                }
            }
        }
        #endregion
    }
}
