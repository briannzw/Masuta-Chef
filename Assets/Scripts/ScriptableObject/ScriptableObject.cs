using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Custom/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int levelNumber;
    public float levelDifficulty;

    // Prefab untuk area utama level
    public GameObject mainAreaPrefab;

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int numberOfEnemies;
        public GameObject[] enemyPrefabs;
    }

    public Wave[] waves;

    public class SpawnPoint
    {
        public string spawnPointName;
        public Vector3 spawnPosition;
    }

    public SpawnPoint playerSpawnPoint;

    [System.Serializable]
    public class EnemySpawn
    {
        public string spawnName;
        public GameObject enemyPrefab;
        public int spawnCount;
        public Vector3[] spawnPositions;
    }

    public EnemySpawn[] enemySpawns;
}
