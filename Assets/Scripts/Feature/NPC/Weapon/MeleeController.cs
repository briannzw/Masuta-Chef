using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.NPCWeapon
{
    using Weapon;
    using Character.Hit;
    public class MeleeController : Weapon
    {
        #region Properties
        public LayerMask enemyLayer;      
        [SerializeField] HitController hitController;
        [SerializeField] Animator animator;
        #endregion

        protected new void Update()
        {
            base.Update();
        }

        public override void Attack()
        {
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
    }
}

