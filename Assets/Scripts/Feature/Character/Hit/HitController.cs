using System.Collections.Generic;
using UnityEngine;

namespace Character.Hit
{
    using MyBox;
    using StatEffect;
    using Weapon;

    public class HitController : MonoBehaviour, IApplyEffect
    {
        public Weapon Source;
        [ConditionalField(nameof(Type), true, HitType.EffectOnly)] public HitValue Value;

        public HitType Type;

        [ConditionalField(nameof(Type), true, HitType.EffectOnly)] public bool ApplyEffectFirst;
        [field:SerializeField] public List<Effect> Effects { get; set; }

        protected virtual void Start()
        {
            foreach(Effect effect in Effects)
            {
                effect.Initialize();
            }
        }

        public void Initialize(Weapon source)
        {
            Source = source;
            Value.WeaponAttack = Source.stats[Weapon.WeaponStatsEnum.Power].Value;
            if(source.Holder == null)
            {
                Debug.LogWarning("There is no Holder detected in Weapon: " + source.name + " this HitController should be deleted or not instantiated!");
                return;
            }
            Value.CharacterAttack = Source.Holder.Stats.StatList[StatsEnum.Attack].Value;
            Value.Multiplier = Source.Holder.Stats.StatList[StatsEnum.DamageMultiplier].Value;
        }

        public void Hit(Character character)
        {
            if (!character.CompareTag(Source.TargetTag)) return;

            if(ApplyEffectFirst) ApplyEffect(character);

            if (Type == HitType.Damage) character.TakeDamage(Value.CharacterAttack + Value.WeaponAttack, StatsEnum.Health, Value.Multiplier);
            // Changeable
            if (Type == HitType.Heal) character.TakeHeal(Value.CharacterAttack + Value.WeaponAttack, StatsEnum.Health, Value.Multiplier);

            if(!ApplyEffectFirst) ApplyEffect(character);
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