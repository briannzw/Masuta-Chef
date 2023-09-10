using System;
using UnityEngine;

namespace Character
{
    using Kryz.CharacterStats;
    using Stats;

    public class Character : MonoBehaviour
    {
        [Header("Stats")]
        public CharacterDynamicStat Health = new CharacterDynamicStat(100);
        public CharacterDynamicStat Mana = new CharacterDynamicStat(100);
        public CharacterStat Attack;
        public CharacterStat Defense;
        public CharacterStat DamageMultiplier = new CharacterStat(1);

        #region Action
        public Action OnDie;
        #endregion

        public void TakeDamage(Character other, float scaling = 1)
        {
            // Total Damage Received = (Other's Total Attack * Scaling) - Defense * Final Damage Multiplier
            Health.CurrentValue = (float)System.Math.Round(Health.CurrentValue - (other.Attack.Value * scaling - Defense.Value) * other.DamageMultiplier.Value, 4);

            if(Health.CurrentValue <= 0f)
            {
                Debug.Log(name + " Died");
                // Make sure to Only call OnDie once
                OnDie?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}