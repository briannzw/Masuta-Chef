namespace Character.Stat
{
    using Weapon;
    public static class StatAbbreviation
    {
        public static string Get(DynamicStatsEnum stats)
        {
            string value = "";
            switch (stats)
            {
                case DynamicStatsEnum.Health:
                    value = "HP";
                    break;
                case DynamicStatsEnum.Mana:
                    value = "Mana";
                    break;
            }

            return value;
        }

        public static string Get(StatsEnum stats)
        {
            string value = "";
            switch (stats)
            {
                case StatsEnum.Attack:
                    value = "ATK";
                    break;
                case StatsEnum.Defense:
                    value = "DEF";
                    break;
                case StatsEnum.Speed:
                    value = "SPD";
                    break;
                case StatsEnum.Resistance:
                    value = "RES";
                    break;
                case StatsEnum.DamageMultiplier:
                    value = "F. DMG";
                    break;
            }

            return value;
        }

        public static string Get(WeaponStatsEnum stats)
        {
            string value = "";
            switch (stats)
            {
                case WeaponStatsEnum.Power:
                    value = "W. ATK";
                    break;
                case WeaponStatsEnum.Speed:
                    value = "W. SPD";
                    break;
                case WeaponStatsEnum.Accuracy:
                    value = "W. ACR"; // Subject to change
                    break;
            }

            return value;
        }
    }
}