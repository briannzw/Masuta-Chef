using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    using Cooking.Recipe;
    using Kryz.CharacterStats;
    using Save.Data;
    using Weapon;

    public class StatsManager : MonoBehaviour
    {
        // Could be not the best practice.
        public Dictionary<string, Dictionary<StatsEnum, List<StatModifier>>> CharacterStatMods = new();
        public Dictionary<string, Dictionary<DynamicStatsEnum, List<StatModifier>>> CharacterDynamicStatMods = new();
        public Dictionary<string, Dictionary<Weapon.WeaponStatsEnum, List<StatModifier>>> WeaponStatMods = new();

        public RecipeListSO RecipeSO;
        private SaveData saveData;

        public void Load()
        {
            saveData = GameManager.Instance.SaveManager.SaveData;

            if(RecipeSO == null)
            {
                Debug.LogError("Assign RecipeSO field in inspector before loading stats!");
                return;
            }

            // Update SO Data
            foreach (var recipe in RecipeSO.Recipes)
            {
                if (saveData.RecipeData.ContainsKey(recipe.name))
                    recipe.data = saveData.RecipeData[recipe.name];
            }

            PopulateMods();
        }

        private void PopulateMods()
        {
            CharacterStatMods.Clear();
            CharacterDynamicStatMods.Clear();
            WeaponStatMods.Clear();

            foreach (var recipe in RecipeSO.Recipes)
            {
                if (recipe.CurrentStats == null) continue;

                var recipeStat = recipe.CurrentStats;
                // Initialize Modifier with StatsManager as Source
                recipeStat.Stat.Initialize(this);
                if (recipeStat.Stat.StatType == StatEffect.StatsType.Character)
                {
                    if (recipeStat.Stat.AffectDynamicStat)
                    {
                        if(!CharacterDynamicStatMods.ContainsKey(recipeStat.AffectedCharacter))
                            // Initialize new Dictionary of Stats
                            CharacterDynamicStatMods.Add(recipeStat.AffectedCharacter, new Dictionary<DynamicStatsEnum, List<StatModifier>>());

                        if (!CharacterDynamicStatMods[recipeStat.AffectedCharacter].ContainsKey(recipeStat.Stat.DynamicStatsAffected)) {
                            // Initialize new List of Mods on that Stats
                            CharacterDynamicStatMods[recipeStat.AffectedCharacter] = new()
                            {
                                { recipeStat.Stat.DynamicStatsAffected, new List<StatModifier>() }
                            };
                        }

                        // And finally add the modifier.
                        CharacterDynamicStatMods[recipeStat.AffectedCharacter][recipeStat.Stat.DynamicStatsAffected].Add(recipeStat.Stat.Modifier);
                    }
                    else
                    {
                        if (!CharacterStatMods.ContainsKey(recipeStat.AffectedCharacter))
                            // Initialize new Dictionary of Stats
                            CharacterStatMods.Add(recipeStat.AffectedCharacter, new Dictionary<StatsEnum, List<StatModifier>>());

                        if (!CharacterStatMods[recipeStat.AffectedCharacter].ContainsKey(recipeStat.Stat.StatsAffected))
                        {
                            // Initialize new List of Mods on that Stats
                            CharacterStatMods[recipeStat.AffectedCharacter] = new()
                            {
                                { recipeStat.Stat.StatsAffected, new List<StatModifier>() }
                            };
                        }

                        // And finally add the modifier.
                        CharacterStatMods[recipeStat.AffectedCharacter][recipeStat.Stat.StatsAffected].Add(recipeStat.Stat.Modifier);
                    }
                }
                else if (recipeStat.Stat.StatType == StatEffect.StatsType.Weapon)
                {
                    if (!WeaponStatMods.ContainsKey(recipeStat.AffectedCharacter))
                        // Initialize new Dictionary of Stats
                        WeaponStatMods.Add(recipeStat.AffectedCharacter, new Dictionary<Weapon.WeaponStatsEnum, List<StatModifier>>());

                    if (!WeaponStatMods[recipeStat.AffectedCharacter].ContainsKey(recipeStat.Stat.WeaponStatsAffected))
                    {
                        // Initialize new List of Mods on that Stats
                        WeaponStatMods[recipeStat.AffectedCharacter] = new()
                        {
                            { recipeStat.Stat.WeaponStatsAffected, new List<StatModifier>() }
                        };
                    }

                    // And finally add the modifier.
                    WeaponStatMods[recipeStat.AffectedCharacter][recipeStat.Stat.WeaponStatsAffected].Add(recipeStat.Stat.Modifier);
                }
            }
        }
    }
}