using Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Spawner
{
    public class Spawner : MonoBehaviour
    {
        [Header("References")]
        protected PoolManager poolManager;
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
            poolManager = PoolManager.Instance;
            OnRelease += Release;
            poolManager.Add(spawnPrefab, maxSpawnObjectInPool);
        }

        public List<GameObject> Spawn(int amount = 1)
        {
            List<GameObject> spawnObjects = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                if (poolManager.Pools[spawnPrefab].CountActive >= maxSpawnObjectInPool)
                {
                    Debug.LogWarning("Max Object in Pool has reached its limit, " + spawnPrefab.name + " can't be spawned.");
                    break;
                }

                Vector3 position = RandomSpawnPosition(transform, spawnOffset, spawnArea);
                Quaternion rotation = (randomizeRotation) ? Random.rotation : Quaternion.identity;
                rotation.x = 0f; rotation.z = 0f;

                GameObject go = poolManager.Pools[spawnPrefab].Get();
                go.transform.position = position;
                go.transform.rotation = rotation;
                spawnObjects.Add(go);

                if(go.GetComponent<SpawnObject>() == null) go.AddComponent<SpawnObject>();
                go.GetComponent<SpawnObject>().Spawner = this;
            }
            return spawnObjects;
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