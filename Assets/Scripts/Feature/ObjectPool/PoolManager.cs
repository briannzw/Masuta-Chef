using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Pool
{
    public class PoolManager : MonoBehaviour
    {
        public Dictionary<GameObject, ObjectPool<GameObject>> Pools = new();

        public void Add(GameObject prefab, int maxObject)
        {
            if (Pools.ContainsKey(prefab)) return;

            Pools.Add(prefab, new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: (obj) => obj.SetActive(true),
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
