using UnityEngine;

namespace Character.Hit
{
    using Module.Detector;

    public class ContinuousAOEHit : AOEController
    {
        [Header("Detection Settings")]
        [SerializeField] private float areaRadius = 5f;
        [SerializeField] private Vector3 areaOffset;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float areaAngle = 120f;

        [Header("Parameters")]
        public float Delay = 0;
        public float Interval = .5f;

        private void OnEnable()
        {
            InvokeRepeating("DoHit", Delay, Interval);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void DoHit()
        {
            characterInArea = ColliderDetector.Find<Character>(transform.position + areaOffset, areaRadius, targetMask, transform.forward, areaAngle);
            foreach(Character character in characterInArea.ToArray())
            {
                // Use AOEController HitChara
                HitChara(character);
            }
        }

#if UNITY_EDITOR
        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + areaOffset, areaRadius);
            Gizmos.color = Color.yellow;
            GizmosExtensions.DrawWireArc(transform.position + areaOffset, transform.forward, areaAngle, areaRadius);
        }
#endif
    }
}