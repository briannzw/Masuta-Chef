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
        [SerializeField] protected bool isLocalOffset = false;

        [Header("Rotations")]
        [SerializeField] private bool randomizeRotation;
        [SerializeField] private bool followSpawnerRotation;
        [SerializeField] private bool randomEulerRotation;
        [SerializeField] private Vector2 eulerRandomRange;

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

                Vector3 position = RandomSpawnPosition(transform, spawnOffset, spawnArea, isLocalOffset);
                Quaternion rotation = (followSpawnerRotation) ? transform.rotation : Quaternion.identity;
                rotation = (randomizeRotation) ? Random.rotation : rotation;
                rotation = (randomEulerRotation) ? Quaternion.Euler(0f, Random.Range(-eulerRandomRange.y / 2, eulerRandomRange.y / 2), 0f) * rotation : rotation;
                
                rotation.x = 0f; rotation.z = 0f;

                rotation = (randomEulerRotation) ? Quaternion.Euler(Random.Range(-eulerRandomRange.x / 2, eulerRandomRange.y / 2), 0f, 0f) * rotation : rotation;

                GameObject go = poolManager.Pools[spawnPrefab].Get();
                go.transform.position = position;
                go.transform.rotation = rotation;
                spawnObjects.Add(go);

                if(go.GetComponent<SpawnObject>() == null) go.AddComponent<SpawnObject>();
                go.GetComponent<SpawnObject>().Spawner = this;
                go.SetActive(true);
            }
            return spawnObjects;
        }

        protected virtual Vector3 RandomSpawnPosition(Transform center, Vector3 offset, Vector3 areaSize, bool local = false)
        {
            if(local)
                return center.TransformPoint(offset + new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    Random.Range(-areaSize.y / 2, areaSize.y / 2),
                    Random.Range(-areaSize.z / 2, areaSize.z / 2)));

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
            if(poolManager != null) poolManager.Remove(spawnPrefab);

            StopAllCoroutines();
        }


        #region GUI
        private void OnDrawGizmos()
        {
            if (isLocalOffset) Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(spawnOffset), transform.rotation, transform.lossyScale);
            else Gizmos.matrix = Matrix4x4.TRS(transform.position + spawnOffset, (followSpawnerRotation) ? transform.rotation : Quaternion.identity, transform.lossyScale);
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawCube(Vector3.zero, spawnArea);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, spawnArea);
        }
        #endregion
    }
}