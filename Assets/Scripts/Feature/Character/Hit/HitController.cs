using UnityEngine;

namespace Character.Hit
{
    using MyBox;
    using StatEffect;
    using System.Collections.Generic;

    public class HitController : MonoBehaviour, IApplyEffect
    {
        //public Weapon Source;
        [ConditionalField(nameof(Type), true, HitType.EffectOnly)] public HitValue Value;
        [Tag]
        public string TargetTag;

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

        public void Hit(Character character)
        {
            if (!character.CompareTag(TargetTag)) return;

            if(ApplyEffectFirst) ApplyEffect(character);

            if (Type == HitType.Damage) character.TakeDamage(Value.CharacterAttack + Value.WeaponAttack, Value.Multiplier);
            // Changeable
            if (Type == HitType.Heal) character.TakeHeal(Value.CharacterAttack + Value.WeaponAttack, Value.Multiplier);

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