using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static SettingsValues settingValues;

    public static void SaveSettings(SettingsValues values)
    {
        settingValues = values;

        PlayerPrefs.SetFloat("volume", settingValues.volume);
        PlayerPrefs.SetFloat("musicVolume", settingValues.ambient);
        PlayerPrefs.SetFloat("ambientVolume", settingValues.ambient);
        PlayerPrefs.SetFloat("sfxVolume", settingValues.ambient);
        PlayerPrefs.SetFloat("shotsVolume", settingValues.ambient);

        PlayerPrefs.SetInt("fullScreen", settingValues.FullScreen);
        PlayerPrefs.SetInt("resolutionIndex", settingValues.resolutionIndex);
        PlayerPrefs.SetInt("fpsMax", settingValues.fps);
        PlayerPrefs.SetInt("vSync", settingValues.VSYNC);

        PlayerPrefs.Save();
    }

    public static void LoadSetting()
    {
        Debug.Log("LOADING SETTINGS");
        settingValues.volume = PlayerPrefs.GetFloat("volume");
        settingValues.music = PlayerPrefs.GetFloat("musicVolume");
        settingValues.ambient = PlayerPrefs.GetFloat("ambientVolume");
        settingValues.sfx = PlayerPrefs.GetFloat("sfxVolume");
        settingValues.shots = PlayerPrefs.GetFloat("shotsVolume");

        settingValues.FullScreen = PlayerPrefs.GetInt("fullScreen");
        settingValues.resolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
        settingValues.fps = PlayerPrefs.GetInt("fpsMax");
        settingValues.VSYNC = PlayerPrefs.GetInt("vSync");
    }

    public static void SetResolution(int index, int fullScreen)
    {
        Resolution resolution = Screen.resolutions[index];

        if (fullScreen == 1)
        {
            Screen.SetResolution(resolution.width, resolution.height, true);
        }
        else
        {
            Screen.SetResolution(resolution.width, resolution.height, false);
        }
    }
}

public struct SettingsValues
{
    private float _volume;
    public float volume
    {
        get
        {
            if (_shots <= 0)
            {
                return 0.4f;
            }
            return _volume;
        }
        set
        {
            if(value <= 0)
            {
                value = 0.4f;
            }
            _volume = Mathf.Log10(value) * 20;
            GameManager.instance.audioManager.mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        }
    }

    private float _ambient;
    public float ambient
    {
        get
        {
            if (_shots <= 0)
            {
                return 0.2f;
            }
            return _ambient;
        }
        set
        {
            value -= 0.2f;
            if (value <= 0)
            {
                value = 0.2f;
            }
            _ambient = Mathf.Log10(value) * 20;
            GameManager.instance.audioManager.mixer.SetFloat("ambientVolume", Mathf.Log10(value) * 20);
        }
    }

    private float _music;
    public float music
    {
        get
        {
            if (_shots <= 0)
            {
                return 0.2f;
            }
            return _music;
        }
        set
        {
            value -= 0.2f;
            if (value <= 0)
            {
                value = 0.2f;
            }
            _music = Mathf.Log10(value) * 20;
            GameManager.instance.audioManager.mixer.SetFloat("musicVolume", Mathf.Log10(value) * 20);
        }
    }

    private float _sfx;
    public float sfx
    {
        get
        {
            if (_shots <= 0)
            {
                return 0.4f;
            }
            return _sfx;
        }
        set
        {
            if (value <= 0)
            {
                value = 0.4f;
            }
            _sfx = Mathf.Log10(value) * 20;
            GameManager.instance.audioManager.mixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20);
        }
    }

    private float _shots;
    public float shots
    {
        get
        {
            if (_shots <= 0)
            {
                return 0.5f;
            }
            return _shots;
        }
        set
        {
            if (value <= 0)
            {
                value = 0.5f;
            }
            _shots = Mathf.Log10(value) * 20;
            GameManager.instance.audioManager.mixer.SetFloat("shotsVolume", Mathf.Log10(value) * 20);
        }
    }

    private int fpsMax;
    public int fps
    {
        get
        {
            return fpsMax;
        }
        set
        {
            if (value < 24)
            {
                fpsMax = 60;
                Application.targetFrameRate = fpsMax;
            }
            else if(value > 144)
            {
                fpsMax = 999;
                Application.targetFrameRate = 999;
            }
            else
            {
                fpsMax = value;
                Application.targetFrameRate = value;
            }
        }
    }


    private int vSync;
    public int VSYNC
    {
        get
        {
            return vSync;
        }
        set
        {
            if (value == 0)
            {
                vSync = 0;
                QualitySettings.vSyncCount = 0;
            }
            else
            {
                vSync = 1;
                QualitySettings.vSyncCount = 1;
            }
        }
    }

    private int fullScreen;
    public int FullScreen
    {
        get
        {
            return fullScreen;
        }
        set
        {
            if (value == 0)
            {
                fullScreen = 0;
            }
            else
            {
                fullScreen = 1;
            }
           
        }
    }

    private int resolutionindex;
    public int resolutionIndex
    {
        get
        {
            return resolutionindex;
        }
        set
        {
            resolutionindex = value;
            Settings.SetResolution(resolutionindex, fullScreen);
        }
    }
}

