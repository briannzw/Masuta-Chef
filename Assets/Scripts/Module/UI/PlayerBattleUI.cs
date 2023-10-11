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
        private Character chara;
        private Slider slider;
        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            chara = GetComponentInParent<Character>();
            slider.maxValue = (chara.Stats.StatList[StatsEnum.Health] as CharacterDynamicStat).Value;
        }
        private void Update()
        {
            slider.value = (chara.Stats.StatList[StatsEnum.Health] as CharacterDynamicStat).CurrentValue;
        }
    }
}
