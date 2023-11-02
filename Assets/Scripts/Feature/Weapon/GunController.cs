using Character;
using Character.Hit;
using Spawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Weapon;

public class GunController : Weapon.Weapon
{
    #region Properties
    public GameObject fireObjectPrefab;
    public Spawner.Spawner spawner;
    [SerializeField] AudioSource weaponSound; 
    #endregion

    protected new void Update()
    {
        base.Update();
    }

    #region Method
    public override void Attack()
    {
        List<GameObject> bullets = spawner.Spawn();

        // Memainkan efek suara senjata saat serangan dimulai
        if (weaponSound != null)
        {
            weaponSound.Play();
        }

        foreach (GameObject bullet in bullets)
        {
            var controller = bullet.GetComponent<BulletHit>();
            controller.Initialize(this);
            bullet.transform.forward = transform.forward;
        }
    }
    #endregion
}
