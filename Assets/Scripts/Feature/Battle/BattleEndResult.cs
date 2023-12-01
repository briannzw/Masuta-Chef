using Cooking;
using Player.Input;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Result
{
    public class BattleEndResult : PlayerInputControl
    {
        [Header("Parameters")]
        [SerializeField] private Sprite battleVictorySprite;
        [SerializeField] private Sprite battleVictorySealSprite;
        [SerializeField] private Sprite battleLoseSprite;
        [SerializeField] private Sprite battleLoseSealSprite;
        [SerializeField] private GameObject ingredientItemPrefab;

        [Header("References")]
        [SerializeField] private Image titleBackground;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private GameObject ingredientItemsParent;
        [SerializeField] private Image blueprintIcon;
        [SerializeField] private TMP_Text blueprintNumText;
        [SerializeField] private TMP_Text blueprintNameText;
        [SerializeField] private TMP_Text bubbleText;
        [SerializeField] private Image sealImage;
        [SerializeField] private GameObject noneIngredient;
        [SerializeField] private GameObject noneBlueprint;

        [Header("Clear Time")]
        [SerializeField] private GameObject clearTimeObject;
        [SerializeField] private GameObject bestClearTime;
        [SerializeField] private TMP_Text clearTimeText;

        [Header("Tips")]
        [SerializeField] private GameObject tipsObject;
        [SerializeField] private TMP_Text tipsText;
        [TextArea, SerializeField] private List<string> tipsContents = new();

        private void Awake()
        {
            bestClearTime.SetActive(false);
            noneIngredient.SetActive(false);
            noneBlueprint.SetActive(false);
            tipsObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            InputManager.ToggleActionMap(playerControls.Panel);
        }

        private void CheckIngredient(bool value)
        {
            ingredientItemsParent.SetActive(value);
            noneIngredient.SetActive(!value);
        }

        private void CheckBlueprint(bool value)
        {
            blueprintIcon.gameObject.SetActive(value);
            blueprintNumText.transform.parent.gameObject.SetActive(value);
            blueprintNameText.transform.parent.gameObject.SetActive(value);
            noneBlueprint.SetActive(!value);
        }

        public void Set(bool victory, BattleResult result)
        {
            // Title
            titleBackground.sprite = (victory) ? battleVictorySprite : battleLoseSprite;
            titleText.text = (victory) ? "Victory" : "Lose";

            // Rewards
            // - Ingredients
            foreach(Transform child in ingredientItemsParent.transform)
            {
                Destroy(child.gameObject);
            }

            bool valid = false;
            foreach(var ingredient in result.ingredientsCollected)
            {
                if (ingredient.Value == 0) continue;
                var item = Instantiate(ingredientItemPrefab, ingredientItemsParent.transform);
                item.GetComponent<IngredientItemUI>().Set(ingredient.Key, ingredient.Value);

                valid = true;
            }

            CheckIngredient(result.ingredientsCollected.Count > 0 && valid);

            // - Blueprint
            valid = false;
            foreach(var blueprint in result.blueprintsCollected)
            {
                if (blueprint.Value == 0) continue;
                blueprintIcon.sprite = blueprint.Key.Icon;
                blueprintIcon.color = blueprint.Key.IsLocked ? Color.black : Color.white;
                blueprintNameText.text = $"Recipe :\n{blueprint.Key.name}";
                blueprintNumText.text = blueprint.Key.IsLocked ? $"{blueprint.Value}/{blueprint.Key.NeededBlueprint}" : "";
                // Only first [TEMP]
                valid = true;
                break;
            }

            CheckBlueprint(result.blueprintsCollected.Count > 0 && valid);

            clearTimeObject.SetActive(victory);
            tipsObject.SetActive(!victory);

            if (victory)
            {
                // Clear Time
                clearTimeText.text = result.clearDuration.ToString(@"mm\:ss\.ff");
                //TODO : Check best
                bestClearTime.SetActive(result.isBestTime);
            }
            else
            {
                // Tips
                tipsText.text = tipsContents[Random.Range(0, tipsContents.Count)];
            }

            // Seal
            sealImage.sprite = victory ? battleVictorySealSprite : battleLoseSealSprite;
            bubbleText.text = victory ? "Good Job!" : "Try Harder!";
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            gameObject.SetActive(false);
            GameManager.Instance.BackToLevelSelection();
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