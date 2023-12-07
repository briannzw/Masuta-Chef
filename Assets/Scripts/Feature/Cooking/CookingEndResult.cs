using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Cooking.Gameplay
{
    using Player.Input;
    using UnityEngine.InputSystem;

    public class CookingEndResult : PlayerInputControl
    {
        [Header("Parameters")]
        [SerializeField] private Sprite cookingSuccessSprite;
        [SerializeField] private Sprite cookingSuccessSealSprite;
        [SerializeField] private Sprite cookingFailedSprite;
        [SerializeField] private Sprite cookingFailedSealSprite;
        [SerializeField] private Sprite cookingFailedResultSprite;
        [SerializeField] private string cookingFailedBubbleText;
        [SerializeField] private SerializedDictionary<CookingResult, Sprite> resultSprite = new();
        [SerializeField] private SerializedDictionary<CookingResult, List<string>> resultBubbleText = new();

        [Header("References")]
        [SerializeField] private Image titleBackground;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text cookingPointsAmountText;
        [SerializeField] private Image cookingResultBackground;
        [SerializeField] private TMP_Text bubbleText;
        [SerializeField] private Image cookingSealImage;

        protected override void Start()
        {
            base.Start();
            InputManager.ToggleActionMap(playerControls.Panel);
        }

        public void Set(CookingResult result)
        {
            titleBackground.sprite = cookingSuccessSprite;
            titleText.text = "Cooking Success";
            cookingPointsAmountText.text = CookingManager.Instance.CookingPoints[result].ToString();
            cookingResultBackground.sprite = resultSprite[result];
            bubbleText.text = resultBubbleText[result][Random.Range(0, resultBubbleText[result].Count)];
            cookingSealImage.sprite = cookingSuccessSealSprite;
        }

        public void SetFail()
        {
            titleBackground.sprite = cookingFailedSprite;
            titleText.text = "Cooking Failed";
            cookingPointsAmountText.text = "0";
            cookingResultBackground.sprite = cookingFailedResultSprite;
            bubbleText.text = cookingFailedBubbleText;
            cookingSealImage.sprite = cookingFailedSealSprite;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            gameObject.SetActive(false);
            CookingManager.Instance.BackToRecipeBook();
        }

        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Panel.Select.canceled += OnClick;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Panel.Select.canceled -= OnClick;
        }
    }
}