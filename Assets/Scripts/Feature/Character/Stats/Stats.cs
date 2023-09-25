namespace Character.Stat
{
    using AYellowpaper.SerializedCollections;
    using Kryz.CharacterStats;
    using System;

    [System.Serializable]
    public class Stats
    {
        public SerializedDictionary<StatsEnum, CharacterStat> StatList;

        // Default Values
        public Stats()
        {
            StatList = new SerializedDictionary<StatsEnum, CharacterStat>
            {
                { StatsEnum.Health, new CharacterDynamicStat(100) },
                { StatsEnum.Mana, new CharacterDynamicStat(100) },
                { StatsEnum.Attack, new CharacterStat() },
                { StatsEnum.Defense, new CharacterStat() },
                { StatsEnum.Speed, new CharacterStat(50) },
                { StatsEnum.Resistance, new CharacterStat() },
                { StatsEnum.DamageMultiplier, new CharacterStat(1) }
            };
        }
    }
}