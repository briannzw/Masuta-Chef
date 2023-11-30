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
            companionIcon.enabled = false;
            companionLevelText.text = "Lv. 0";
        }

        public void SubToCompanion(Companion companion)
        {
            companionChara = companion.GetComponent<Character>();
            companionLevel = companion.GetComponent<CompanionLevelSystem>();

            companionIcon.sprite = companion.data.icon;
            companionIcon.enabled = true;
            SetHealthUI();
            SetLevelUI();

            companionLevel.OnLevelUp += SetLevelUI;
            companionLevel.OnLevelUp += SetHealthUI;
            companionChara.OnDamaged += SetHealthUI;
            companionChara.OnHealed += SetHealthUI;
        }

        public void UnsubToCompanion()
        {
            if (companionChara == null || companionLevel == null) return;

            companionChara.OnDamaged -= SetHealthUI;
            companionChara.OnHealed -= SetHealthUI;
            companionChara = null;

            companionLevel.OnLevelUp -= SetLevelUI;
            companionLevel.OnLevelUp -= SetHealthUI;
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