using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Hit
{
    using StatEffect;
    public class AOEController : HitController
    {
        public float AreaDuration;
        public AOEBehaviour Behaviour;
        [Header("Settings")]
        public bool ContinuousHitOnTrigger = false;

        // ApplyOnStay
        protected List<Character> characterInArea = new List<Character>();
        // Instant, Hit is only applied once to every Chara.
        protected HashSet<Character> characterAffected = new HashSet<Character>();

        protected override void Start()
        {
            base.Start();
            StartCoroutine(RemoveAllEffect(AreaDuration));
        }

        private void OnTriggerEnter(Collider other)
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

            if (characterAffected.Contains(chara))
            {
                if (ContinuousHitOnTrigger)
                {
                    if (Type == HitType.Damage) chara.TakeDamage(Value.CharacterAttack + Value.WeaponAttack, StatsEnum.Health, Value.Multiplier);
                    // Changeable
                    if (Type == HitType.Heal) chara.TakeHeal(Value.CharacterAttack + Value.WeaponAttack, StatsEnum.Health, Value.Multiplier);
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

            //else if(other.CompareTag(Source.TargetTag)){ characterInArea.Add(chara); }
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

        private void OnTriggerExit(Collider other)
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