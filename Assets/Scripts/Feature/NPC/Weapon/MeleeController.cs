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
        public Animator WeaponAnimator;

        #region Properties
        public LayerMask enemyLayer;      
        [SerializeField] HitController hitController;
        #endregion

        private new void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
            if (rb == null) rb = GetComponentInParent<Rigidbody>();
            weaponCollider = GetComponent<Collider>();
            Holder = GetComponentInParent<Character>();
            weaponCollider.isTrigger = true;
            hitController.Initialize(this, damageScaling);
        }

        protected new void Update()
        {
            base.Update();
        }

        public override void Attack()
        {
            base.Attack();
            weaponCollider.enabled = true;
            weaponCollider.isTrigger = true;
        }

        public override void StartAttack()
        {
            if (WeaponAnimator == null) base.StartAttack();
            else
            {
                float animationLength = 0;
                foreach (AnimationClip clip in WeaponAnimator.runtimeAnimatorController.animationClips)
                {
                    if (clip.name == "attack")
                    {
                        animationLength = clip.length;
                    }
                }
                WeaponAnimator.SetFloat("AttackSpeed", stats[WeaponStatsEnum.Speed].Value / 100 * animationLength);
                WeaponAnimator.SetBool("IsAttacking", true);
            }
        }

        public override void StopAttack()
        {
            if (WeaponAnimator == null) base.StopAttack();
            else WeaponAnimator.SetBool("IsAttacking", false);
        }
    }
}

