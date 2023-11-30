using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private const string musicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(musicVolumeKey) && PlayerPrefs.HasKey(SFXVolumeKey))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume(PlayerPrefs.GetFloat(musicVolumeKey, 0.75f)); // Default volume 0.75f
            SetSFXVolume(PlayerPrefs.GetFloat(SFXVolumeKey, 0.75f)); // Default volume 0.75f
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(musicVolumeKey, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    private void LoadVolume()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat(musicVolumeKey);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey);

        musicSlider.value = savedMusicVolume;
        SFXSlider.value = savedSFXVolume;

        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);
    }
}



