using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.StatEffect
{
    using Kryz.CharacterStats;
    using MyBox;

    public enum EffectBehaviour { Instant, Duration }
    public class EffectTimer
    {
        public float Timer;
        public float IntervalTimer;
    }

    [System.Serializable]
    public class Effect
    {
        [Header("Values")]
        public StatsEnum StatsAffected;
        //public WeaponStatsEnum WeaponStatsAffected;
        [SerializeField] protected float StatValue;
        [SerializeField] protected StatModType StatModifierType;
        public StatModifier Modifier;

        [Header("Advanced")]
        [SerializeField] protected bool overwriteOrder;
        [SerializeField, ConditionalField(nameof(overwriteOrder))] protected int priorityOrder;

        [Header("Type")]
        public EffectBehaviour Behaviour;
        public StatusEffects StatusEffect;

        [Space]
        [ConditionalField(nameof(Behaviour), false, EffectBehaviour.Duration)] public float Duration;
        [ConditionalField(nameof(Behaviour), false, EffectBehaviour.Duration)] public bool UseInterval;
        [ConditionalField(new []{ nameof(Behaviour), nameof(UseInterval) }, new[] { false, false }, EffectBehaviour.Duration)] public float Interval;

        public void Initialize()
        {
             if(overwriteOrder) Modifier = new StatModifier(((StatusEffect == StatusEffects.Debuff) ? -1 : 1) * StatValue, StatModifierType, priorityOrder, this);
             else Modifier = new StatModifier(((StatusEffect == StatusEffects.Debuff) ? -1 : 1) * StatValue, StatModifierType, this);
        }
    }
}