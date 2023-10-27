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

        [Header("Weapon")]
        //public Weapon Weapon;
        //public Dicitonary<IAbilityWeapon, float> WeaponAbilityCooldowns;

        [Header("Effect")]
        protected List<Effect> CurrentStatusEffects;
        protected Dictionary<Effect, Coroutine> StatusEffectCoroutines;

        public Action OnDie;

        public Character(StatsPreset preset)
        {
            Stats = new Stats(preset.Stats);
        }

        private void Awake()
        {
            if (StatsPreset != null) Stats = new Stats(StatsPreset.Stats);
            CurrentStatusEffects = new List<Effect>();
            StatusEffectCoroutines = new Dictionary<Effect, Coroutine>();
        }

        public virtual void TakeDamage(float damageAmount, DynamicStatsEnum dynamicEnum, float multiplier = 1, StatModType modType = StatModType.Flat)
        {
            CharacterDynamicStat Stat = Stats.DynamicStatList[dynamicEnum];

            // Total Damage Received = (Base Attack + Weapon Attack - Defense) * Final Damage Multiplier
            StatModifier statMod;
            if (modType == StatModType.Flat)
                statMod = new StatModifier(-(damageAmount - (dynamicEnum == DynamicStatsEnum.Health ? Stats.StatList[StatsEnum.Defense].Value : 0)) * multiplier, modType);
            else
                statMod = new StatModifier(-damageAmount * multiplier, modType);
                

            if (statMod.Value > 0) statMod.Value = 0;

            Stat.ChangeCurrentValue(statMod);

            if (Stat.CurrentValue <= 0 && dynamicEnum == DynamicStatsEnum.Health) OnDie?.Invoke();
        }

        public void TakeHeal(float healAmount, DynamicStatsEnum dynamicEnum, float multiplier = 1, StatModType modType = StatModType.Flat)
        {
            CharacterDynamicStat Stat = Stats.DynamicStatList[dynamicEnum];

            StatModifier statMod = new StatModifier(healAmount * multiplier, modType);
            if(statMod.Value < 0) statMod.Value = 0;

            Stat.ChangeCurrentValue(statMod);
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
            else if (effect.AffectDynamicStat)
                Stats.DynamicStatList[effect.DynamicStatsAffected].AddModifier(effect.Modifier);
            else
                Stats.StatList[effect.StatsAffected].AddModifier(effect.Modifier);
        }
    }
}