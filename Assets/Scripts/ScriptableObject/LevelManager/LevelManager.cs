using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevelData; // Drag and drop the LevelData asset in the Inspector
    private object playerPrefab;

    private void Start()
    {
        if (currentLevelData != null)
        {
            Debug.Log("Level Name: " + currentLevelData.levelName);
            Debug.Log("Level Number: " + currentLevelData.levelNumber);
            Debug.Log("Level Difficulty: " + currentLevelData.levelDifficulty);

            // Instantiate area utama level
            GameObject mainArea = Instantiate(currentLevelData.mainAreaPrefab, Vector3.zero, Quaternion.identity);

            // Spawn pemain di posisi yang telah ditentukan
            GameObject player = Instantiate(playerPrefab, currentLevelData.playerSpawnPoint.spawnPosition, Quaternion.identity);

            foreach (LevelData.EnemySpawn enemySpawn in currentLevelData.enemySpawns)
            {
                Debug.Log("Enemy Spawn Name: " + enemySpawn.spawnName);
                Debug.Log("Enemy Count: " + enemySpawn.spawnCount);

                for (int i = 0; i < enemySpawn.spawnCount; i++)
                {
                    // Instantiate musuh-musuh dalam jumlah yang ditentukan dan di posisi yang telah ditentukan
                    GameObject enemy = Instantiate(enemySpawn.enemyPrefab, enemySpawn.spawnPositions[i], Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.LogError("No LevelData assigned!");
        }
    }

    private GameObject Instantiate(object playerPrefab, Vector3 spawnPosition, Quaternion identity)
    {
        throw new NotImplementedException();
    }
}