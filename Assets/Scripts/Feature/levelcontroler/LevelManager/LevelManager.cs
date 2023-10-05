using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static LevelData;

namespace LevelManager
{ 
public class LevelManager : MonoBehaviour
   {
    public LevelData currentLevelData; // Drag and drop the LevelData asset in the Inspector
    public EnemySpawn enemySpawner; // Drag and drop the EnemySpawner script in the Inspector
    public GameObject levelAreaPrefab; // Drag and drop the Level Area prefab in the Inspector
    public GameObject playerPrefab; // Assuming playerPrefab is a GameObject

    private void Start()
    {
        if (currentLevelData != null)
        {
            Debug.Log("Level Name: " + currentLevelData.levelName);
            Debug.Log("Level Number: " + currentLevelData.levelNumber);
            Debug.Log("Level Difficulty: " + currentLevelData.levelDifficulty);

            SpawnLevelArea();
            SpawnPlayer();
            SpawnEnemies();
        }
        else
        {
            Debug.LogError("No LevelData assigned!");
        }
    }

    private void SpawnLevelArea()
    {
        // Instantiate main level area and generate NavMesh
        GameObject mainArea = Instantiate(levelAreaPrefab, Vector3.zero, Quaternion.identity);
        NavMeshSurface navMeshSurface = mainArea.GetComponent<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("NavMeshSurface component not found on the main area. NavMesh not generated.");
        }
    }

    private void SpawnPlayer()
    {
        // Spawn player at the specified position
        GameObject player = Instantiate(playerPrefab, currentLevelData.playerSpawnPoint.spawnPosition, Quaternion.identity);
    }

    private void SpawnEnemies()
    {
        foreach (LevelData.EnemySpawn enemySpawn in currentLevelData.enemySpawns)
        {
            Debug.Log("Enemy Spawn Name: " + enemySpawn.spawnName);
            Debug.Log("Enemy Count: " + enemySpawn.spawnCount);

            // Tell the EnemySpawner to spawn enemies
            enemySpawner.SpawnEnemies(enemySpawn.enemyPrefab, enemySpawn.spawnPositions, enemySpawn.spawnCount);
        }
    }
  }
}
