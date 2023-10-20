using UnityEngine;

namespace Weapon
{
    using AYellowpaper.SerializedCollections;
    using Interaction;
    using Kryz.CharacterStats;
    using MyBox;
    using Character;
    using Player.Controller;
    using System.Collections;

    public class Weapon : MonoBehaviour, IInteractable
    {
        #region Properties
        [Header("References")]
        public Character Holder;
        private Rigidbody rb;
        private new Collider collider;
        [Tag] public string TargetTag;
        public SerializedDictionary<WeaponStatsEnum, CharacterStat> stats;
        
        protected bool isFiring;
        private float attackTimer;

        [Header("Ultimate Properties")]
        [SerializeField] protected float UltimateTimer = 1;
        [SerializeField] protected bool isCooldownUltimate = false;
        [SerializeField] private bool isUltimateCancelable = false;

        private bool initialTrigger;
        #endregion

        public enum WeaponStatsEnum 
        {
            Power,
            Speed,
            Accuraccy
        }

        #region Lifecycle
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if(rb == null) rb = GetComponentInParent<Rigidbody>();
            collider = GetComponent<Collider>();
            initialTrigger = collider.isTrigger;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            attackTimer = 0;

            // If Spawned
            if (Holder == null)
            {
                rb.isKinematic = false;
                collider.isTrigger = false;
            }
        }

        // Update is called once per frame
        protected void Update()
        {
            if (isFiring && attackTimer > stats[WeaponStatsEnum.Speed].Value / 100)
            {
                Attack();
                attackTimer = 0;
            }

            attackTimer += Time.deltaTime;
        }
        #endregion

        #region Method
        public virtual void Attack() 
        {
            Debug.Log("Attack Method of Weapon Not Implemented!");
        }

        public virtual void StartAttack()
        {
            isFiring = true;
        }

        public virtual void StopAttack()
        {
            isFiring = false;
        }

        public virtual void StartUltimateAttack()
        {
            if (isCooldownUltimate) 
            {
                if (isUltimateCancelable) StopUltimateAttack();
                return;
            }
            
            StartCoroutine(UltimateCooldown());
            UltimateAttack();
        }

        public virtual void StopUltimateAttack()
        {
            isCooldownUltimate = false;
            StopCoroutine(UltimateCooldown());
        }

        protected virtual void UltimateAttack()
        {
            // Implement the Ultimate Attack Logic Here;
        }

        public void OnEquip(Character holder)
        {
            if (Holder != null) return;

            Holder = holder;
            rb.isKinematic = true;
            collider.isTrigger = initialTrigger;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        public void OnUnequip()
        {
            Holder = null;
            rb.isKinematic = false;
            collider.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            StopAttack();
        }

        public void Interact(GameObject other = null)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<PlayerWeaponController>().Equip(this);
            OnEquip(other.GetComponent<Character>());
        }

        private IEnumerator UltimateCooldown()
        {
            isCooldownUltimate = true;
            yield return new WaitForSeconds(UltimateTimer);
            isCooldownUltimate = false;
        }
        #endregion
    }
}
