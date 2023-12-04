using Level;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelection
{
    public class LevelInfoController : MonoBehaviour
    {
        [Header("Layout Parents")]
        [SerializeField] private GameObject objectivesLayout;
        [SerializeField] private GameObject enemiesLayout;

        [Header("Layout Child")]
        [SerializeField] private GameObject objectivePrefab;
        [SerializeField] private GameObject enemyInfoPrefab;

        [Header("UI References")]
        [SerializeField] private Image levelMapIcon;
        [SerializeField] private GameObject levelBackPanel;
        [SerializeField] private GameObject levelPanel;
        [SerializeField] private TMP_Text levelTitleText;
        [SerializeField] private Image levelImage;
        [SerializeField] private TMP_Text levelInstructionText;
        [SerializeField] private GameObject levelBestTimeBar;
        [SerializeField] private TMP_Text levelBestTimeText;

        private LevelData selectedData;

        private void Awake()
        {
            levelBackPanel.SetActive(false);
            levelPanel.SetActive(false);
        }

        public void Set(LevelData data)
        {
            selectedData = data;

            levelBackPanel.SetActive(true);
            levelPanel.SetActive(true);

            // Map Icon
            levelMapIcon.sprite = data.Info.MapIcon;

            // Title
            levelTitleText.text = data.name.ToUpper() + " - " + data.Info.Region;

            // Objective
            foreach (Transform child in objectivesLayout.transform) Destroy(child.gameObject);

            foreach(var objective in data.Info.Objectives)
            {
                var go = Instantiate(objectivePrefab, objectivesLayout.transform);
                go.GetComponentInChildren<TMP_Text>().text = objective;
            }

            // Enemies Info
            foreach (Transform child in enemiesLayout.transform) Destroy(child.gameObject);

            foreach (var enemyData in data.EnemiesInLevel)
            {
                var go = Instantiate(enemyInfoPrefab, enemiesLayout.transform);
                go.transform.GetChild(0).GetComponent<Image>().sprite = enemyData.icon;
            }

            // Level Image
            levelImage.sprite = data.Info.LevelImage;

            // Level Instruction
            levelInstructionText.text = data.Info.Instruction;

            levelBestTimeBar.SetActive(!data.Info.IsTutorial);
            // Best Time
            if (GameManager.Instance.SaveManager.SaveData.LevelBestTime.ContainsKey(data.name))
            {
                TimeSpan ts = GameManager.Instance.SaveManager.SaveData.LevelBestTime[data.name];
                levelBestTimeText.text = ts.ToString(@"mm\:ss\.ff");
            }
            else levelBestTimeText.text = "-";
        }

        public void Play()
        {
            GameManager.SelectedLevel = selectedData;
            if (selectedData.Info.IsTutorial)
            {
                SceneManager.LoadScene("Tutorial 1");
                return;
            }

            SceneManager.LoadScene("Main");
        }
    }
}