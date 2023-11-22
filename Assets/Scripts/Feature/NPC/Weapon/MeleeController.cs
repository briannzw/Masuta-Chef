using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.NPCWeapon
{
    using Weapon;
    using Character;
    using Character.Hit;
    public class MeleeController : Weapon
    {
        #region Properties
        public LayerMask enemyLayer;      
        [SerializeField] HitController hitController;
        [SerializeField] Animator animator;
        #endregion

        private new void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
            if (rb == null) rb = GetComponentInParent<Rigidbody>();
            weaponCollider = GetComponent<Collider>();
            Holder = GetComponentInParent<Character>();
        }

        protected new void Update()
        {
            base.Update();
        }

        public override void Attack()
        {
            base.Attack();
            hitController.Initialize(this, damageScaling);
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

