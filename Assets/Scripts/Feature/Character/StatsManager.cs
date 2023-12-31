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
        public Dictionary<string, Dictionary<WeaponStatsEnum, List<StatModifier>>> WeaponStatMods = new();

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
            RecipeSO.PopulateData(saveData);

            PopulateMods();
        }

        private void PopulateMods()
        {
            CharacterStatMods.Clear();
            CharacterDynamicStatMods.Clear();
            WeaponStatMods.Clear();

            foreach (var recipe in RecipeSO.Recipes)
            {
                for (int i = 0; i < recipe.Stats.Length; i++)
                {
                    if (recipe.IsLocked) continue;
                    if (recipe.data.CookingDone < RecipeSO.UnlockSettings[i]) continue;

                    var recipeStat = recipe.Stats[i];

                    // Replace Stat 3
                    if(i >= 2 && recipe.data.IsStat3Assigned)
                    {
                        recipeStat = RecipeSO.GetStatFromIndex(recipe.data.StatsIndex);
                    }

                    // Initialize Modifier with StatsManager as Source
                    recipeStat.Stat.Initialize(this);
                    if (recipeStat.Stat.StatType == StatEffect.StatsType.Character)
                    {
                        if (recipeStat.Stat.AffectDynamicStat)
                        {
                            if (!CharacterDynamicStatMods.ContainsKey(recipeStat.AffectedCharacter))
                                // Initialize new Dictionary of Stats
                                CharacterDynamicStatMods.Add(recipeStat.AffectedCharacter, new Dictionary<DynamicStatsEnum, List<StatModifier>>());

                            if (!CharacterDynamicStatMods[recipeStat.AffectedCharacter].ContainsKey(recipeStat.Stat.DynamicStatsAffected))
                            {
                                // Initialize new List of Mods on that Stats
                                CharacterDynamicStatMods[recipeStat.AffectedCharacter].Add(recipeStat.Stat.DynamicStatsAffected, new List<StatModifier>());
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
                                CharacterStatMods[recipeStat.AffectedCharacter].Add(recipeStat.Stat.StatsAffected, new List<StatModifier>());
                            }
                            // And finally add the modifier.
                            CharacterStatMods[recipeStat.AffectedCharacter][recipeStat.Stat.StatsAffected].Add(recipeStat.Stat.Modifier);
                        }
                    }
                    else if (recipeStat.Stat.StatType == StatEffect.StatsType.Weapon)
                    {
                        if (!WeaponStatMods.ContainsKey(recipeStat.AffectedCharacter))
                            // Initialize new Dictionary of Stats
                            WeaponStatMods.Add(recipeStat.AffectedCharacter, new Dictionary<WeaponStatsEnum, List<StatModifier>>());

                        if (!WeaponStatMods[recipeStat.AffectedCharacter].ContainsKey(recipeStat.Stat.WeaponStatsAffected))
                        {
                            // Initialize new List of Mods on that Stats
                            WeaponStatMods[recipeStat.AffectedCharacter].Add(recipeStat.Stat.WeaponStatsAffected, new List<StatModifier>());
                        }

                        // And finally add the modifier.
                        WeaponStatMods[recipeStat.AffectedCharacter][recipeStat.Stat.WeaponStatsAffected].Add(recipeStat.Stat.Modifier);
                    }
                }
            }
        }
    }
}