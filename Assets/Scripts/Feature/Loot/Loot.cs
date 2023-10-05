using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
    public enum LootType { Recipe, Weapon }

    [Serializable]
    public abstract class Loot
    {
        public LootType Type;
        public float Chance;
    }
}
