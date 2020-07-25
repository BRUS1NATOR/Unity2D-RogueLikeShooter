using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas menuCanvas;
    public Text mapNameText;
    public InputField field;
    public GameObject gameSettings;

    int id = 0;
    int mapGeneratedId = 0;

    public void Show(bool show)
    {
        if (show)
        {
            menuCanvas.enabled = true;
        }
        else
        {
            menuCanvas.enabled = false;
        }
    }


    public void ShowGameSettings()
    {
        if (gameSettings.activeSelf == false)
        {
            gameSettings.SetActive(true);
        }
        else
        {
            gameSettings.SetActive(false);
        }
    }

    public void SetSeed()
    {
        int seed = 0;
        try
        {
            seed = System.Convert.ToInt32(field.text);
        }
        catch { }
        GameManager.instance.SetGenerationSeed(seed);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Start()
    {
        mapNameText.text = GameManager.instance.generator.mapTypes[GameManager.instance.mapTypeIdNow].name;
    }

    public void ChangeMapType(int step)
    {
        id = GameManager.instance.mapTypeIdNow;

        id += step;
        if (id < GameManager.instance.generator.mapTypes.Count)
        {
            if (id < 0)
            {
                id = GameManager.instance.generator.mapTypes.Count - 1;
            }
        }
        else
        {
            id = 0;
        }

        GameManager.instance.mapTypeIdNow = id;
        Map.tiles = GameManager.instance.generator.mapTypes[GameManager.instance.mapTypeIdNow].tileSet;
        mapNameText.text = GameManager.instance.generator.mapTypes[GameManager.instance.mapTypeIdNow].name;

        GameManager.instance.SaveGame();

        GenerateMap(GameManager.instance.mapTypeIdNow); 
    }

    private void GenerateMap(int Id)
    {
        Camera.main.GetComponent<Camera_Bounds>().fadeImage.color = new Color(0, 0, 0, 1f);

        GameManager.instance.StopAll();

        GameManager.instance.GenerateLevel();

        mapGeneratedId = Id;
        if (mapGeneratedId != GameManager.instance.mapTypeIdNow)  //Если айди поменялся
        {
            GenerateMap(GameManager.instance.mapTypeIdNow);
        }
    }

}
