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

        // ApplyOnStay
        [SerializeField] private List<Character> characterInArea = new List<Character>();
        // Instant, Hit is only applied once to every Chara.
        private HashSet<Character> characterAffected = new HashSet<Character>();

        protected override void Start()
        {
            base.Start();
            StartCoroutine(RemoveAllEffect(AreaDuration));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TargetTag != null && other.CompareTag(TargetTag))
            {
                Character chara = other.GetComponent<Character>();
                if (chara == null) return;

                if (characterAffected.Contains(chara))
                {
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
            //else if(other.CompareTag(Source.TargetTag)){ characterInArea.Add(chara); }
        }

        private void OnTriggerExit(Collider other)
        {
            if (TargetTag != null && other.CompareTag(TargetTag))
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