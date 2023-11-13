using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadSceneMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadTutorial1()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
