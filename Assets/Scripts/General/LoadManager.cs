using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public void NextLevel()
    {
        GameManager.instance.level++;
        if (GameManager.instance.generator.RoomsNow < GameManager.instance.generator.maxRooms)
        {
            GameManager.instance.generator.RoomsNow += UnityEngine.Random.Range(GameManager.instance.generator.roomIncreaseMin, GameManager.instance.generator.roomIncreaseMax);
        }

        Debug.Log("ITEM PHASE: " + ItemManager.instance.phaseNow);
        GameManager.instance.NextLevel();
    }

    public void LoadMainMenu()
    {
        GameManager.instance.sceneNow = SceneNow.MainMenu;
        Camera.main.GetComponent<Camera_Bounds>().fadeImage.color = new Color(0, 0, 0, 1f);

        DestroyPlayScene();
        StartCoroutine(LoadScene("Main Menu"));
    }

    public void RestartGame()
    {
        DestroyPlayScene();
        StartCoroutine(LoadScene("Play_Scene"));
    }
    public void LoadGame()
    {
        StartCoroutine(LoadScene("Play_Scene"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        GameManager.instance.StopAll();

        AsyncOperation wait = SceneManager.LoadSceneAsync(sceneName);
        yield return wait;

        if (sceneName == "Play_Scene")
        {
            GameManager.instance.sceneNow = SceneNow.Game;
        }
        else
        {
            GameManager.instance.sceneNow = SceneNow.MainMenu;
            GameManager.instance.SetGenerationSeed(0);
        }
        GameManager.instance.GenerateLevel();
    }

    private void DestroyPlayScene()
    {
        if (Player.instance != null)
        {
            GameObject.Destroy(Player.instance.gameObject);
        }
    }
}