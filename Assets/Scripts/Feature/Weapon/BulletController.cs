using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun.Bullet
{
    public class BulletController : MonoBehaviour
    {
        [Tooltip("Time interval between bullet")]
        public float fireRate = .5f;
        [Tooltip("Bullet travel speed")]
        public float bulletSpeed = 10;
        public GameObject bulletPrefab;
        public Transform BulletSpawnTransform;

        private bool isCooldown = false;

        private Gun gun;

        private void OnEnable()
        {
            Shoot();
        }

        private void Awake()
        {
            gun = GetComponent<Gun>();
        }

        private void Update()
        {
            if (!isCooldown)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            var bullet = Instantiate(bulletPrefab, BulletSpawnTransform.position, BulletSpawnTransform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = BulletSpawnTransform.forward * bulletSpeed;
            bullet.GetComponent<Bullet>().source = gun;

            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {
            isCooldown = true;
            yield return new WaitForSeconds(fireRate);
            isCooldown = false;
        }
    }
}