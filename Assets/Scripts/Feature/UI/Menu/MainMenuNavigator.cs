using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuNavigator : MonoBehaviour
    {
        [Header("Scene Load")]
        [SerializeField] private GameObject sceneLoadPrefab;

        public void Play()
        {
            LoadScene("LevelSelection");
        }

        public void RecipeBook()
        {
            LoadScene("Menu");
        }

        public void Quit()
        {
            Application.Quit(0);
        }

        private void LoadScene(string sceneName, int panelIndex = -1)
        {
            GameObject go = Instantiate(sceneLoadPrefab);
            go.GetComponent<SceneLoad>().loadingIndex = panelIndex;
            go.GetComponent<SceneLoad>().LoadScene(sceneName);
        }
    }
}