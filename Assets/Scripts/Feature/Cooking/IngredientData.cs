using MyBox;
using Newtonsoft.Json;
using System;

namespace Cooking
{
    [Serializable]
    public class IngredientData
    {
        [ReadOnly] public int Count;
        public int MaxCount = 200;

        [JsonIgnore] public bool IsMaxed => Count >= MaxCount;
    }
}