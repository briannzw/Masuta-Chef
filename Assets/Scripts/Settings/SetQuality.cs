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

    public void SetQualityLevelDropdown(int index)
    {
        switch(index)
        {
            //low
            case 0:
                _urpPerformant.shadowDistance = 50;
                break;
            //medium
            case 1:
                _urpBalanced.shadowDistance = 50;
                break;
            //High
            case 2:
                _urpHighFidelity.shadowDistance = 150;
                break;
        }
        QualitySettings.SetQualityLevel(index, false);
    }
  
}
