using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace wavemanager
{
    public class WaveManager : MonoBehaviour
    {
        public LevelData levelData;
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

            // Spawn enemies for the current wave
            levelData.enemySpawns[currentWaveIndex].SpawnEnemies(currentWave.enemyPrefabs, currentWave.spawnPositions, currentWave.spawnCount);

            EnableNavMeshAgents(currentWave.enemyPrefabs);

            isWaveInProgress = true;
            currentWaveIndex++;

            if (currentWaveIndex < levelData.waves.Length)
            {
                StartCoroutine(StartNextWaveWithDelay(levelData.waves[currentWaveIndex].spawnTime));
            }
        }

        private void EnableNavMeshAgents(GameObject[] enemyPrefabs)
        {
            foreach (var enemyPrefab in enemyPrefabs)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
                if (navMeshAgent != null)
                {
                    navMeshAgent.enabled = true;
                }
            }
        }

        private IEnumerator StartNextWaveWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            isWaveInProgress = false;
        }
    }
}
