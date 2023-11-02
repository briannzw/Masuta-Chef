using Character.Hit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class SaberController : Weapon
    {
        #region Properties
        public LayerMask enemyLayer;

        [Header("References")]
        [SerializeField] HitController hitController;
        [SerializeField] Animator animator;
        [SerializeField] SaberVFXController vfxController;
        [SerializeField] GameObject saberUltimateApplicator;
        #endregion

        #region Method
        protected override void Awake()
        {
            base.Awake();
            weaponCollider.isTrigger = false;
        }

        public override void Attack()
        {
            base.Attack();
            hitController.Initialize(this);
            animator.SetTrigger("Attack");
        }

        public override void StartAttack()
        {
            base.StartAttack();
            weaponCollider.isTrigger = true;
        }

        public override void StopAttack()
        {
            base.StopAttack();
            weaponCollider.isTrigger = false;
        }

        protected override void UltimateAttack()
        {
            base.UltimateAttack();
            
            // Start Damage
            GameObject ultimate = Instantiate(saberUltimateApplicator, transform.position, transform.rotation);
            ultimate.GetComponent<AOEController>().Source = this;
            ultimate.GetComponent<AOEController>().AreaDuration = UltimateTimer;
            
            // Start VFX
            vfxController.StartVFX();
        }


        #endregion
    }
}