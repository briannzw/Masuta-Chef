namespace Character.Stat
{
    using AYellowpaper.SerializedCollections;
    using Kryz.CharacterStats;
    using System;

    [Serializable]
    public class Stats
    {
        public SerializedDictionary<DynamicStatsEnum, CharacterDynamicStat> DynamicStatList;
        public SerializedDictionary<StatsEnum, CharacterStat> StatList;

        // Default Values
        public Stats()
        {
            DynamicStatList = new SerializedDictionary<DynamicStatsEnum, CharacterDynamicStat>
            {
                { DynamicStatsEnum.Health, new CharacterDynamicStat(100) },
                { DynamicStatsEnum.Mana, new CharacterDynamicStat(100) }
            };
            StatList = new SerializedDictionary<StatsEnum, CharacterStat>
            {
                { StatsEnum.Attack, new CharacterStat() },
                { StatsEnum.Defense, new CharacterStat() },
                { StatsEnum.Speed, new CharacterStat(50) },
                { StatsEnum.Resistance, new CharacterStat() },
                { StatsEnum.DamageMultiplier, new CharacterStat(1) }
            };
        }

        public Stats(Stats oldStat)
        {
            DynamicStatList = new SerializedDictionary<DynamicStatsEnum, CharacterDynamicStat>();
            StatList = new SerializedDictionary<StatsEnum, CharacterStat>();
            foreach(DynamicStatsEnum Enum in Enum.GetValues(typeof(DynamicStatsEnum)))
            {
                DynamicStatList.Add(Enum, new CharacterDynamicStat(oldStat.DynamicStatList[Enum].Value));
            }
            foreach(StatsEnum Enum in Enum.GetValues(typeof(StatsEnum)))
            {
                StatList.Add(Enum, new CharacterStat(oldStat.StatList[Enum].Value));
            }
        }

        // For weapon or single instance applier
        public void RemoveAllModifiersFromSource(object source)
        {
            foreach(var stat in StatList)
            {
                stat.Value.RemoveAllModifiersFromSource(source);
            }
        }
    }
}