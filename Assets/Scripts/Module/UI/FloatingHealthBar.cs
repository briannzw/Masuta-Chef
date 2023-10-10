using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Module.UI
{
    using Character;
    using Character.Stat;

    public class FloatingHealthBar : MonoBehaviour
    {
        private Character chara;
        private Slider slider;
        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            chara = GetComponentInParent<Character>();
            slider.maxValue = (chara.Stats.StatList[StatsEnum.Health] as CharacterDynamicStat).Value;
            slider.value = slider.maxValue;
        }
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
        private void Update()
        {
            Debug.Log((chara.Stats.StatList[StatsEnum.Health] as CharacterDynamicStat).CurrentValue);
            slider.value = (chara.Stats.StatList[StatsEnum.Health] as CharacterDynamicStat).CurrentValue;
        }
    }
}

