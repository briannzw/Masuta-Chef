using System.Collections;
using UnityEngine;

namespace Spawner.Crate
{
    using Level;

    public class CrateSpawner : NavMeshSpawner
    {
        [SerializeField] private int amount;
        private LevelData levelData;

        protected override void Start()
        {
            base.Start();
            levelData = GameManager.Instance.LevelManager.CurrentLevel;
            StartCoroutine(DoSpawn());
        }

        private IEnumerator DoSpawn()
        {
            yield return new WaitForSeconds(Random.Range(levelData.CrateSpawnMinInterval, levelData.CrateSpawnMaxInterval));
            Spawn(amount);
            StartCoroutine(DoSpawn());
        }
    }
}
