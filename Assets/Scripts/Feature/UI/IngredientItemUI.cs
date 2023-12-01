using Cooking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IngredientItemUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image ingredientIcon;
        [SerializeField] private TMP_Text ingredientCountText;

        public void Set(Ingredient ingredient, int count)
        {
            ingredientIcon.sprite = ingredient.Icon;
            ingredientCountText.text = count.ToString();
        }
    }
}