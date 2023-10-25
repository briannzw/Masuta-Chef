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

        public override void StartAttack()
        {
            if(attackTimer > stats[WeaponStatsEnum.Speed].Value / 100)
            {
                base.StartAttack();
                hitController.Initialize(this);
                animator.SetTrigger("Attack");
                attackTimer = 0;
            }

        }
    }
}

