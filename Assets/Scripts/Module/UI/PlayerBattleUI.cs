using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Module.UI
{
    using Character;
    using Character.Stat;

    public class PlayerBattleUI : MonoBehaviour
    {
        [SerializeField] private Character chara;
        [SerializeField] private Slider slider;

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
