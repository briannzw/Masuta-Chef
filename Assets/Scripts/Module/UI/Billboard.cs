using UnityEngine;

namespace Module.UI
{
    using Character;
    using Character.Stat;

    public class Billboard : MonoBehaviour
    {
        [SerializeField] Character chara;
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
        private void Update()
        {
            Debug.Log((chara.Stats.StatList[StatsEnum.Health] as CharacterDynamicStat).CurrentValue);
        }
    }
}