using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Spawner;
using UnityEngine;
using UnityEngine.AI;

namespace wavemanager
{
    public class WaveManager : MonoBehaviour
    {
        public LevelData levelData;
        public NavMeshSpawner spawner;
        private int currentWaveIndex = 0;
        private float gameTime = 0f;
        private bool isWaveInProgress = false;

        private void Update()
        {
            gameTime += Time.deltaTime;

            if (!isWaveInProgress && currentWaveIndex < levelData.waves.Length)
            {
                if (gameTime >= levelData.waves[currentWaveIndex].spawnTime)
                {
                    StartNextWave();
                }
            }
        }

        private void StartNextWave()
        {
            LevelData.Wave currentWave = levelData.waves[currentWaveIndex];
            Debug.Log($"Spawn Wave {currentWaveIndex + 1}");

            foreach (var enemyPrefab in currentWave.enemyPrefabs)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
                if (navMeshAgent != null)
                {
                    navMeshAgent.enabled = true;
                }
            }

            int spawnCount = (int)currentWave.spawnCount;
            int spawnMod = spawnCount % spawner.GetSpawners(currentWave.enemyPrefabs[0]).Count;
            int spawnPerSpawner = spawnCount / spawner.GetSpawners(currentWave.enemyPrefabs[0]).Count;

            for (int i = 0; i < spawner.GetSpawners(currentWave.enemyPrefabs[0]).Count; i++)
            {
                int countToSpawn = i < spawnMod ? spawnPerSpawner + 1 : spawnPerSpawner;
                spawner.GetSpawners(currentWave.enemyPrefabs[0])[i].Spawn(currentWave.enemyPrefabs[0], countToSpawn);
            }

            isWaveInProgress = true;
            currentWaveIndex++;

            if (currentWaveIndex < levelData.waves.Length)
            {
                StartCoroutine(StartNextWaveWithDelay(levelData.waves[currentWaveIndex].spawnTime));
            }
            else
            {
                isWaveInProgress = false;
            }
        }

        private IEnumerator StartNextWaveWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            isWaveInProgress = false;
        }
    }
}
