using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private Vector3 spawnAreaSize;

    [SerializeField] private int maxSpawnCount = 10;
    [SerializeField] private int enemiesPerWave = 3;
    [SerializeField] private float waveInterval = 5f;
    [SerializeField] private int maxWaves = 5;

    private int currentSpawnCount = 0;
    private int currentWave = 0;
    private float timeSinceLastWave = 0f;

    private void SpawnPrefab()
    {
        Vector3 randomSpawnPoint = GetRandomSpawnPoint();

        // Check for overlap with obstacles before spawning
        Instantiate(enemyToSpawn, randomSpawnPoint, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 randomPoint = transform.position +
            new Vector3(Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f), 0f,
                        Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return randomPoint; // Fallback to original point if sampling fails
    }

    private void Start()
    {
        timeSinceLastWave = waveInterval - 1f;
    }

    private void Update()
    {
        timeSinceLastWave += Time.deltaTime;

        if (currentWave < maxWaves && currentSpawnCount < maxSpawnCount && timeSinceLastWave >= waveInterval)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnPrefab();
            }

            timeSinceLastWave = 0f;
            currentSpawnCount += enemiesPerWave;
            currentWave++;

            Debug.Log("Wave " + currentWave + " spawned.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
