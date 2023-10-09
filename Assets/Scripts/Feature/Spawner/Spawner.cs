using Pool;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Spawner
{
    public class Spawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected PoolManager poolManager;
        [SerializeField] protected GameObject spawnPrefab;

        [Header("Parameters")]
        [SerializeField] private Vector3 spawnArea = Vector3.one;
        [SerializeField] protected Vector3 spawnOffset;
        [SerializeField] private bool randomizeRotation;

        [Header("Object Pool")]
        [SerializeField] protected int maxSpawnObjectInPool = 100;

        public Action<SpawnObject> OnRelease;

        public int GetActiveSpawnObject() => poolManager.Pools[spawnPrefab].CountActive;

        protected virtual void Start()
        {
            OnRelease += Release;
            poolManager.Add(spawnPrefab, maxSpawnObjectInPool);
        }

        public void Spawn(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                if (poolManager.Pools[spawnPrefab].CountActive >= maxSpawnObjectInPool)
                {
                    Debug.LogWarning("Max Object in Pool has reached its limit, " + spawnPrefab.name + " can't be spawned.");
                    break;
                }

                Vector3 position = RandomSpawnPosition(transform, spawnOffset, spawnArea);
                Quaternion rotation = (randomizeRotation) ? Random.rotation : Quaternion.identity;

                GameObject go = poolManager.Pools[spawnPrefab].Get();
                go.transform.position = position + spawnOffset;
                go.transform.rotation = rotation;
                go.GetComponent<SpawnObject>().Spawner = this;
            }
        }

        protected virtual Vector3 RandomSpawnPosition(Transform center, Vector3 offset, Vector3 areaSize)
        {
            return center.position + offset + new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    Random.Range(-areaSize.y / 2, areaSize.y / 2),
                    Random.Range(-areaSize.z / 2, areaSize.z / 2));
        }

        private void Release(SpawnObject obj)
        {
            if (obj == null) return;
            poolManager.Pools[spawnPrefab].Release(obj.gameObject);
        }

        private void OnDestroy()
        {
            OnRelease -= Release;
            poolManager.Remove(spawnPrefab);

            StopAllCoroutines();
        }


        #region GUI
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + spawnOffset, spawnArea);
        }
        #endregion
    }
}