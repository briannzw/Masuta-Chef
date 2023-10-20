using Character.Hit;
using Module.Detector;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamyDispenserController : Weapon.Weapon
{
    #region Properties
    public GameObject fireObjectPrefab;
    public float maxRange;
    public LayerMask enemyLayer;
    public GameObject AttackArea;

    [SerializeField] private ParticleSystem vfx;

    // Ultimate Attack
    private bool isUltimateAttack = false;
    private int ultimateBulletCount = 4;
    private bool isUltimateCooldown = false;
    [Header("Ultimate Attack Properties")]
    public GameObject ultimateBulletObject;
    [SerializeField] private float ultimateBulletInterval = 1;
    #endregion

    protected new void Start()
    {
        base.Start();
        //vfx = fireObjectPrefab.GetComponent<ParticleSystem>();
        AOEController aoeController = AttackArea.GetComponent<AOEController>();
        aoeController.Initialize(this);
        // this.StopAttack();
    }
    protected new void Update()
    {
        //base.Update();
    }


    #region Method
    public override void Attack()
    {

    }
    public override void StartAttack()
    {
        //base.StartAttack();
        if (!isUltimateAttack)
        {
            vfx.Play();
            AttackArea.SetActive(true);
        }
        else if(!isUltimateCooldown)
        {
            ShootUltimate();
        }
    }

    public override void StopAttack()
    {
        //base.StopAttack();
        if (!isUltimateAttack)
        {
            vfx.Stop();
            AttackArea.SetActive(false);
        }
    }

    public override void UltimateAttack()
    {
        if (isUltimateAttack) return;
        StartUltimate();
    }

    private void ShootUltimate()
    {
        GameObject bullet = Instantiate(ultimateBulletObject, fireObjectPrefab.transform.position, fireObjectPrefab.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bullet.GetComponent<Bullet>().TravelSpeed; ;
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

        AttackArea.SetActive(false);
    }

    private void StopUltimate()
    {
        isUltimateAttack = false;
        AttackArea.SetActive(true);
    }

    private IEnumerator UltimateCooldown()
    {
        isUltimateCooldown = true;
        yield return new WaitForSeconds(ultimateBulletInterval);
        isUltimateCooldown = false;
    }
    #endregion
}
