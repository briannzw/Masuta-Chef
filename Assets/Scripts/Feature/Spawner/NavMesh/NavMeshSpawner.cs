using UnityEngine;
using UnityEngine.AI;

namespace Spawner
{
    public class NavMeshSpawner : Spawner
    {
        [Header("NavMesh Randomization")]
        [SerializeField] protected int maxRandomPosIteration = 1000;
        private int randomPosIteration = 0;

        protected override void Start()
        {
            base.Start();
            if (!NavMesh.SamplePosition(Vector3.zero, out var hit, 1000f, NavMesh.AllAreas))
            {
                Debug.LogError("There is no Navmesh Surface detected in the scene. Destroying '" + name + "' Spawner");
                Destroy(this);
            }
        }

        protected override Vector3 RandomSpawnPosition(Transform center, Vector3 offset, Vector3 areaSize)
        {
            NavMeshHit hit;
            Vector3 randomPos = base.RandomSpawnPosition(center, offset, areaSize);
            while (!NavMesh.SamplePosition(randomPos, out hit, 5.0f, 1 << NavMesh.GetAreaFromName("Walkable")))
            {
                randomPosIteration++;
                if (randomPosIteration >= maxRandomPosIteration) break;

                randomPos = base.RandomSpawnPosition(center, offset, areaSize);
            }

            return hit.position;
        }
    }
}