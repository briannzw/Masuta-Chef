using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking.Recipe.UI
{
    using MyBox;
    public class IngredientItem : MonoBehaviour
    {
        public Image IngredientIcon;
        public TMP_Text IngredientName;
        public Color NotEnoughColor;

        public void Set(Ingredient ingredient, int count = 0)
        {
            IngredientIcon.sprite = ingredient.CookingIcon;
            if (count == 0) IngredientName.text = ingredient.data.Count + " " + ingredient.name;
            else
            {
                if (ingredient.data.Count < count) IngredientName.text = $"<color={NotEnoughColor.ToHex()}>{ingredient.data.Count}</color>";
                else IngredientName.text = ingredient.data.Count.ToString();
                IngredientName.text += $" / {count} {ingredient.name}";
            }
        }
    }
}