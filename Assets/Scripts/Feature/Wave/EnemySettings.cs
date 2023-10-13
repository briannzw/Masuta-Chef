using UnityEngine;

namespace Wave
{
    using Character.Stat;
    using MyBox;

    [CreateAssetMenu(menuName = "Enemy/Enemy Settings", fileName = "New Enemy Settings")]
    public class EnemySettings : ScriptableObject
    {
        public GameObject Prefab;
        public StatsPreset StatsPreset;
        [ConditionalField(nameof(StatsPreset), inverse: true)] public Stats Stats;
    }
}