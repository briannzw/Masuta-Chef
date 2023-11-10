using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.StatEffect
{
    using Kryz.CharacterStats;
    using MyBox;
    using Weapon;

    public enum EffectBehaviour { Instant, Duration }
    public enum StatsType { Character, Weapon }

    [System.Serializable]
    public class Effect
    {
        [Header("Type")]
        public StatsType StatType;

        [Header("Values")]
#if UNITY_EDITOR
        [ConditionalField(nameof(StatType), false, StatsType.Character)]
#endif
        public bool AffectDynamicStat = false;

#if UNITY_EDITOR
        [ConditionalField(true, nameof(IsNotDynamicStat))]
#endif
        public StatsEnum StatsAffected;

#if UNITY_EDITOR
        [ConditionalField(true, nameof(IsDynamicStat))]
#endif
        public DynamicStatsEnum DynamicStatsAffected;

#if UNITY_EDITOR
        [ConditionalField(nameof(StatType), false, StatsType.Weapon)]
#endif
        public Weapon.WeaponStatsEnum WeaponStatsAffected;

        [SerializeField] protected float StatValue;
        [SerializeField] protected StatModType StatModifierType;
        public StatModifier Modifier;

#if UNITY_EDITOR
        private bool IsDynamicStat()
        {
            if (StatType != StatsType.Character) return false;
            if (StatusEffect == StatusEffects.Heal || StatusEffect == StatusEffects.Burn) AffectDynamicStat = true;
            return AffectDynamicStat;
        }
        private bool IsNotDynamicStat()
        {
            if (StatType != StatsType.Character) return false;
            return !IsDynamicStat();
        }
#endif

        [Header("Advanced")]
        [SerializeField] protected bool overwriteOrder;
        [SerializeField, ConditionalField(nameof(overwriteOrder))] protected int priorityOrder;

        [Header("Behaviour")]
        public EffectBehaviour Behaviour;
        public StatusEffects StatusEffect;

        [Space]
        [ReadOnly, ConditionalField(nameof(Behaviour), false, EffectBehaviour.Duration)] public float Timer = 0f;
        [ConditionalField(nameof(Behaviour), false, EffectBehaviour.Duration)] public float Duration;
        [ConditionalField(nameof(Behaviour), false, EffectBehaviour.Duration)] public bool UseInterval;
        [ConditionalField(new []{ nameof(Behaviour), nameof(UseInterval) }, new[] { false, false }, EffectBehaviour.Duration)][ReadOnly] public float IntervalTimer = 0f;
        [ConditionalField(new []{ nameof(Behaviour), nameof(UseInterval) }, new[] { false, false }, EffectBehaviour.Duration)] public float Interval;

        public bool DurationEnds => Timer >= Duration;

        public void Initialize(object source = null)
        {
             if(overwriteOrder) Modifier = new StatModifier(((StatusEffect == StatusEffects.Debuff) ? -1 : 1) * StatValue, StatModifierType, priorityOrder, source == null ? this : source);
             else Modifier = new StatModifier(((StatusEffect == StatusEffects.Debuff) ? -1 : 1) * StatValue, StatModifierType, source == null ? this : source);
        }
    }
}