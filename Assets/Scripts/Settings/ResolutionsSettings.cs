using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    private string resolutionPrefKey = "ResolutionIndex";

    void OnEnable()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            // Check if the aspect ratio is 16:9
            if (Is16By9AspectRatio(resolutions[i]))
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);

        SaveResolutionIndex(resolutionIndex);
    }

    private void SaveResolutionIndex(int index)
    {
        PlayerPrefs.SetInt(resolutionPrefKey, index);
    }

    private int LoadResolutionIndex()
    {
        return PlayerPrefs.GetInt(resolutionPrefKey, 0);
    }

    void Start()
    {
        currentResolutionIndex = LoadResolutionIndex();
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Helper function to check if the aspect ratio is 16:9
    private bool Is16By9AspectRatio(Resolution resolution)
    {
        return Mathf.Approximately((float)resolution.width / resolution.height, 16f / 9f);
    }
}