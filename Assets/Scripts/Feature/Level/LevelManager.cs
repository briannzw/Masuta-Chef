using Unity.AI.Navigation;
using UnityEngine;

namespace Level
{
    using Level.Area;
    using Wave;
    public class LevelManager : MonoBehaviour
    {
        public LevelData CurrentLevel;

        [Header("References")]
        public WaveManager waveManager;
        public Transform PlayerTransform;
        public NavMeshSurface[] surfaces;

        private void Awake()
        {
            SpawnLevelArea();
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
            waveManager.Spawners = levelArea.GetComponent<LevelAreaInfo>().Spawners;
        }

        private void SpawnPlayer()
        {
            // Spawn player at the specified position
            PlayerTransform.position = CurrentLevel.PlayerSpawnPoint;
        }
    }
}
