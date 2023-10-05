using UnityEngine;

namespace Loot.Object
{
    using MyBox;
    using Cooking.Recipe;

    public class RecipeLootObject : MonoBehaviour
    {
        [Tag]
        public string Tag;

        [Header("References")]
        //[SerializeField] private SaveManager saveManager;
        public Recipe Recipe;

        [Header("Follow")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float minSmoothSpeed = 2f;
        [SerializeField] private float maxSmoothSpeed = 4f;
        private Transform target;
        private Vector3 _velocity;

        private void Update()
        {
            if (target == null) return;
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(minSmoothSpeed, maxSmoothSpeed), maxSpeed);
            if (Vector3.Distance(transform.position, target.position) < 1f) AddBlueprint();
        }

        private void AddBlueprint()
        {
            // TODO: saveManager Handle Recipe Save Data
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(target != null) return;

            if (other.CompareTag(Tag))
            {
                target = other.transform;
                // TODO: Add Blueprint AND Save
                Recipe.AddBlueprint();
            }
        }
    }
}
