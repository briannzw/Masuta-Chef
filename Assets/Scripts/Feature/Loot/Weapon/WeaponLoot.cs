using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
    public class WeaponLoot : Loot
    {
        public List<GameObject> Weapons;

        public WeaponLoot()
        {
            Type = LootType.Weapon;
        }
    }
}