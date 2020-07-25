using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour {

    public Image background;
    public Canvas PauseCanvas;

    private bool paused = false;

	// Use this for initialization

    public void Show(bool show)
    {
        if (show)
        {
            PauseCanvas.enabled = true;
        }
        else
        {
            PauseCanvas.enabled = false;
        }
    }

    public void PauseOrResume()
    {
        if (paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        paused = false;

        GameManager.instance.audioManager.UnPauseLiveSources();
        background.color = new Color(0, 0, 0, 0);
        PauseCanvas.enabled = false;
        Time.timeScale = GameManager.instance.gameTime;
    }

    public void Pause()
    {
        paused = true;

        GameManager.instance.audioManager.PauseLiveSources();
        background.color = new Color(0, 0, 0, 0.5f);
        PauseCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        PauseCanvas.enabled = false;
        GameManager.instance.loadManager.LoadGame();
    }

    public void Quit()
    {
        PauseCanvas.enabled = false;
        Application.Quit();      
    }
}
