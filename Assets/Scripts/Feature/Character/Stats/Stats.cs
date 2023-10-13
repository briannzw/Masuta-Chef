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

        public Stats(Stats oldStat)
        {
            StatList = new SerializedDictionary<StatsEnum, CharacterStat>();
            foreach(StatsEnum Enum in Enum.GetValues(typeof(StatsEnum)))
            {
                if (oldStat.StatList[Enum] is CharacterDynamicStat)
                    StatList.Add(Enum, new CharacterDynamicStat(oldStat.StatList[Enum].Value));
                else
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