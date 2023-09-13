using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
    using Character;
    using Chance;
    using Object;

    //public enum LootRarity { UltraRare, SuperRare, Rare, Common }

    [RequireComponent(typeof(Character))]
    public class LootDropController : MonoBehaviour
    {
        [SerializeField] private float weaponLootChance = 5;
        [SerializeField] private WeaponLootChance weaponLoots;
        [SerializeField] private float recipeLootChance = 100;
        [SerializeField] private RecipeLootChance recipeLoots;
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
            character.OnDie += DropLoot;
        }

        private void DropLoot()
        {
            if(Random.Range(0, 100) < weaponLootChance && !weaponLoots.IsEmpty)
                Instantiate(weaponLoots.LootPrefab, transform.position, Quaternion.identity);
            if (Random.Range(0, 100) < recipeLootChance && !recipeLoots.IsEmpty)
            {
                GameObject go = Instantiate(recipeLoots.LootPrefab, transform.position, Quaternion.identity);
                RecipeLootObject lootObject = go.GetComponent<RecipeLootObject>();
                RecipeLoot recipeLoot = RandomRecipeLoot(recipeLoots);
                lootObject.Recipe = recipeLoot.Recipe;
                lootObject.Count = Random.Range(recipeLoot.MinCount, recipeLoot.MaxCount + 1);
            }
            character.OnDie -= DropLoot;
        }

        private WeaponLoot RandomWeaponLoot(WeaponLootChance lootChance)
        {
            int index = 0;
            float pick = Random.value * lootChance.LootWeight;
            float cumulativeWeight = lootChance.LootList[0].RandomWeight;

            while (pick > cumulativeWeight && index < lootChance.LootList.Count - 1)
            {
                index++;
                cumulativeWeight += lootChance.LootList[index].RandomWeight;
            }

            return lootChance.LootList[index];
        }

        private RecipeLoot RandomRecipeLoot(RecipeLootChance lootChance)
        {
            int index = 0;
            float pick = Random.value * lootChance.LootWeight;
            float cumulativeWeight = lootChance.LootList[0].RandomWeight;

            while (pick > cumulativeWeight && index < lootChance.LootList.Count - 1)
            {
                index++;
                cumulativeWeight += lootChance.LootList[index].RandomWeight;
            }

            return lootChance.LootList[index];
        }
    }
}