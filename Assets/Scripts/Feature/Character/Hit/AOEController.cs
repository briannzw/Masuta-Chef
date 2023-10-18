using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Hit
{
    using StatEffect;

    // AOEController only register one Hit for every Character.
    // Preferred to used in area buffs, area debuffs, any that didn't need CONTINUOUS DAMAGES.
    // Refer to ContinuousAOEHit for Continuous Damages.

    // HOW TO USE : 
    // AOEController (Instant) with Effect (Instant) is best for INSTANT HEAL/BURN (ex. One-time Heal 10% Max health) [NON-REMOVEABLE]
    // AOEController (Instant) with Effect (Duration, UseInterval false) is best for APPLYING DURATIONAL BUFF/DEBUFF/HEAL/BURN (ex. Debuff 20 ATK for 10 sec) [NON-REMOVEABLE]
    // AOEController (Instant) with Effect (Duration, UseInterval true) is best for APPLYING INTERVAL BUFF/DEBUFF/HEAL/BURN (ex. Burn 10 HP per 3 sec for 10 sec) [NON-REMOVEABLE]

    // AOEController (ApplyOnStay) with Effect (Instant) is best for IN-AREA Instant BUFF/DEBUFF (ex. Buff 50 DEF for every character within the AOE area) [REMOVED when exiting area]
    // AOEController (ApplyOnStay) with Effect (Duration, UseInterval false) doesn't have any useful case. (not recommended to be used)
    // AOEController (ApplyOnStay) with Effect (Duration, UseInterval true) is best described as a burn that attached to the Character (entering area will run the interval timer)
    // (ex: Entering area BURN 10 HP, for every 3 seconds in the area with max duration of 10 sec) [Interval timer will not reset after exiting area]

    public class AOEController : HitController
    {
        [Header("AOE Settings")]
        public float AreaDuration = 5;

        [Header("AOE Behaviour")]
        public AOEBehaviour Behaviour;

        // ApplyOnStay
        [HideInInspector] protected List<Character> characterInArea = new List<Character>();
        // Instant, Hit is only applied once to every Chara.
        private HashSet<Character> characterAffected = new HashSet<Character>();

        protected override void Start()
        {
            base.Start();
            StartCoroutine(RemoveAllEffect(AreaDuration));
        }

        protected void OnTriggerEnter(Collider other)
        {
            // ONLY APPLIED FOR PLAYABLE BUILD
            if (Source != null)
            {
                if (Source.Holder == null || string.IsNullOrEmpty(Source.TargetTag)) return;

                if (!other.CompareTag(Source.TargetTag)) return;
            }
            else
            {
                if (string.IsNullOrEmpty(TargetTag)) return;
                if (!other.CompareTag(TargetTag)) return;
            }

            Character chara = other.GetComponent<Character>();
            if (chara == null) return;

            HitChara(chara);
        }

        protected void HitChara(Character chara, bool continuous = false)
        {
            if (characterAffected.Contains(chara))
            {
                // Hit
                if (continuous)
                {
                    if (Type == HitType.Damage) chara.TakeDamage(Value.CharacterAttack + Value.WeaponAttack, DynamicStatsEnum.Health, Value.Multiplier);
                    if (Type == HitType.Heal) chara.TakeHeal(Value.CharacterAttack + Value.WeaponAttack, DynamicStatsEnum.Health, Value.Multiplier);
                }

                if (Behaviour == AOEBehaviour.ApplyOnStay)
                {
                    characterInArea.Add(chara);
                    ApplyEffect(chara);
                }
                return;
            }

            // First Hit Register
            characterAffected.Add(chara);
            Hit(chara);
        }

        protected void OnTriggerExit(Collider other)
        {
            // ONLY APPLIED FOR PLAYABLE BUILD
            if (Source != null)
            {
                if (Source.Holder == null) return;
                if (other.CompareTag(Source.TargetTag))
                {
                    Character chara = other.GetComponent<Character>();
                    if (chara == null) return;

                    if (Behaviour == AOEBehaviour.ApplyOnStay)
                    {
                        characterInArea.Remove(chara);
                        PauseEffect(chara);
                    }
                }
            }
            else
            {
                if (TargetTag == null || TargetTag == string.Empty) return;
                if (other.CompareTag(TargetTag))
                {
                    Character chara = other.GetComponent<Character>();
                    if (chara == null) return;

                    if (Behaviour == AOEBehaviour.ApplyOnStay)
                    {
                        characterInArea.Remove(chara);
                        PauseEffect(chara);
                    }
                }
            }
        }

        // APPLYONSTAY Only
        private void PauseEffect(Character character)
        {
            if (Behaviour != AOEBehaviour.ApplyOnStay) return;

            foreach(Effect effect in Effects)
            {
                if (effect.Behaviour == EffectBehaviour.Instant) character.RemoveEffect(effect);
                else if (effect.Behaviour == EffectBehaviour.Duration) character.PauseEffect(effect);
            }
        }
        
        private IEnumerator RemoveAllEffect(float Timer)
        {
            yield return new WaitForSeconds(Timer);
            // Area of Effect is no longer applying
            if (Behaviour == AOEBehaviour.ApplyOnStay)
            {
                foreach (Effect effect in Effects)
                {
                    foreach (Character chara in characterInArea)
                    {
                        chara.RemoveEffect(effect);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}