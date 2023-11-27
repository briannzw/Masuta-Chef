using Character;
using Character.Hit;
using Spawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NPC.NPCWeapon
{
    using Weapon;
    using Character;
    using Character.Hit;
    public class RangedController : Weapon
    {
        #region Properties
        public Animator WeaponAnimator;
        [SerializeField] private Spawner.Spawner bulletSpawner;
        #endregion

        private new void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
            if (rb == null) rb = GetComponentInParent<Rigidbody>();
            Holder = GetComponentInParent<Character>();
        }

        protected new void Update()
        {
            base.Update();
        }

        #region Method
        public override void Attack()
        {
            base.Attack();
            var bullets = bulletSpawner.Spawn();
            foreach (var bullet in bullets)
            {
                var controller = bullet.GetComponent<BulletHit>();
                controller.Initialize(this, damageScaling);
            }

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
        #endregion
    }
}