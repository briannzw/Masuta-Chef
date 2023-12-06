using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    using Kryz.CharacterStats;
    using MyBox;
    using Stat;
    using StatEffect;

    public class Character : MonoBehaviour
    {
        [Header("Stats")]
        public StatsPreset StatsPreset;
        [ConditionalField(nameof(StatsPreset), inverse:true)] public Stats Stats;

        [Header("Effect")]
        protected List<Effect> CurrentStatusEffects;
        protected Dictionary<Effect, Coroutine> StatusEffectCoroutines;

#if UNITY_EDITOR
        // EDITOR ONLY
        [ReadOnly, SerializeField] protected string HealthDisplay = "";
#endif

        #region C# Events
        public Action OnStatsInitialized;
        public Action OnDie;
        public Action OnDamaged;
        public Action OnHealed;
        public Action OnSpeedChanged;
        #endregion

        public bool isInvincible = false;
        public Character(StatsPreset preset)
        {
            Stats = new Stats(preset.Stats);
        }

        public void Reset()
        {
            InitializeStats();
            FetchStatMods();

            if (CompareTag("Enemy"))
            {
                GetComponent<NPC.Enemy.Enemy>().IsStun = false;
                GetComponent<NPC.Enemy.Enemy>().IsTaunted = false;
                GetComponent<NPC.Enemy.Enemy>().IsConfused = false;
            }
        }

        private void OnEnable()
        {
            InitializeStats();

#if UNITY_EDITOR
            // EDITOR ONLY
            HealthDisplay = Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue + " / " + Stats.DynamicStatList[DynamicStatsEnum.Health].Value;
            OnDamaged += () => HealthDisplay = Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue + " / " + Stats.DynamicStatList[DynamicStatsEnum.Health].Value;
            OnHealed += () => HealthDisplay = Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue + " / " + Stats.DynamicStatList[DynamicStatsEnum.Health].Value;
#endif
        }

        private void OnDisable()
        {

#if UNITY_EDITOR
            // EDITOR ONLY
            OnDamaged -= () => HealthDisplay = Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue + " / " + Stats.DynamicStatList[DynamicStatsEnum.Health].Value;
            OnHealed -= () => HealthDisplay = Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue + " / " + Stats.DynamicStatList[DynamicStatsEnum.Health].Value;
#endif
        }

        public void InitializeStats()
        {
            if (StatsPreset != null) Stats = new Stats(StatsPreset.Stats);
            CurrentStatusEffects = new List<Effect>();
            StatusEffectCoroutines = new Dictionary<Effect, Coroutine>();

            isInvincible = false;
        }

        protected virtual void Start()
        {
            FetchStatMods();
        }

        public void FetchStatMods()
        {
            // Fetch Stat Mods from Recipe Book
            if (GameManager.Instance.StatsManager != null)
            {
                if (GameManager.Instance.StatsManager.CharacterStatMods.ContainsKey(tag))
                {
                    foreach (var modList in GameManager.Instance.StatsManager.CharacterStatMods[tag])
                    {
                        foreach (var mod in modList.Value)
                        {
                            // Add Flat Mod to Base Value
                            if (mod.Type == StatModType.Flat) Stats.StatList[modList.Key].BaseValue += mod.Value;
                            // Add Percent Mod to Total Value
                            else
                            {
                                // Change to percent
                                mod.Value /= 100;
                                Stats.StatList[modList.Key].AddModifier(mod);
                            }

                            // Events
                            if (modList.Key == StatsEnum.Speed) OnSpeedChanged?.Invoke();
                        }
                    }
                }

                if (GameManager.Instance.StatsManager.CharacterDynamicStatMods.ContainsKey(tag))
                {
                    foreach (var modList in GameManager.Instance.StatsManager.CharacterDynamicStatMods[tag])
                    {
                        foreach (var mod in modList.Value)
                        {
                            // Add Flat Mod to Base Value
                            if (mod.Type == StatModType.Flat) Stats.DynamicStatList[modList.Key].BaseValue += mod.Value;
                            // Add Percent Mod to Total Value
                            else
                            {
                                // Change to percent
                                mod.Value /= 100;
                                Stats.DynamicStatList[modList.Key].AddModifier(mod);
                            }

                            // Reset Current Value
                            Stats.DynamicStatList[modList.Key].ResetCurrentValue();
                        }
                    }
                }
            }

            OnStatsInitialized?.Invoke();
        }

        public virtual void TakeDamage(float damageAmount, DynamicStatsEnum dynamicEnum, float multiplier = 1, StatModType modType = StatModType.Flat)
        {
            if (isInvincible)
            {
                return;
            }
                
            CharacterDynamicStat Stat = Stats.DynamicStatList[dynamicEnum];

            // Total Damage Received = (Base Attack + Weapon Attack - Defense) * Final Damage Multiplier
            StatModifier statMod;
            if (modType == StatModType.Flat)
                statMod = new StatModifier(-(damageAmount - (dynamicEnum == DynamicStatsEnum.Health ? Stats.StatList[StatsEnum.Defense].Value : 0)) * multiplier, modType);
            else
                statMod = new StatModifier(-damageAmount * multiplier, modType);
                

            if (statMod.Value > 0) statMod.Value = 0;

            Stat.ChangeCurrentValue(statMod);
            OnDamaged?.Invoke();

            if (Stat.CurrentValue <= 0 && dynamicEnum == DynamicStatsEnum.Health)
            {
                OnDie?.Invoke();

                // Send Die data to Level Manager
                GameManager.Instance.LevelManager.CharacterDied(this);
                // Make sure only called once
                isInvincible = true;
            }
        }

        public void TakeHeal(float healAmount, DynamicStatsEnum dynamicEnum, float multiplier = 1, StatModType modType = StatModType.Flat)
        {
            CharacterDynamicStat Stat = Stats.DynamicStatList[dynamicEnum];

            StatModifier statMod = new StatModifier(healAmount * multiplier, modType);
            if(statMod.Value < 0) statMod.Value = 0;

            Stat.ChangeCurrentValue(statMod);
            OnHealed?.Invoke();
        }

        public void AddEffect(Effect effect)
        {
            // Save References to all modifiers except changes for dynamic currentValue
            if(effect.StatusEffect == StatusEffects.Buff || effect.StatusEffect == StatusEffects.Debuff)
                CurrentStatusEffects.Add(effect);

            if(effect.Behaviour == EffectBehaviour.Duration)
            {
                if (effect.DurationEnds) return;
                
                StatusEffectCoroutines.Add(effect, StartCoroutine(ApplyEffect(effect)));
                return;
            }
            TakeEffect(effect);
        }

        public void PauseEffect(Effect effect)
        {
            if (effect.Behaviour != EffectBehaviour.Duration) return;
            if (StatusEffectCoroutines.ContainsKey(effect))
            {
                StopCoroutine(StatusEffectCoroutines[effect]);
                StatusEffectCoroutines.Remove(effect);
            }
        }

        // Remove All Modifiers this Effect has done.
        public void RemoveEffect(Effect effect)
        {
            if (CurrentStatusEffects.Remove(effect))
            {
                if (effect.Behaviour == EffectBehaviour.Duration) PauseEffect(effect);
                Stats.StatList[effect.StatsAffected].RemoveAllModifiersFromSource(effect);
                return;
            }
            Debug.LogWarning("Effect not found on Character: " + name);
        }

        // Ex-case: Remove All Debuffs, All Interval Healings, etc.
        // Problem: How to restore Armor Debuffs?
        public void RemoveAllEffect(StatusEffects statEffect)
        {
            foreach(Effect effect in CurrentStatusEffects.ToArray())
            {
                if(effect.StatusEffect == statEffect)
                {
                    if (effect.Behaviour == EffectBehaviour.Duration)
                    {
                        PauseEffect(effect);
                    }
                    RemoveEffect(effect);
                }
            }
        }   

        private IEnumerator ApplyEffect(Effect effect)
        {
            // Apply on time 0
            TakeEffect(effect);

            // While there's still duration left
            while (!effect.DurationEnds)
            {
                effect.Timer += Time.deltaTime;
                if (!effect.UseInterval)
                {
                    yield return null;
                    continue;
                }

                effect.IntervalTimer += Time.deltaTime;
                if (effect.IntervalTimer > effect.Interval)
                {
                    effect.IntervalTimer = 0f;
                    TakeEffect(effect);
                }
                yield return null;
            }

            // If duration ends
            RemoveEffect(effect);
        }

        private void TakeEffect(Effect effect)
        {
            if (effect.Modifier == null) Debug.LogError("Effect must be Initialized first to create Modifier!");
            if (effect.StatusEffect == StatusEffects.Burn)
            {
                // If Flat -> Reduce directly, else reduce by value * Dynamic Max Value
                TakeDamage(effect.Modifier.Value / (effect.Modifier.Type == StatModType.Flat ? 1 : 100), effect.DynamicStatsAffected, 1, effect.Modifier.Type);
            }
            else if (effect.StatusEffect == StatusEffects.Heal)
            {
                // If Flat -> Adds directly, else adds value * Dynamic Max Value
                TakeHeal(effect.Modifier.Value / (effect.Modifier.Type == StatModType.Flat ? 1 : 100), effect.DynamicStatsAffected, 1, effect.Modifier.Type);
            }
            else if(effect.StatusEffect == StatusEffects.Stun || effect.StatusEffect == StatusEffects.Taunt || effect.StatusEffect == StatusEffects.Confuse)
            {
                // Enemy Only
                Debug.Log("test");
                if (!CompareTag("Enemy")) return;
                StartCoroutine(TakeEnemy(effect));
            }
            else if (effect.AffectDynamicStat)
                Stats.DynamicStatList[effect.DynamicStatsAffected].AddModifier(effect.Modifier);
            else
                Stats.StatList[effect.StatsAffected].AddModifier(effect.Modifier);

            // Events
            if(effect.StatsAffected == StatsEnum.Speed) OnSpeedChanged?.Invoke();
        }

        private IEnumerator TakeEnemy(Effect effect)
        {
            switch (effect.StatusEffect)
            {
                case StatusEffects.Taunt:
                    GetComponent<NPC.Enemy.Enemy>().IsTaunted = true;
                    break;
                case StatusEffects.Stun:
                    GetComponent<NPC.Enemy.Enemy>().IsStun = true;
                    break;
                case StatusEffects.Confuse:
                    GetComponent<NPC.Enemy.Enemy>().IsConfused = true;
                    break;
            }

            yield return new WaitForSeconds(effect.Modifier.Value);

            switch (effect.StatusEffect)
            {
                case StatusEffects.Taunt:
                    GetComponent<NPC.Enemy.Enemy>().IsTaunted = false;
                    break;
                case StatusEffects.Stun:
                    GetComponent<NPC.Enemy.Enemy>().IsStun = false;
                    break;
                case StatusEffects.Confuse:
                    GetComponent<NPC.Enemy.Enemy>().IsConfused = false;
                    break;
            }
        }
    }
}