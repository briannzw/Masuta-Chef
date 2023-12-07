using UnityEngine;

namespace Level
{
    using Player.Controller;
    using Weapon;

    public class WeaponSelection : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Weapon weapon;
        public bool IsSaber;

        private void Awake()
        {
            Invoke("LateStart", 0.05f);
            weapon.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        private void LateStart()
        {
            weapon.rb.isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                weapon.rb.isKinematic = false;
                if (IsSaber)
                {
                    weapon.transform.parent.SetParent(null);
                    weapon.Interact(other.gameObject);
                    return;
                }
                weapon.transform.SetParent(null);
                weapon.Interact(other.gameObject);
            }
        }
    }
}