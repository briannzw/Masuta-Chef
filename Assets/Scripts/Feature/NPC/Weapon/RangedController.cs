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
        public GameObject fireObjectPrefab;
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
            var fireObject = Instantiate(fireObjectPrefab, transform.position, transform.rotation);
            var controller = fireObject.GetComponent<BulletHit>();
            controller.Initialize(this, damageScaling);
            fireObject.GetComponent<Rigidbody>().velocity = transform.forward * fireObject.GetComponent<Bullet>().TravelSpeed;
        }

        public override void StartAttack()
        {
            base.StartAttack();
        }
        #endregion
    }
}
