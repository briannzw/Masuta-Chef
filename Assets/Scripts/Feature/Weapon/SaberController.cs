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
        [SerializeField] Collider collider;
        #endregion

        #region Method
        private void Awake() {
            collider.enabled = false;
        }

        public override void Attack()
        {

        }

        public override void StartAttack()
        {
            base.StartAttack();
            collider.enabled = true;
            hitController.Initialize(this);
            animator.SetTrigger("Attack");
        }

        public override void StopAttack()
        {
            base.StopAttack();
            collider.enabled = false;
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