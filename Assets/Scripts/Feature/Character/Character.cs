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
            Stats = preset.Stats;
        }

        private void Awake()
        {
            if(StatsPreset != null) Stats = StatsPreset.Stats;
            CurrentStatusEffects = new List<Effect>();
            StatusEffectCoroutines = new Dictionary<Effect, Coroutine>();
        }

        public virtual void TakeDamage(float totalAttack = 0, float multiplier = 1, StatModifier statMod = null)
        {
            CharacterDynamicStat healthStat = Stats.StatList[StatsEnum.Health] as CharacterDynamicStat;

            // Total Damage Received = (Base Attack + Weapon Attack - Defense) * Final Damage Multiplier
            if (statMod == null)
                statMod = new StatModifier(-(totalAttack - Stats.StatList[StatsEnum.Defense].Value) * multiplier, StatModType.Flat);
            
            if (statMod.Value > 0) statMod.Value = 0;
            
            healthStat.ChangeCurrentValue(statMod);
            Debug.Log(healthStat.CurrentValue);

            if (healthStat.CurrentValue <= 0) OnDie?.Invoke();
        }

        public void TakeHeal(float healAmount = 0, float multiplier = 1, StatModifier statMod = null)
        {
            CharacterDynamicStat healthStat = Stats.StatList[StatsEnum.Health] as CharacterDynamicStat;

            if (statMod == null)
                statMod = new StatModifier(healAmount * multiplier, StatModType.Flat);

            if(statMod.Value < 0) statMod.Value = 0;

            healthStat.ChangeCurrentValue(statMod);
        }

        public void AddEffect(Effect effect)
        {
            CurrentStatusEffects.Add(effect);
            if(effect.Behaviour == EffectBehaviour.Duration)
            {
                if (effect.DurationEnds) return;
                
                StatusEffectCoroutines.Add(effect, StartCoroutine(ApplyEffect(effect)));
                return;
            }
            Stats.StatList[effect.StatsAffected].AddModifier(effect.Modifier);
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
            Debug.LogError("Effect not found on Character: " + name);
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
            if(!effect.UseInterval) TakeEffect(effect);

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
            if (effect.StatusEffect == StatusEffects.Burn) TakeDamage(0, 1, effect.Modifier);
            else if (effect.StatusEffect == StatusEffects.Heal) TakeHeal(0, 1, effect.Modifier);
            else
                Stats.StatList[effect.StatsAffected].AddModifier(effect.Modifier);
        }
    }
}