using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun.Bullet
{
    public class Bullet : MonoBehaviour
    {
        public float aliveTime = 3f;
        // TODO : Bullet owner (so it doesn't damage self)
        public Gun source;

        private void Start()
        {
            Destroy(this.gameObject, aliveTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO : Bullet behaviour
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Character.Character>().TakeDamage(source.Character, source.DamageScale);
            }
        }
    }
}