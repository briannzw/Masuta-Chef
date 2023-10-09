using UnityEngine;

namespace Spawner
{
    public abstract class SpawnObject : MonoBehaviour
    {
        public Spawner Spawner;
        public virtual void Release()
        {
            if (Spawner == null) return;

            Spawner.OnRelease(this);
        }
    }
}