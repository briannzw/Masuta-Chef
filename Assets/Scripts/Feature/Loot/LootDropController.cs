using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
    using Character;
    using Chance;

    //public enum LootRarity { UltraRare, SuperRare, Rare, Common }

    [RequireComponent(typeof(Character))]
    public class LootDropChance : MonoBehaviour
    {
        [SerializeField] private LootChance lootChance;
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
            character.OnDie += DropLoot;
        }

        private void DropLoot()
        {
            Instantiate(RandomLootPrefab(), transform.position, Quaternion.identity);
            character.OnDie -= DropLoot;
        }

        private GameObject RandomLootPrefab()
        {
            int index = 0;
            float pick = UnityEngine.Random.value * lootChance.LootWeight;
            float cumulativeWeight = lootChance.LootList[0].RandomWeight;

            while (pick > cumulativeWeight && index < lootChance.LootList.Count - 1)
            {
                index++;
                cumulativeWeight += lootChance.LootList[index].RandomWeight;
            }

            return lootChance.LootList[index].Prefab;
        }
    }
}