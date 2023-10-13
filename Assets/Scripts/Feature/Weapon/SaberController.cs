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
        #endregion

        #region Method
        public override void Attack()
        {

        }

        public override void StartAttack()
        {
            base.StartAttack();
            animator.SetTrigger("Attack");
        }
        #endregion
    }
}