using System.Collections;
using UnityEngine;

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

        public void ReleaseAfter(float time)
        {
            StartCoroutine(Count(time));
        }

        private IEnumerator Count(float time)
        {
            yield return new WaitForSeconds(time);

            Release();
        }
    }
}