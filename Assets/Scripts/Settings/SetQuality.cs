using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public class SetQuality : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [SerializeField] private UniversalRenderPipelineAsset _urpPerformant;
    [SerializeField] private UniversalRenderPipelineAsset _urpBalanced;
    [SerializeField] private UniversalRenderPipelineAsset _urpHighFidelity;

    private string qualityPrefKey = "QualityLevel"; 

    void Start()
    {
        int savedQualityLevel = LoadQualityLevel();
        qualityDropdown.value = savedQualityLevel;
        SetQualityLevelDropdown(savedQualityLevel);
    }

    public void SetQualityLevelDropdown(int index)
    {
        switch (index)
        {
            // Low
            case 0:
                _urpPerformant.shadowDistance = 50;
                break;
            // Medium
            case 1:
                _urpBalanced.shadowDistance = 50;
                break;
            // High
            case 2:
                _urpHighFidelity.shadowDistance = 150;
                break;
        }

        QualitySettings.SetQualityLevel(index, false);
        SaveQualityLevel(index); 
    }

    private void SaveQualityLevel(int level)
    {
        PlayerPrefs.SetInt(qualityPrefKey, level);
    }

    private int LoadQualityLevel()
    {
        return PlayerPrefs.GetInt(qualityPrefKey, 0); 
    }
}
