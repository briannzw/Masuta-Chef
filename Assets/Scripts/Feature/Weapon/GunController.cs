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

    private bool isFiringUltimate = false;
    [SerializeField] private float ultimateDuration;

    [Header("Ultimate Attack Properties")]
    public GameObject ultimateBulletObject;
    [SerializeField] private float ultimateBulletInterval = 1;
    [SerializeField] private float bulletAmount = 3;
    [SerializeField] private GameObject bulletSpawnPoint;
    #endregion

    protected new void Update()
    {
        base.Update();
    }

    #region Method
    public override void Attack()
    {
        var fireObject = Instantiate(fireObjectPrefab, transform.position, transform.rotation);
        var controller = fireObject.GetComponent<AOEController>();
        controller.Initialize(this);
        fireObject.GetComponent<Rigidbody>().velocity = transform.forward * fireObject.GetComponent<Bullet>().TravelSpeed;
    }

    public override void StartAttack()
    {
        if(isFiringUltimate) return;
        base.StartAttack();
    }

    protected override void UltimateAttack()
    {
        StartCoroutine(ShootFussiliBombs());
    }

    private IEnumerator ShootFussiliBombs()
    {
        isFiringUltimate = true;
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject gameObject = Instantiate(ultimateBulletObject, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            gameObject.GetComponent<Bullet>().weapon = this;
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * gameObject.GetComponent<Bullet>().TravelSpeed;
            yield return new WaitForSeconds(ultimateDuration / bulletAmount);
        }
        isFiringUltimate = false;
    }
    #endregion
}
