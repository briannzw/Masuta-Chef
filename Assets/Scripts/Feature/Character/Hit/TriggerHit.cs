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
                if (Source.Holder == null || Source.TargetTags.Count == 0) return;

                if (!Source.TargetTags.Contains(other.tag)) return;
            }

            Character chara = other.GetComponent<Character>();
            if (chara == null) return;

            // Apply HIT and EFFECT
            Hit(chara);
        }
    }
}