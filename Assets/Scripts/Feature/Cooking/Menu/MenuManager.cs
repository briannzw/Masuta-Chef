using Cooking.Recipe;
using Save;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cooking.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private RecipeListSO recipeSO;

        [Header("References")]
        [SerializeField] private MenuPage leftPage;
        [SerializeField] private MenuPage rightPage;

        private int currentPage = 0;

        [Header("Notify")]
        [SerializeField] private GameObject notifyPanel;
        [SerializeField] private float waitAnimation = 2f;
        [SerializeField] private TMP_Text cookingTitleText;
        [SerializeField] private Image cookingImage;

        private Queue<Recipe.Recipe> notifyRecipe = new();

        [Header("Scene Load")]
        [SerializeField] private GameObject sceneLoadPrefab;

        private void Awake()
        {
            // Load SaveData on Awake
            saveManager.Load();
            recipeSO.PopulateData(saveManager.SaveData);

            foreach(var recipe in recipeSO.Recipes)
            {
                if(!recipe.data.IsNotified == false && !recipe.IsLocked)
                    notifyRecipe.Enqueue(recipe);
            }

            if (notifyRecipe.Count > 0) StartCoroutine(Notify());
        }

        private void Start()
        {
            leftPage.Set((currentPage < 0) ? null : recipeSO.Recipes[currentPage], currentPage <= 0);
            rightPage.Set((currentPage + 1 >= recipeSO.Recipes.Count) ? null : recipeSO.Recipes[currentPage + 1], currentPage + 2 > recipeSO.Recipes.Count - 1);
        }

        public void BackToMenu()
        {
            LoadScene("MainMenu");
        }

        public void ChangePage(int direction)
        {
            if (currentPage + direction < 0 || currentPage + direction > recipeSO.Recipes.Count) return;
            currentPage += direction * 2;

            if (leftPage.IsAnimating || rightPage.IsAnimating)
            {
                currentPage -= direction * 2;
                leftPage.StopSwitch();
                rightPage.StopSwitch();
            }

            leftPage.SwitchPage((currentPage < 0) ? null : recipeSO.Recipes[currentPage], direction, currentPage <= 0);
            rightPage.SwitchPage((currentPage + 1 >= recipeSO.Recipes.Count) ? null : recipeSO.Recipes[currentPage + 1], direction, currentPage + 2 > recipeSO.Recipes.Count - 1);
        }

        public void Select(bool isRightButton)
        {
            CookingManager.selectedIndex = currentPage + (isRightButton ? 1 : 0);
            SceneManager.LoadScene("RecipeBook");
        }

        private IEnumerator Notify()
        {
            notifyPanel.SetActive(true);

            while(notifyRecipe.TryDequeue(out var recipe))
            {
                cookingTitleText.text = recipe.name;
                cookingImage.sprite = recipe.Icon;
                yield return new WaitForSeconds(waitAnimation);
                saveManager.SaveData.RecipeData[recipe.name].IsNotified = true;
            }
            notifyRecipe.Clear();
            saveManager.Save();

            notifyPanel.SetActive(false);
        }

        private void LoadScene(string sceneName, int panelIndex = -1)
        {
            GameObject go = Instantiate(sceneLoadPrefab);
            go.GetComponent<SceneLoad>().loadingIndex = panelIndex;
            go.GetComponent<SceneLoad>().LoadScene(sceneName);
        }
    }
}