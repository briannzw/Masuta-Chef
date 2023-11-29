using System;
using System.Collections.Generic;
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

    public enum WeaponStatsEnum
    {
        Power,
        Speed,
        Accuracy
    }

    public class Weapon : MonoBehaviour, IInteractable
    {
        #region Properties
        [Header("References")]
        public Character Holder;
        [SerializeField] protected Rigidbody rb;
        public Collider weaponCollider;
        [Tag] public List<string> TargetTags = new();

        [Header("Stats")]
        public SerializedDictionary<WeaponStatsEnum, CharacterStat> stats;
        public float damageScaling = 1f;
        
        protected bool isFiring;
        protected float attackTimer;

        [Header("Ultimate Properties")]
        [SerializeField] protected float UltimateTimer = 1;
        [SerializeField] protected bool isCooldownUltimate = false;
        [SerializeField] private bool isUltimateCancelable = false;

        private bool initialTrigger;

        private Dictionary<WeaponStatsEnum, float> previousFlatModValue = new();
        #endregion

        #region C# Events
        // Currently for Weapon SFXs
        public Action OnStartAttack;
        public Action OnAttack;
        public Action OnStopAttack;
        public Action OnUltimateAttack;
        #endregion

        #region Lifecycle
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if(rb == null) rb = GetComponentInParent<Rigidbody>();
            weaponCollider = GetComponent<Collider>();
            initialTrigger = weaponCollider.isTrigger;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            attackTimer = 0;

            // If Spawned
            if (Holder == null)
            {
                rb.isKinematic = false;
                weaponCollider.isTrigger = false;
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
            OnAttack?.Invoke();
        }

        public virtual void StartAttack()
        {
            isFiring = true;

            OnStartAttack?.Invoke();
        }

        public virtual void StopAttack()
        {
            isFiring = false;

            OnStopAttack?.Invoke();
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
            
            // SFX
            OnUltimateAttack?.Invoke();
        }

        public void OnEquip(Character holder)
        {
            if (Holder != null) return;

            Holder = holder;
            rb.isKinematic = true;
            weaponCollider.isTrigger = initialTrigger;
            gameObject.layer = LayerMask.NameToLayer("Default");

            // Add Mods based on Holder tag
            FetchModFromHolder();
        }

        public void OnUnequip()
        {
            // Remove Mods from previous Holder
            RemovePreviousHolderMod();

            Holder = null;
            GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
            weaponCollider.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            StopAttack();
        }

        public void Interact(GameObject other = null)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<PlayerWeaponController>().Equip(this);
            OnEquip(other.GetComponent<Character>());
        }

        private void FetchModFromHolder()
        {
            if (Holder == null) return;

            // Fetch Stat Mods from Recipe Book
            if (GameManager.Instance.StatsManager != null)
            {
                if (GameManager.Instance.StatsManager.WeaponStatMods.ContainsKey(Holder.tag))
                {
                    foreach (var modList in GameManager.Instance.StatsManager.WeaponStatMods[Holder.tag])
                    {
                        foreach (var mod in modList.Value)
                        {
                            // Add Flat Mod to Base Value
                            if (mod.Type == StatModType.Flat)
                            {
                                stats[modList.Key].BaseValue += mod.Value;

                                if (!previousFlatModValue.ContainsKey(modList.Key))
                                    previousFlatModValue.Add(modList.Key, 0);

                                previousFlatModValue[modList.Key] += mod.Value;
                            }
                            // Add Percent Mod to Total Value
                            else
                            {
                                // Change to percent
                                mod.Value /= 100;
                                stats[modList.Key].AddModifier(mod);
                            }
                        }
                    }
                }
            }
        }

        private void RemovePreviousHolderMod()
        {
            // Remove Flat Mods that already applied
            foreach(var flatMod in previousFlatModValue)
            {
                stats[flatMod.Key].BaseValue -= flatMod.Value;
            }
            // Reset Flat Mods dictionary
            previousFlatModValue.Clear();

            // Remove Percentage Mods from source
            if (GameManager.Instance.StatsManager != null)
            {
                foreach (var stat in stats)
                    stat.Value.RemoveAllModifiersFromSource(GameManager.Instance.StatsManager);
            }
            else
                Debug.LogWarning("No StatsManager detected in GameManager!");
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
