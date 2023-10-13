using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
    using AYellowpaper.SerializedCollections;
    using MyBox;

    [CreateAssetMenu(menuName = "Loot/Loot Chance", fileName = "New Loot Chance")]
    public class LootChance : ScriptableObject
    {
        [Header("References")]
        public SerializedDictionary<LootType, GameObject> LootPrefab;

        [SerializeReference]
        public List<Loot> LootList = new();

        [ButtonMethod]
        public void AddRecipeLoot()
        {
            LootList.Add(new RecipeLoot());
        }

        [ButtonMethod]
        public void AddWeaponLoot()
        {
            LootList.Add(new WeaponLoot());
        }
    }
}