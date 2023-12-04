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
    private bool isFiringUltimate = false;

    private bool isUltimateAttack = false;
    private bool isUltimateCooldown = false;
    [Header("Ultimate Attack Properties")]
    public GameObject ultimateBulletObject;
    [SerializeField] private float ultimateDuration;
    [SerializeField] private float bulletAmount = 4;
    [SerializeField] private Spawner.Spawner ultimateSpawner;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    #region Method
    public override void StartAttack()
    {
        base.StartAttack();
        OnAttack?.Invoke();
        if (!isUltimateCooldown)
        {
            //ShootUltimate();
        }
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }

    public override void Attack()
    {
        if (isFiringUltimate) return;
        
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
        StartCoroutine(ShootCreamyDeluge());
    }

    private IEnumerator ShootCreamyDeluge()
    {
        isFiringUltimate = true;

        for (int i = 0; i < bulletAmount; i++)
        {
            var creamyDeluges = ultimateSpawner.Spawn();
            foreach (var creamyDeluge in creamyDeluges)
            {
                var creamyHit = creamyDeluge.GetComponent<BulletHit>();
                creamyHit.Initialize(this, damageScaling);
            }
            yield return new WaitForSeconds(ultimateDuration / bulletAmount);
        }
        isFiringUltimate = false;
    }
    #endregion
}
