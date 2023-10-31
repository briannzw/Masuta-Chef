using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Module.UI
{
    using Character;

    public class FloatingHealthBar : MonoBehaviour
    {
        private Character chara;
        private Slider slider;
        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            chara = GetComponentInParent<Character>();
        }

        private void Start()
        {
            slider.maxValue = chara.Stats.DynamicStatList[DynamicStatsEnum.Health].Value;
        }
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
        private void Update()
        {
            slider.value = chara.Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue;
        }
    }
}

