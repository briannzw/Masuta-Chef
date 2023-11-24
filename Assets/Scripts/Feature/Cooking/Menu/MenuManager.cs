using Cooking.Recipe;
using Save;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void Awake()
        {
            // Load SaveData on Awake
            saveManager.Load();
            recipeSO.PopulateData(saveManager.SaveData);
        }

        private void Start()
        {
            leftPage.Set((currentPage < 0) ? null : recipeSO.Recipes[currentPage], currentPage <= 0);
            rightPage.Set((currentPage + 1 >= recipeSO.Recipes.Count) ? null : recipeSO.Recipes[currentPage + 1], currentPage + 2 > recipeSO.Recipes.Count - 1);
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
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
    }
}