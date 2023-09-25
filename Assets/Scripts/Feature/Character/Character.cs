using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
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
        }
    }
}