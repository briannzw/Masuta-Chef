using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    using Character;
    public class Weapon : MonoBehaviour
    {
        // Currently using this weapon
        [ReadOnly] public Character Character;
        public float DamageScale = 1;

        protected virtual void Start()
        {
            Character = GetComponentInParent<Character>();
        }

        public void Equip(Character chara)
        {
            Character = chara;
        }
    }
}