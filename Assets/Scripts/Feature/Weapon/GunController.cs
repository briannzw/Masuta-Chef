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
    public Animator WeaponAnimator;

    private bool isFiringUltimate = false;
    [SerializeField] private float ultimateDuration;

    [Header("Ultimate Attack Properties")]
    public GameObject ultimateBulletObject;
    [SerializeField] private float ultimateBulletInterval = 1;
    [SerializeField] private float bulletAmount = 3;
    [SerializeField] private Spawner.Spawner bulletSpawner;
    #endregion

    protected new void Update()
    {
        base.Update();
    }

    #region Method
    public override void Attack()
    {
        base.Attack();
        var bullets = bulletSpawner.Spawn();
        foreach (var bullet in bullets)
        {
            var controller = bullet.GetComponent<BulletHit>();
            controller.Initialize(this, damageScaling);
        }

    }

    public override void StartAttack()
    {
        if (isFiringUltimate) return;
        if (WeaponAnimator == null) base.StartAttack();
        else
        {
            float animationLength = 0;
            foreach (AnimationClip clip in WeaponAnimator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "attack")
                {
                    animationLength = clip.length;
                }
            }

            WeaponAnimator.SetFloat("AttackSpeed", stats[WeaponStatsEnum.Speed].Value/100 * animationLength);
            WeaponAnimator.SetBool("IsAttacking", true);
        }

    }

    public override void StopAttack()
    {
        if (WeaponAnimator == null) base.StopAttack();
        else WeaponAnimator.SetBool("IsAttacking", false);

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
            GameObject gameObject = Instantiate(ultimateBulletObject, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
            gameObject.GetComponent<Bullet>().weapon = this;
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * gameObject.GetComponent<Bullet>().TravelSpeed;
            yield return new WaitForSeconds(ultimateDuration / bulletAmount);
        }
        isFiringUltimate = false;
    }
    #endregion
}
