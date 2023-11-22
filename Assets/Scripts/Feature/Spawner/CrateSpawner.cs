using System.Collections;
using UnityEngine;

namespace Spawner.Crate
{
    using Level;

    public class CrateSpawner : NavMeshSpawner
    {
        [SerializeField] private int amount;
        private LevelData levelData;

        private Coroutine spawnCoroutine;

        protected override void Start()
        {
            base.Start();
            levelData = GameManager.Instance.LevelManager.CurrentLevel;
            Enable();
        }

        private IEnumerator DoSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(levelData.CrateSpawnMinInterval, levelData.CrateSpawnMaxInterval));
                Spawn(amount);
            }
        }

        private void OnEnable()
        {
            if (levelData == null) return;
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }

        private void Enable()
        {
            if(spawnCoroutine != null) spawnCoroutine = null;
            spawnCoroutine = StartCoroutine(DoSpawn());
        }

        private void Disable()
        {
            if (spawnCoroutine == null) return;
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
