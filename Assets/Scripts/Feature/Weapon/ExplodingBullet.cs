using System.Collections;
using System.Collections.Generic;
using Character.Hit;
using UnityEngine;
using Weapon;

public class ExplodingBullet : Bullet
{
    [Header("Explosion Properties")]
    public GameObject explosionApplicator;
    // Start is called before the first frame update
    protected override void OnHit()
    {
        GameObject gameObject = Instantiate(explosionApplicator, transform.position, transform.rotation);
        gameObject.GetComponentInChildren<AOEController>().Initialize(weapon);
    }
}
