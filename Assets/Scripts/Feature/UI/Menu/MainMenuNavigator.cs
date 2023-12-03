using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuNavigator : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("LevelSelection");
        }

        public void RecipeBook()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Quit()
        {
            Application.Quit(0);
        }
    }
}