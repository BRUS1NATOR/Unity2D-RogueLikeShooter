using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame_UI : MonoBehaviour
{
    public Canvas canvas;
    public Text enemiesDefeated;
    public Text roomsCount;
    public Text shotsFired;
    public Text timePlayed;
    // Start is called before the first frame update

    public void Show()
    {
        enemiesDefeated.text = "Enemies defeated: " + Player.instance.gameStatistic.EnemiesDefeatedCount;
        roomsCount.text = "Rooms: " + Player.instance.gameStatistic.RoomsCount;
        shotsFired.text = "Shots fired: " + Player.instance.gameStatistic.ShotsBeenFired;

        int minutes = 0;
        float seconds = 0;
        float overalltime = Player.instance.gameStatistic.timePlayed;
        while (overalltime > 59.9)
        {
            overalltime -= 60;
            minutes += 1;
        }
        seconds = overalltime;

        timePlayed.text = "Play time: " + minutes + ":" + System.Math.Round(seconds, 2);
        canvas.enabled = true;
    }

    public void MainMenu()
    {
        canvas.enabled = false;
        GameManager.instance.overallUI.GoToMainMenu();
    }

    public void Restart()
    {
        canvas.enabled = false;
        GameManager.instance.loadManager.RestartGame();
    }

    public void Quit()
    {
        canvas.enabled = false;
        GameManager.instance.overallUI.mainMenuUI.Quit();
    }
}
