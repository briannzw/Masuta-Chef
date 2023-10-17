using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wave
{
    using AYellowpaper.SerializedCollections;
    using Level;
    using Loot;
    using Spawner;
    public class WaveManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelManager levelManager;
        protected LevelData LevelData;
        public SerializedDictionary<GameObject, List<NavMeshSpawner>> Spawners = new SerializedDictionary<GameObject, List<NavMeshSpawner>>();

        private float gameTime = 0f;
        private int currentWaveIndex = 0;

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
                    if (i < spawnMod) StartCoroutine(SpawnEnemies(Spawners[enemy.Key.Prefab][i], spawnCount + 1, wave.SpawnPerInterval, wave.SpawnInterval));
                    else StartCoroutine(SpawnEnemies(Spawners[enemy.Key.Prefab][i], spawnCount, wave.SpawnPerInterval, wave.SpawnInterval));
                }
            }
        }

        private IEnumerator SpawnEnemies(NavMeshSpawner spawner, int total, int spawnCount, float interval)
        {
            float timer = 0f;
            while (total > 0)
            {
                timer += Time.deltaTime;
                if(timer >= interval)
                {
                    if(total < spawnCount)
                    {
                        spawner.Spawn(total);
                        break;
                    }
                    List<GameObject> enemies = spawner.Spawn(spawnCount);
                    foreach(var enemy in enemies)
                    {
                        if(enemy.GetComponent<LootDropController>() == null) enemy.AddComponent<LootDropController>();
                        enemy.GetComponent<LootDropController>().lootChance = LevelData.EnemyLootDrop;
                    }
                    total -= spawnCount;
                    timer = 0f;
                }
                yield return null;
            }
        }

    }
}
