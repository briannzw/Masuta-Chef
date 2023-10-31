using System;
using UnityEngine;

namespace Character.Stat
{
    using Kryz.CharacterStats;
    using static UnityEngine.Rendering.DebugUI;

    [Serializable]
    public class CharacterDynamicStat : CharacterStat
    {
        public float CurrentValue
        {
            get { return Mathf.Clamp(_currentValue, 0, Value); }
            private set { _currentValue = value; }
        }

        protected float _currentValue;

        public CharacterDynamicStat()
        {
            _currentValue = BaseValue;
        }

        public CharacterDynamicStat(float baseValue) : this()
        {
            BaseValue = baseValue;
            _currentValue = BaseValue;
        }

        public void ChangeCurrentValue(StatModifier mod)
        {
            float finalValue = CurrentValue;

            if (mod.Type == StatModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModType.PercentAdd ||mod.Type == StatModType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }

            CurrentValue = (float)Math.Round(finalValue, 4);
        }
    }
}