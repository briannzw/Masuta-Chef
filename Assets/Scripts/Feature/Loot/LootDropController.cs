using UnityEngine;

namespace Loot
{
    using Character;
    using Interactable;
    using Object;
    using System.Collections.Generic;

    [RequireComponent(typeof(Character))]
    public class LootDropController : MonoBehaviour
    {
        [SerializeField] private LootChance lootChance;
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
            character.OnDie += DropLoots;
        }

        private void DropLoots()
        {
            foreach (var loot in RandomLoots())
            {
                GameObject lootObj = Instantiate(lootChance.LootPrefab[loot.Type], transform.position, Quaternion.identity);
                if(loot.Type == LootType.Recipe)
                {
                    int index = Random.Range(0, (loot as RecipeLoot).Recipes.Count);
                    lootObj.GetComponent<RecipeLootObject>().Recipe = (loot as RecipeLoot).Recipes[index];
                }
                else if(loot.Type == LootType.Weapon)
                {
                    int index = Random.Range(0, (loot as WeaponLoot).Weapons.Count);
                    //lootObj.GetComponent<WeaponLootObject>().Data = (loot as WeaponLoot).Weapons[index];
                }
            }
            character.OnDie -= DropLoots;
        }

        private List<Loot> RandomLoots()
        {
            List<Loot> lootsGO = new List<Loot>();

            foreach(var loot in lootChance.LootList)
            {
                int randomNum = Random.Range(1, 101);
                if(randomNum <= loot.Chance)
                {
                    lootsGO.Add(loot);
                }
            }

            return lootsGO;
        }
    }
}