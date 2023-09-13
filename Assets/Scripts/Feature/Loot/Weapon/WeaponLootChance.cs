using System.Collections.Generic;
using UnityEngine;

namespace Loot.Chance
{
    [CreateAssetMenu(menuName = "Loot/Weapon Loot", fileName = "New Weapon Loot Chance")]
    public class WeaponLootChance : LootChance
    {
        //private float rarityTotalWeight;
        //[SerializeField, ReadOnly] private string rarityTotalChance;
        //[SerializeField] private SerializedDictionary<LootRarity, int> rarityChanceList;

        [ReadOnly] public float LootWeight;
        public List<WeaponLoot> LootList;
        public bool IsEmpty => LootList.Count == 0;

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