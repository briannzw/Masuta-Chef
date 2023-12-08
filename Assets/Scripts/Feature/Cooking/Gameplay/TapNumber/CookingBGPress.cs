using UnityEngine;
using UnityEngine.EventSystems;

namespace Cooking.Gameplay.TapNumber
{
    public class CookingBGPress : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CookingNumberController controller;

        public void OnPointerClick(PointerEventData eventData)
        {
            controller.OnClick();
        }
    }
}