using UnityEngine;

namespace Loot
{
    using Character;
    using Object;
    using System.Collections.Generic;
    using UnityEngine.AI;

    [RequireComponent(typeof(Character))]
    public class LootDropController : MonoBehaviour
    {
        public LootChance lootChance;
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
            character.OnDie += DropLoots;
        }

        private void DropLoots()
        {
            NavMesh.SamplePosition(transform.position, out var hit, 10f, 1 << LayerMask.GetMask("Walkable"));

            foreach (var loot in RandomLoots())
            {
                if(loot.Type == LootType.Recipe)
                {
                    GameObject lootObj = Instantiate(lootChance.LootPrefab[LootType.Recipe], hit.position, Quaternion.identity);
                    int index = Random.Range(0, (loot as RecipeLoot).Recipes.Count);
                    lootObj.GetComponent<RecipeLootObject>().Set((loot as RecipeLoot).Recipes[index], lootChance.RecipeAmount);
                }
                else if(loot.Type == LootType.Weapon)
                {
                    int index = Random.Range(0, (loot as WeaponLoot).Weapons.Count);
                    Instantiate((loot as WeaponLoot).Weapons[index], hit.position, Quaternion.identity);
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