using UnityEngine;
using UnityEngine.UI;

namespace Module.UI
{
    using Character;

    public class StatsBattleUI : MonoBehaviour
    {
        [SerializeField] private Character chara;
        [SerializeField] private Slider slider;
        [SerializeField] private bool disableOnAwake = true;
        [SerializeField] private bool useBillboard = true;
        [SerializeField] private bool disableOnDie = true;
        [SerializeField] private DynamicStatsEnum DynamicEnum = DynamicStatsEnum.Health;

        private void Awake()
        {
            if(chara == null) chara = GetComponentInParent<Character>();
            if(slider == null) slider = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            slider.gameObject.SetActive(!disableOnAwake);
            if (chara.CompareTag("Companion")) chara.GetComponent<CompanionLevelSystem>().OnLevelUp += MaxValueChanged;
            if(disableOnDie) chara.OnDie += DisableBar;
            chara.OnDamaged += ValueChanged;
            chara.OnHealed += ValueChanged;
            chara.OnStatsInitialized += Initialize;
        }

        private void OnDisable()
        {
            if (chara != null && chara.CompareTag("Companion")) chara.GetComponent<CompanionLevelSystem>().OnLevelUp -= MaxValueChanged;
            if(disableOnDie) chara.OnDie -= DisableBar;
            chara.OnDamaged -= ValueChanged;
            chara.OnHealed -= ValueChanged;
        }

        private void Initialize()
        {
            slider.maxValue = chara.Stats.DynamicStatList[DynamicEnum].Value;
            if(!disableOnAwake) ValueChanged();
            chara.OnStatsInitialized -= Initialize;
        }

        private void LateUpdate()
        {
            if (useBillboard) transform.LookAt(transform.position + Camera.main.transform.forward);
        }

        private void DisableBar()
        {
            slider.gameObject.SetActive(false);
        }

        private void MaxValueChanged()
        {
            slider.maxValue = chara.Stats.DynamicStatList[DynamicEnum].Value;
            ValueChanged();
        }

        private void ValueChanged()
        {
            if(!slider.gameObject.activeSelf) slider.gameObject.SetActive(true);
            slider.value = chara.Stats.DynamicStatList[DynamicEnum].CurrentValue;
        }
    }
}
