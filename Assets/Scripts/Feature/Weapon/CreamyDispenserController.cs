using Character.Hit;
using Module.Detector;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamyDispenserController : Weapon.Weapon
{
    [Header("Parameters")]
    [SerializeField] private Spawner.Spawner spawner;
    [SerializeField] private int timesPerSecond = 100;

    [Space]
    #region Properties
    public float maxRange;
    public LayerMask enemyLayer;

    private ParticleSystem vfx;

    private bool isUltimateAttack = false;
    private int ultimateBulletCount = 4;
    private bool isUltimateCooldown = false;
    [Header("Ultimate Attack Properties")]
    public GameObject ultimateBulletObject;
    [SerializeField] private float ultimateBulletInterval = 1;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    #region Method
    public override void StartAttack()
    {
        base.StartAttack();
        if(!isUltimateCooldown)
        {
            ShootUltimate();
        }
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }

    public override void Attack()
    {
        OnAttack?.Invoke();
        StartCoroutine(SpawnWithInterval(Mathf.RoundToInt(timesPerSecond * stats[Weapon.WeaponStatsEnum.Speed].Value / 100 * Time.deltaTime), Time.deltaTime));
    }

    private IEnumerator SpawnWithInterval(int totalSpawn, float totalTime)
    {
        for (int i = 0; i < totalSpawn; i++)
        {
            var go = spawner.Spawn();
            if(go.Count > 0) go[0].GetComponent<BulletHit>().Initialize(this, damageScaling);
            yield return new WaitForSeconds(totalTime / totalSpawn);
        }
    }

    private new void Update() { if (isFiring) Attack(); }

    protected override void UltimateAttack()
    {
        if (isUltimateAttack) return;
        StartUltimate();
    }

    private void ShootUltimate()
    {
        GameObject bullet = Instantiate(ultimateBulletObject, ultimateBulletObject.transform.position, ultimateBulletObject.transform.rotation);
        bullet.GetComponent<Bullet>().weapon = this;
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bullet.GetComponent<Bullet>().TravelSpeed;
        // if bullet exhausted, stop ultimate attack
        if(--ultimateBulletCount == 0)
        {
            StopUltimate();
        }
    }

    private void StartUltimate()
    {
        isUltimateAttack = true;
        ultimateBulletCount = 4;
    }

    private void StopUltimate()
    {
        isUltimateAttack = false;
    }

    private IEnumerator UltimateCooldown()
    {
        isUltimateCooldown = true;
        yield return new WaitForSeconds(ultimateBulletInterval);
        isUltimateCooldown = false;
    }
    #endregion
}
