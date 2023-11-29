using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    using NPC.Companion;
    using Character;
    public class CompanionHUDItem : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image hpBar;
        [SerializeField] private Image companionIcon;
        [SerializeField] private TMP_Text companionLevelText;

        private Character companionChara;
        private CompanionLevelSystem companionLevel;

        private void Awake()
        {
            Reset();
        }

        public void Reset()
        {
            hpBar.fillAmount = 1f;
            companionIcon.sprite = null;
            companionLevelText.text = "";
        }

        public void SubToCompanion(Companion companion)
        {
            companionChara = companion.GetComponent<Character>();
            companionLevel = companion.GetComponent<CompanionLevelSystem>();

            companionIcon.sprite = companion.data.icon;
            SetHealthUI();
            SetLevelUI();

            companionLevel.OnLevelUp += SetLevelUI;
            companionChara.OnDamaged += SetHealthUI;
            companionChara.OnHealed += SetHealthUI;
            companionChara.OnDie += UnsubToCompanion;
        }

        private void UnsubToCompanion()
        {
            companionChara.OnDamaged -= SetHealthUI;
            companionChara.OnHealed -= SetHealthUI;
            companionChara.OnDie -= UnsubToCompanion;
            companionChara = null;

            companionLevel.OnLevelUp -= SetLevelUI;
            companionLevel = null;

            Reset();
        }

        private void SetLevelUI()
        {
            companionLevelText.text = $"Lv. {companionLevel.CurrentLevel}";
        }

        private void SetHealthUI()
        {
            hpBar.fillAmount = companionChara.Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue / companionChara.Stats.DynamicStatList[DynamicStatsEnum.Health].Value;
        }
    }
}