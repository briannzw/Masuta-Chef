using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    [Header("Scene Load")]
    [SerializeField] private GameObject sceneLoadPrefab;

    private void LoadScene(string sceneName, int panelIndex = -1)
    {
        GameObject go = Instantiate(sceneLoadPrefab);
        go.GetComponent<SceneLoad>().loadingIndex = panelIndex;
        go.GetComponent<SceneLoad>().LoadScene(sceneName);
    }

    public void ChangeScene()
    {
        LoadScene(sceneToLoad);
    }
}
