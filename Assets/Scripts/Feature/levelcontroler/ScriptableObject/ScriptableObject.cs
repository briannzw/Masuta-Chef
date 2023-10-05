using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Custom/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int levelNumber;
    public float levelDifficulty;

    public GameObject mainAreaPrefab;
    public SpawnPoint playerSpawnPoint;
    public Wave[] waves;
    public EnemySpawn[] enemySpawns;

    public Stats playerStats;
    public CrateSpawnInterval crateSpawnInterval; 

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int numberOfEnemies;
        public GameObject[] enemyPrefabs;
        internal float spawnTime;
        internal object spawnPositions;
        internal object spawnCount;
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public string spawnPointName;
        public Vector3 spawnPosition;
    }

    [System.Serializable]
    public class EnemySpawn
    {
        public string spawnName;
        public GameObject enemyPrefab;
        public int spawnCount;
        public Vector3[] spawnPositions;

        internal void SpawnEnemies(GameObject[] enemyPrefabs, object spawnPositions, object spawnCount)
        {
            throw new NotImplementedException();
        }

        internal void SpawnEnemies(GameObject enemyPrefab, Vector3[] spawnPositions, int spawnCount)
        {
            throw new NotImplementedException();
        }
    }

    [System.Serializable]
    public class Stats
    {
        public int health;
        public int attackDamage;
        public float movementSpeed;
    }

    [System.Serializable]
    public class CrateSpawnInterval
    {
        public float crateSpawnMinInterval; 
        public float crateSpawnMaxInterval; 
    }

    [System.Serializable]
    public class StatsPreset
    {
        public string presetName;
        public Stats stats;
    }
}

