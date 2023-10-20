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

        [SerializeField] HitController hitController;
        [SerializeField] Animator animator;
        [SerializeField] SaberVFXController vfxController;
        #endregion

        #region Method

        public override void Attack()
        {

        }

        public override void StartAttack()
        {
            base.StartAttack();
            hitController.Initialize(this);
            animator.SetTrigger("Attack");
        }

        protected override void UltimateAttack()
        {
            base.UltimateAttack();
            vfxController.StartVFX();
        }
        #endregion
    }
}