using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class UIPauseMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public TMP_Dropdown resolutionDropdown;

    public List<string> resolutionOptions;
    public Toggle isFullscreenToggle;

    public void Awake()
    {
        SetSlidersFromVolume();
        SetResolutionOptions();
    }

    public void ResumeGame()
    {
        GameManager.instance.UnPause();
    }

    public void QuitToMenu()
    {
        GameManager.instance.UnPause();
        SceneManager.LoadScene("MainMenu");
    }

    public void SetSlidersFromVolume()
    {
        float tempFloat = 0;
        GameManager.instance.audioMixer.GetFloat("MasterVolume", out tempFloat);
        masterVolumeSlider.SetValueWithoutNotify(Mathf.Pow(10.0f, tempFloat / 20.0f));

        GameManager.instance.audioMixer.GetFloat("MusicVolume", out tempFloat);
        musicVolumeSlider.SetValueWithoutNotify(Mathf.Pow(10.0f, tempFloat / 20.0f));

        GameManager.instance.audioMixer.GetFloat("EffectsVolume", out tempFloat);
        effectsVolumeSlider.SetValueWithoutNotify(Mathf.Pow(10.0f, tempFloat / 20.0f));
    }

    public void SetVolumeFromSliders()
    {
        GameManager.instance.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
        GameManager.instance.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        GameManager.instance.audioMixer.SetFloat("EffectsVolume", Mathf.Log10(effectsVolumeSlider.value) * 20);
    }

    public void SetResolution()
    {
        Screen.SetResolution(Screen.resolutions[resolutionDropdown.value].width, Screen.resolutions[resolutionDropdown.value].height, isFullscreenToggle.isOn);
    }

   public void SetResolutionOptions()
    {
        // Make a new list of the text we want to show as resolution options. 
        // This list is parallel to our Screen.resolutions array, so the index in one array lines up with the same index in the other array!
        resolutionOptions = new List<string>();
        for (int i = 0; i<Screen.resolutions.Length; i++)
        {
            resolutionOptions.Add(Screen.resolutions[i].width + "x" + Screen.resolutions[i].height + " :" + Screen.resolutions[i].refreshRate);            
        }
        // Clear the dropdown
        resolutionDropdown.ClearOptions();

        // Add those options to the dropdown
        resolutionDropdown.AddOptions(resolutionOptions);
    }
}