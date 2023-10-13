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

    private ParticleSystem vfx;
    #endregion

    protected new void Start()
    {
        vfx = fireObjectPrefab.GetComponent<ParticleSystem>();
        AOEController aoecontroller = AttackArea.GetComponent<AOEController>();
        aoecontroller.Value.WeaponAttack = stats[WeaponStatsEnum.Power].Value;
        aoecontroller.TargetTag = this.TargetTag;
    }
    protected new void Update()
    {
        base.Update();
    }

    public override void StartAttack()
    {
        base.StartAttack();
        vfx.Play();
        AttackArea.SetActive(true);
    }

    public override void StopAttack()
    {
        base.StopAttack();
        vfx.Stop();
        AttackArea.SetActive(false);
    }

    #region Method
    public override void Attack()
    {
        
    }
    #endregion
}
