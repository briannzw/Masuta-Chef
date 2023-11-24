using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Cooking.Menu
{
    using Recipe;

    public class MenuPage : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform menuInfo;
        [SerializeField] private RectTransform lockOverlay;
        [SerializeField] private Image menuImage;
        [SerializeField] private Button menuImageButton;
        [SerializeField] private Image menuNotifIcon;
        [SerializeField] private GameObject menuLockIcon;
        [SerializeField] private TMP_Text menuName;
        [SerializeField] private Transform menuIngredientsParent;
        [SerializeField] private TMP_Text menuDescription;
        [SerializeField] private Button pageButton;

        [Header("UI Settings")]
        [SerializeField] private Vector2 menuIngredientItemSize = new(70f, 70f);

        [Header("Switch Animation")]
        [SerializeField] private Image nextPageImage;
        [SerializeField] private GameObject nextPageLockIcon;
        [SerializeField] private float switchDistance = 600f;
        [SerializeField] private float switchDuration = 0.5f;
        private Coroutine switchCoroutine;
        private Coroutine lockSwitchCoroutine;
        private bool isSkipped;
        public bool IsAnimating => switchCoroutine != null || lockSwitchCoroutine != null;

        private bool isLocked = false;

        public void Set(Recipe recipe, bool isFirstOrLastPage)
        {
            menuInfo.gameObject.SetActive(recipe != null);
            lockOverlay.gameObject.SetActive(recipe == null);

            menuNotifIcon.gameObject.SetActive(false);

            if (recipe == null) return;

            // Clear Ingredients
            foreach (Transform child in menuIngredientsParent.transform) Destroy(child.gameObject);

            // Add Ingredient Items
            foreach (var ingredient in recipe.Ingredients)
            {
                var imageObj = new GameObject("Ingredient Item");

                var rect = imageObj.AddComponent<RectTransform>();
                rect.transform.SetParent(menuIngredientsParent);
                rect.localScale = Vector3.one;
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = menuIngredientItemSize;

                var image = imageObj.AddComponent<Image>();
                image.sprite = ingredient.Key.CookingIcon;
                image.color = (recipe.IsLocked) ? Color.black : Color.white;
                imageObj.transform.SetParent(menuIngredientsParent);
            }

            // Description
            menuDescription.text = recipe.Description;

            menuImage.sprite = (recipe.IsLocked) ? recipe.LockedIcon : recipe.Icon;

            menuImageButton.interactable = !recipe.IsLocked;

            if(!recipe.IsLocked) menuNotifIcon.gameObject.SetActive(recipe.HasEnoughIngredients());

            menuLockIcon.SetActive(recipe.IsLocked);
            menuName.text = (recipe.IsLocked) ? "???" : recipe.name;

            // Page Index
            pageButton.gameObject.SetActive(!isFirstOrLastPage);
        }

        public void SwitchPage(Recipe recipe, int direction, bool isFirstOrLastPage)
        {
            menuInfo.gameObject.SetActive(true);

            // Page Index
            pageButton.gameObject.SetActive(false);

            menuImageButton.interactable = false;

            if (recipe == null)
            {
                if(!isLocked) lockSwitchCoroutine = StartCoroutine(AnimateSwitch(-direction, isFirstOrLastPage));
                lockOverlay.gameObject.SetActive(true);
                return;
            }

            if (isLocked)
            {
                lockSwitchCoroutine = StartCoroutine(AnimateSwitch(-direction, isFirstOrLastPage));
                return;
            }

            nextPageImage.rectTransform.anchoredPosition = menuImage.rectTransform.anchoredPosition + direction * switchDistance * Vector2.right;

            nextPageImage.gameObject.SetActive(true);
            nextPageImage.sprite = (recipe.IsLocked) ? recipe.LockedIcon : recipe.Icon;
            nextPageLockIcon.SetActive(recipe.IsLocked);

            switchCoroutine = StartCoroutine(AnimateSwitch(recipe, -direction, isFirstOrLastPage));
        }

        public void StopSwitch()
        {
            isSkipped = true;
            switchCoroutine = null;
            lockSwitchCoroutine = null;
        }

        private IEnumerator AnimateSwitch(Recipe recipe, int direction, bool isFirstOrLastPage)
        {
            float time = 0f;
            Vector2 startPos = menuImage.rectTransform.anchoredPosition;
            Vector2 targetPos = menuImage.rectTransform.anchoredPosition + direction * switchDistance * Vector2.right;

            Vector2 nextStartPos = nextPageImage.rectTransform.anchoredPosition;
            Vector2 nextTargetPos = startPos;
            while (time < switchDuration && !isSkipped)
            {
                menuImage.rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, time / switchDuration);
                nextPageImage.rectTransform.anchoredPosition = Vector2.Lerp(nextStartPos, nextTargetPos, time / switchDuration);
                time += Time.deltaTime;
                yield return null;
            }

            menuImage.rectTransform.anchoredPosition = targetPos;
            nextPageImage.rectTransform.anchoredPosition = nextTargetPos;
            isSkipped = false;

            // Delay
            //yield return new WaitForSeconds(.2f);

            Set(recipe, isFirstOrLastPage);
            menuImage.rectTransform.anchoredPosition = startPos;
            nextPageImage.rectTransform.anchoredPosition = targetPos;
            nextPageImage.gameObject.SetActive(false);
            switchCoroutine = null;
        }

        private IEnumerator AnimateSwitch(int direction, bool isFirstOrLastPage)
        {
            if (!isLocked)
                lockOverlay.anchoredPosition = Vector2.zero + -direction * menuInfo.sizeDelta.x * Vector2.right;
            else
                menuInfo.anchoredPosition = Vector2.zero + -direction * menuInfo.sizeDelta.x * Vector2.right;

            float time = 0f;
            Vector2 startPos = menuInfo.anchoredPosition;
            Vector2 targetPos = menuInfo.anchoredPosition + direction * menuInfo.sizeDelta.x * Vector2.right;

            Vector2 nextStartPos = lockOverlay.anchoredPosition;
            Vector2 nextTargetPos = isLocked ? nextStartPos + direction * menuInfo.sizeDelta.x * Vector2.right : startPos;

            while (time < switchDuration && !isSkipped)
            {
                menuInfo.anchoredPosition = Vector2.Lerp(startPos, targetPos, time / switchDuration);
                lockOverlay.anchoredPosition = Vector2.Lerp(nextStartPos, nextTargetPos, time / switchDuration);
                time += Time.deltaTime;
                yield return null;
            }

            menuInfo.anchoredPosition = targetPos;
            lockOverlay.anchoredPosition = nextTargetPos;
            isSkipped = false;
            isLocked = !isLocked;

            menuInfo.gameObject.SetActive(!isLocked);
            menuImageButton.interactable = !isLocked;
            lockOverlay.gameObject.SetActive(isLocked);

            // Page Index
            pageButton.gameObject.SetActive(!isFirstOrLastPage);
            lockSwitchCoroutine = null;
        }
    }
}