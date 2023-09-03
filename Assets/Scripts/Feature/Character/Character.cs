using UnityEngine;

namespace Character
{
    using Kryz.CharacterStats;
    using Stats;

    public class Character : MonoBehaviour
    {
        [Header("Stats")]
        public CharacterDynamicStat Health;
        public CharacterDynamicStat Mana;
        public CharacterStat Attack;
        public CharacterStat Defense;
        public CharacterStat DamageMultiplier;

        private void Start()
        {
            // Core Stats
            Health.BaseValue = 100;
            Health.CurrentValue = Health.BaseValue;
            Mana.BaseValue = 100;
            Mana.CurrentValue = Mana.BaseValue;
            DamageMultiplier.BaseValue = 1;
        }

        public void TakeDamage(Character other, float scaling = 1)
        {
            // Total Damage Received = (Other's Total Attack * Scaling) - Defense * Final Damage Multiplier
            Health.CurrentValue -= (other.Attack.Value * scaling - Defense.Value) * other.DamageMultiplier.Value;
        }
    }
}