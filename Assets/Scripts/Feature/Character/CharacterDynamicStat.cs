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
            get { return _currentValue; }
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
            float finalValue = Mathf.Clamp(_currentValue, 0, Value);

            if (mod.Type == StatModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModType.PercentAdd || mod.Type == StatModType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }

            _currentValue = (float)Math.Round(finalValue, 4);

            CurrentValue = Mathf.Clamp(_currentValue, 0, Value);
        }
    }
}