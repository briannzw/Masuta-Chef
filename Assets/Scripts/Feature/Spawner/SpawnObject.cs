using System.Collections;
using System.Collections.Generic;using UnityEngine;

namespace Spawner
{
    public class SpawnObject : MonoBehaviour
    {
        public Spawner Spawner;
        public virtual void Release()
        {
            if (Spawner == null) return;

            Spawner.OnRelease(this);
        }
    }
}