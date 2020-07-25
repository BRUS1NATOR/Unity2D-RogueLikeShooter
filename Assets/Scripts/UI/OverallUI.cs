using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallUI : MonoBehaviour
{
    public Canvas settingsUI;

    public MainMenu mainMenuUI;
    public Pause_Menu pauseUI;
    public EndGame_UI endGameUI;  

    void Start()
    {
        TurnOffMenu();
        if (GameManager.instance.sceneNow != SceneNow.Game)
        {
            TurnOnMenu();
        }
    }

    private void Update()
    {
        if (GameManager.instance.Loading != true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Player.instance != null)
                {
                    if (Player.instance.mapCamera != null)
                    {
                        if (Player.instance.mapCamera.bigMap) // Open MiniMap
                        {
                            Player.instance.mapCamera.TurnOffBigMap(true);
                            return;
                        }
                    }
                }
                if (settingsUI.enabled == false)
                {
                    if (Player.instance != null)
                    {
                        if (Player.instance.isAlive)
                        {
                            pauseUI.PauseOrResume();
                        }
                    }
                }
                else
                {
                    TurnOnMenu();
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (Player.instance != null)
                {
                    if (Player.instance.mapCamera != null)
                    {
                        if (Player.instance.mapCamera.bigMap)
                        {
                            Player.instance.mapCamera.TurnOffBigMap(true);
                            return;
                        }
                        if (settingsUI.enabled == false && pauseUI.PauseCanvas.enabled == false)    // Open BigMap
                        {
                            Player.instance.mapCamera.TurnOnBigMap(null);
                        }
                    }
                }
            }
        }
    }

    public void GoToSettingsMenu()
    {
        TurnOffMenu();
        settingsUI.enabled = true;
    }

    public void TurnOffMenu()
    {
        mainMenuUI.Show(false);
        pauseUI.Show(false);
    }

    public void TurnOnMenu()
    {
        switch (GameManager.instance.sceneNow)
        {
            case SceneNow.Game:
                {
                    settingsUI.enabled = false;
                    mainMenuUI.Show(false);
                    pauseUI.Show(true);
                    pauseUI.Pause();
                    break;
                }
            case SceneNow.MainMenu:
                {
                    settingsUI.enabled = false;
                    mainMenuUI.Show(true);
                    break;
                }
        }
    }

    public void NewGame()
    {
        mainMenuUI.field.text = "";
        mainMenuUI.gameSettings.SetActive(false);
        TurnOffMenu();
        GameManager.instance.loadManager.LoadGame();
    }

    public void GoToMainMenu()
    {
        TurnOffMenu();
        GameManager.instance.loadManager.LoadMainMenu();
        TurnOnMenu();
    }

    public void EndGame()
    {
        endGameUI.Show();
    }
}
