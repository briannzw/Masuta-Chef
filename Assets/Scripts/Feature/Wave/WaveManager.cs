using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

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
        public SerializedDictionary<GameObject, List<NavMeshSpawner>> Spawners = new();

        [Header("UI References")]
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private TMP_Text waveEnemiesText;
        [SerializeField] private TMP_Text nextWaveText;
        [SerializeField] private float incomingWaveDuration = 5f;

        private int currentWaveIndex = 0;
        private int currentEnemyCount = 0;

        private HashSet<GameObject> spawnedEnemies = new();

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

        private void Start()
        {
            StartWave(LevelData.Waves[0]);
        }

        public void EnemyDied()
        {
            if (currentWaveIndex >= LevelData.Waves.Count - 1) return;

            currentEnemyCount++;
            waveEnemiesText.text = $"Enemies Left: {LevelData.Waves[currentWaveIndex].TotalEnemies - currentEnemyCount}";

            if (currentEnemyCount == LevelData.Waves[currentWaveIndex].TotalEnemies)
            {
                currentEnemyCount = 0;
                StartCoroutine(CountdownWave());
            }
        }

        private void StartWave(Wave wave)
        {
            // UI
            waveText.text = $"Wave {currentWaveIndex + 1}";
            waveEnemiesText.text = $"Enemies Left: {wave.TotalEnemies}";

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

                    foreach (var enemy in enemies)
                    {
                        if(enemy.GetComponent<Character>() == null) enemy.AddComponent<Character>();
                        enemy.GetComponent<Character>().StatsPreset = enemyPreset;
                        enemy.GetComponent<Character>().Reset();
                        if(enemy.GetComponent<LootDropController>() == null) enemy.AddComponent<LootDropController>();
                        enemy.GetComponent<LootDropController>().lootChance = LevelData.EnemyLootDrop;

                        if(!spawnedEnemies.Contains(enemy)) spawnedEnemies.Add(enemy);
                    }
                    total -= spawnCount;
                    timer = 0f;
                }

                yield return null;
            }
        }

        private IEnumerator CountdownWave()
        {
            nextWaveText.enabled = true;

            int time = LevelData.WaveWaitTime;
            while(time > 0)
            {
                nextWaveText.text = $"Next wave in\n{time}";
                yield return new WaitForSeconds(1f);
                time--;
            }

            currentWaveIndex++;
            StartWave(LevelData.Waves[currentWaveIndex]);

            nextWaveText.text = "INCOMING WAVE";
            yield return new WaitForSeconds(incomingWaveDuration);
            nextWaveText.enabled = false;
        }

        #region NavMeshAgent
        public void DisableWave()
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null && enemy.activeSelf)
                {
                    if (enemy.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                    {
                        navMeshAgent.isStopped = true;
                    }
                }
            }
        }
        public void EnableWave()
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null && enemy.activeSelf)
                {
                    if (enemy.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                    {
                        navMeshAgent.isStopped = false;
                    }
                }
            }
        }
        #endregion
    }
}
