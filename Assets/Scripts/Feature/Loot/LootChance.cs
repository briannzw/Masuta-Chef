using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Loot.Chance
{
    [CreateAssetMenu(menuName = "Loot/Loot Chance", fileName = "New Loot Chance")]
    public class LootChance : ScriptableObject
    {
        //private float rarityTotalWeight;
        //[SerializeField, ReadOnly] private string rarityTotalChance;
        //[SerializeField] private SerializedDictionary<LootRarity, int> rarityChanceList;

        [ReadOnly] public float LootWeight;
        public List<Loot> LootList;

        private void OnValidate()
        {
            //rarityTotalWeight = 0f;
            //foreach(var rarity in rarityChanceList)
            //{
            //    rarityTotalWeight += rarity.Value;
            //}
            //rarityTotalChance = rarityTotalWeight.ToString() + " / 100";

            LootWeight = 0f;
            foreach (var item in LootList)
            {
                LootWeight += item.RandomWeight;
            }
        }
    }
}