using UnityEngine;

namespace Character.Hit
{
    public class TriggerHit : HitController
    {
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
        }
    }
}