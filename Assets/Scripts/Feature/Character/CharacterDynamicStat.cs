using System;
using UnityEngine;

namespace Character.Stat
{
    using Kryz.CharacterStats;

    [Serializable]
    public class CharacterDynamicStat : CharacterStat
    {
        public float CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = Mathf.Clamp(value, 0, Value);
            }
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
    }
}