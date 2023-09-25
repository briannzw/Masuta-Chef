using UnityEngine;

namespace Character.Stat
{
    [CreateAssetMenu(menuName = "Character/Stats Preset", fileName ="New Stats Preset")]
    public class StatsPreset : ScriptableObject
    {
        public Stats Stats;
    }
}