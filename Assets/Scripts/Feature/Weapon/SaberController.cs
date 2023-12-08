using Character.Hit;
using System.Drawing;
using UnityEngine;
using UnityEngine.VFX;

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

        [SerializeField] GameObject slashVFXPrefab;
        public Vector3 vfxSize;
        #endregion

        #region Method
        protected override void Awake()
        {
            base.Awake();
            weaponCollider.isTrigger = false;
        }

        public override void Attack()
        {
            base.Attack();
            hitController.Initialize(this, damageScaling);
            animator.SetTrigger("Attack");

            if(slashVFXPrefab != null) SpawnVFX();
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

        protected override void UltimateAttack()
        {
            base.UltimateAttack();
            
            // Start Damage
            GameObject ultimate = Instantiate(saberUltimateApplicator, transform.position, transform.rotation);
            ultimate.GetComponent<AOEController>().Source = this;

            // Heal
            //ultimate.GetComponentInChildren<HitController>().Source = this;
            ultimate.transform.GetChild(0).GetComponent<HitController>().Hit(Holder);
            
            // Start VFX
            vfxController.StartVFX();
        }



        public void SpawnVFX()
        {
            GameObject go = Instantiate(slashVFXPrefab, transform);
            go.transform.localScale = vfxSize;
            Destroy(go, slashVFXPrefab.GetComponent<VisualEffect>().GetFloat("Lifetime"));
        }
        #endregion
    }
}