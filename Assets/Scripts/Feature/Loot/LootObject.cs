
namespace Loot.Object
{
    using UnityEngine;

    public abstract class LootObject : MonoBehaviour
    {
        [SerializeField] private float Lifetime = 20f;
        private void Start()
        {
            if(Lifetime > 0)
                Destroy(gameObject, Lifetime);
        }
    }
}