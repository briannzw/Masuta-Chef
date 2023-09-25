using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.StatEffect
{
    using Kryz.CharacterStats;
    public enum EffectBehaviour { Instant, Duration }

    public class Effect
    {
        [Header("Values")]
        public StatsEnum StatsAffected;
        //public WeaponStatsEnum WeaponStatsAffected;
        public StatModifier Modifier;

        [Header("Type")]
        public EffectBehaviour Behaviour;
        public StatusEffects StatusEffect;

        [Header("Time")]
        public float Duration;
        public float Interval;
    }
}