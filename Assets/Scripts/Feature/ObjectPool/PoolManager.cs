using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Pool
{
    public class PoolManager : MonoBehaviour
    {
        #region Singleton
        public static PoolManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else Instance = this;
        }
        #endregion

        public Dictionary<GameObject, ObjectPool<GameObject>> Pools = new();

        public void Add(GameObject prefab, int maxObject)
        {
            if (Pools.ContainsKey(prefab)) return;

            Pools.Add(prefab, new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: (obj) => obj.SetActive(false),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: false,
                maxSize: maxObject));
        }

        public void Remove(GameObject prefab)
        {
            if (!Pools.ContainsKey(prefab)) return;

            Pools[prefab].Clear();
            Pools.Remove(prefab);
        }
    }
}
