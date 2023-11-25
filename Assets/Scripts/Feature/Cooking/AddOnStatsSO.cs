using System.Collections.Generic;
using UnityEngine;

namespace Character.StatEffect
{
    [CreateAssetMenu(menuName = "Character/AddOnStats SO", fileName = "new AddOnStatsSO")]
    public class AddOnStatsSO : ScriptableObject
    {
        public List<AddOnStat> Stats = new();
    }
}