using Character;
using Character.Hit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Weapon;

public class GunController : Weapon.Weapon
{
    #region Properties
    public GameObject fireObjectPrefab;
    #endregion

    protected new void Update()
    {
        base.Update();
    }

    #region Method
    public override void Attack()
    {
        var fireObject = Instantiate(fireObjectPrefab, transform.position, transform.rotation);
        var controller = fireObject.GetComponent<BulletHit>();
        controller.Initialize(this);
        fireObject.GetComponent<Rigidbody>().velocity = transform.forward * fireObject.GetComponent<Bullet>().TravelSpeed;
    }
    #endregion
}
