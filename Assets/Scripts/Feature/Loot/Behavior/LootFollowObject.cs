using MyBox;
using UnityEngine;

namespace Loot.Object.Behaviour
{
    public class LootFollowObject : MonoBehaviour
    {
        [Tag]
        public string Tag;

        [Header("Follow")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float minSmoothSpeed = 2f;
        [SerializeField] private float maxSmoothSpeed = 4f;
        protected Transform target;
        private Vector3 _velocity;

        protected virtual void Update()
        {
            if (target == null) return;
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(minSmoothSpeed, maxSmoothSpeed), maxSpeed);
            if (Vector3.Distance(transform.position, target.position) < 1f) ReachedTarget();
        }

        protected virtual void ReachedTarget() { }
    }
}