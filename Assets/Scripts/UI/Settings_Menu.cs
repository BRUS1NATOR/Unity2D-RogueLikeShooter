using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Settings_Menu : MonoBehaviour
{
    public Slider volumeSlider;
    public Text volumeText;

    public Slider sfxVolumeSlider;
    public Text sfxVolumeText;

    public Slider musicVolumeSlider;
    public Text musicVolumeText;

    public Slider ambientVolumeSlider;
    public Text ambientVolumeText;

    public Slider shotsVolumeSlider;
    public Text shotsVolumeText;

    public Slider fpsSlider;
    public Text fpsText;

    public Toggle vsyncToggle;

    public Dropdown dropDownResolution;
    public Toggle fullScreenToggle;

    public void Awake()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void Save()
    {
        Debug.Log("SAVING!!");
        SettingsValues settings = new SettingsValues();

        settings.volume = volumeSlider.value;
        settings.music = musicVolumeSlider.value;
        settings.ambient = ambientVolumeSlider.value;
        settings.sfx = sfxVolumeSlider.value;
        settings.shots = shotsVolumeSlider.value;

        settings.fps = System.Convert.ToInt32(fpsSlider.value);

        if (vsyncToggle.isOn)
        {
            settings.VSYNC = 1;
        }
        else
        {
            settings.VSYNC = 0;
        }
        if (fullScreenToggle.isOn)
        {
            settings.FullScreen = 1;
        }
        else
        {
            settings.FullScreen = 0;
        }

        settings.resolutionIndex = dropDownResolution.value;

        Settings.SaveSettings(settings);
    }

    public void OnFpsSliderChanged()
    {
        if (System.Convert.ToInt32(fpsSlider.value) > 144)
        {
            fpsText.text = "UNLIMITED";
        }
        else
        {
            fpsText.text = System.Convert.ToInt32(fpsSlider.value).ToString();
        }
    }

    public void OnVolumeSliderChanged()
    {
        volumeText.text = (volumeSlider.value*100).ToString("0") + "%";
        sfxVolumeText.text = (sfxVolumeSlider.value * 100).ToString("0") + "%";
        musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0") + "%";
        ambientVolumeText.text = (ambientVolumeSlider.value * 100).ToString("0") + "%";
        shotsVolumeText.text = (shotsVolumeSlider.value * 100).ToString("0") + "%";
    }

    public void Start()
    {
        Settings.LoadSetting();

        volumeSlider.value = Settings.settingValues.volume;
        musicVolumeSlider.value = Settings.settingValues.music;
        ambientVolumeSlider.value = Settings.settingValues.ambient;
        sfxVolumeSlider.value = Settings.settingValues.sfx;
        shotsVolumeSlider.value = Settings.settingValues.shots;

        if (Settings.settingValues.FullScreen == 0)
        {
            fullScreenToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        if (Settings.settingValues.VSYNC == 0)
        {
            Debug.Log("VSYNC OFF");
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        Resolution[] resolutions = Screen.resolutions;
        int currentResIndex = 0;

        dropDownResolution.ClearOptions();
        
        List<string> res = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            res.Add(option);

            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
            {
                currentResIndex = i;
            }

        }

        dropDownResolution.AddOptions(res);
        dropDownResolution.value = currentResIndex;
        dropDownResolution.RefreshShownValue();


        fpsSlider.value = Settings.settingValues.fps;
    }
}