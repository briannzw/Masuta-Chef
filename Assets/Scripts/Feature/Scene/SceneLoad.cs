using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public int loadingIndex = -1;
    [SerializeField] private TipsSO tipsSO;
    [SerializeField] private List<GameObject> loadingPanelList;
    [SerializeField] private List<TMP_Text> tipsText;
    [SerializeField] private Slider loadingSlider;

    private int selectedIndex = 0;

    public void LoadScene(string sceneName)
    {
        if (loadingIndex == -1) selectedIndex = Random.Range(0, loadingPanelList.Count);
        else selectedIndex = loadingIndex;

        tipsText[selectedIndex].text = tipsSO.RandomTips();
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        foreach (var go in loadingPanelList) go.SetActive(false);
        loadingPanelList[selectedIndex].SetActive(true);

        float progressValue = 0f;

        while(!operation.isDone)
        {
            progressValue = Mathf.Clamp01(operation.progress / .9f);

            loadingSlider.value = progressValue;

            yield return null;
        }
    }

}
