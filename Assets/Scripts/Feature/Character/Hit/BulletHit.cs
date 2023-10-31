using UnityEngine;

namespace Character.Hit
{
    using Spawner;
    public class BulletHit : HitController
    {
        [Header("Bullet Settings")]
        [SerializeField] protected bool isPiercing = false;
        private void OnTriggerEnter(Collider other)
        {
            // Check which object tag to be affected
            if (Source != null)
            {
                if (Source.Holder == null || string.IsNullOrEmpty(Source.TargetTag)) return;

                if (!other.CompareTag(Source.TargetTag)) return;
            }
            // This line will be deleted in future versions (every hit MUST have Source Weapon).
            else
            {
                if (string.IsNullOrEmpty(TargetTag)) return;
                if (!other.CompareTag(TargetTag)) return;
            }

            Character chara = other.GetComponent<Character>();
            if (chara == null) return;

            // Apply HIT and EFFECT
            Hit(chara);

            // Bullet Object must have SpawnObject component and spawned only from Spawner in Gun Weapon
            // This will return bullet object to pool
            if(!isPiercing) GetComponent<SpawnObject>().Release();

            // IF isPiercing is TRUE, the one that will Release the bullet object is BulletController (using travel distances).
        }
    }
}