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
    #endregion

    protected new void Update()
    {
        base.Update();
    }

    #region Method
    public override void Attack()
    {
        List<GameObject> bullets = spawner.Spawn();
        // var fireObject = spawner.Spawn(fireObjectPrefab, transform.position, transform.rotation);
        foreach (GameObject bullet in bullets)
        {
            var controller = bullet.GetComponent<BulletHit>();
            controller.Initialize(this);
            bullet.transform.forward = transform.forward;
        }
    }
    #endregion
}
