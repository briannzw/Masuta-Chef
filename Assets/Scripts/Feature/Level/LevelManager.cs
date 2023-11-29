using System;
using Unity.AI.Navigation;
using UnityEngine;

namespace Level
{
    using AYellowpaper.SerializedCollections;
    using Level.Area;
    using Wave;
    public class LevelManager : MonoBehaviour
    {
        public LevelData CurrentLevel;

        [Header("References")]
        public WaveManager waveManager;
        public Transform PlayerTransform;
        public NavMeshSurface[] surfaces;

        [Header("Data")]
        [SerializeField] protected SerializedDictionary<string, int> characterDied = new();

        private LevelAreaInfo levelAreaInfo;

        #region C# Events
        public Action OnLevelWin;
        public Action OnLevelLose;
        #endregion

        private void Awake()
        {
            SpawnLevelArea();
            SpawnPlayer();
            GenerateNavMesh();
        }

        private void GenerateNavMesh()
        {
            foreach (var surface in surfaces)
            {
                surface.BuildNavMesh();
            }
            Debug.Log("Navmesh Built");
        }

        private void SpawnLevelArea()
        {
            GameObject levelArea = Instantiate(CurrentLevel.LevelAreaPrefab, transform.position, Quaternion.identity);
            levelAreaInfo = levelArea.GetComponent<LevelAreaInfo>();
            waveManager.Spawners = levelAreaInfo.EnemySpawners;
        }

        private void SpawnPlayer()
        {
            // Spawn player at the specified position
            PlayerTransform.position = CurrentLevel.PlayerSpawnPoint;
        }

        public void CharacterDied(Character.Character chara)
        {
            if(!characterDied.ContainsKey(chara.tag)) characterDied.Add(chara.tag, 0);
            characterDied[chara.tag]++;

            // Action
            if (chara.CompareTag("Enemy"))
            {
                GameManager.Instance.OnEnemiesKilled?.Invoke();
                waveManager.EnemyDied();
            }

            // If All enemy in all Waves killed
            if (chara.CompareTag("Enemy") && characterDied["Enemy"] >= waveManager.TotalEnemies)
            {
                OnLevelWin?.Invoke();
                Debug.Log("[Game Over] Player Win (All Waves Cleared)");
            }

            if (chara.CompareTag("Player") && characterDied["Player"] >= 1)
            {
                OnLevelLose?.Invoke();
                Debug.Log("[Game Over] Player Lose (Health < 0)");
            }
        }

        public void DisableCrateSpawn()
        {
           levelAreaInfo.CrateSpawner.gameObject.SetActive(false);
        }

        public void EnableCrateSpawn()
        {
            levelAreaInfo.CrateSpawner.gameObject.SetActive(true);
        }
    }
}
