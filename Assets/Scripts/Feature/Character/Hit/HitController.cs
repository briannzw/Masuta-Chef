using System.Collections.Generic;
using UnityEngine;

namespace Character.Hit
{
    using MyBox;
    using StatEffect;
    using Weapon;

    public class HitController : MonoBehaviour, IApplyEffect
    {
        [Header("Weapon")]
        public Weapon Source;
        // Change to ReadOnly on Future Versions
        [ConditionalField(nameof(Type), true, HitType.EffectOnly)] public HitValue Value;

        [Header("Hit Type")]
        public HitType Type;

        [Header("Apply Effects")]
        [ConditionalField(nameof(Type), true, HitType.EffectOnly)] public bool ApplyEffectBeforeHit;
        [field:SerializeField] public List<Effect> Effects { get; set; }

        protected virtual void Awake()
        {
            foreach(Effect effect in Effects)
            {
                effect.Initialize();
            }
        }

        public void Initialize(Weapon source, float scaling)
        {
            Source = source;
            Value.WeaponAttack = Source.stats[WeaponStatsEnum.Power].Value;
            if(source.Holder == null)
            {
                Debug.LogWarning("There is no Holder detected in Weapon: " + source.name + " this HitController should be deleted or not instantiated!");
                return;
            }
            Value.CharacterAttack = Source.Holder.Stats.StatList[StatsEnum.Attack].Value;
            Value.Multiplier = Source.Holder.Stats.StatList[StatsEnum.DamageMultiplier].Value * scaling;
        }

        public void Hit(Character character)
        {
            // Check which object tag to be affected
            if (Source != null)
            {
                if (Source.Holder == null || Source.TargetTags.Count == 0) return;

                if (!Source.TargetTags.Contains(character.tag)) return;
            }

            if (ApplyEffectBeforeHit) ApplyEffect(character);

            if (Type == HitType.Damage) character.TakeDamage(Value.CharacterAttack + Value.WeaponAttack, DynamicStatsEnum.Health, Value.Multiplier);
            // Changeable
            if (Type == HitType.Heal) character.TakeHeal(Value.CharacterAttack + Value.WeaponAttack, DynamicStatsEnum.Health, Value.Multiplier);

            if(!ApplyEffectBeforeHit) ApplyEffect(character);
        }

        public void ApplyEffect(Character character)
        {
            foreach(Effect effect in Effects)
            {
                character.AddEffect(effect);
            }
        }
    }
}