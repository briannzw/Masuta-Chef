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
        [SerializeField] GameObject saberUltimateAttack;
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

        protected override void StartUltimateAttack()
        {
            animator.SetTrigger("Ultimate");
            StartCoroutine(UltimateVFX(1));
        }

        private IEnumerator UltimateVFX(float duration)
        {
            saberUltimateAttack.SetActive(true);
            yield return new WaitForSeconds(duration);
            saberUltimateAttack.SetActive(false);
        }
        #endregion
    }
}