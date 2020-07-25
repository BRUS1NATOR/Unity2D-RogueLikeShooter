using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static PlayerData data;
    public static void SaveData(PlayerData values)
    {
        data = values;
        PlayerPrefs.SetInt("Score", values.score);
        PlayerPrefs.SetInt("MapType", values.mapTypeIdNow);
        PlayerPrefs.Save();
    }

    public static void LoadData()
    {
        data.score = PlayerPrefs.GetInt("Score");
        data.mapTypeIdNow = PlayerPrefs.GetInt("MapType");
    }
}

public struct PlayerData
{
    public int score;

    public int mapTypeIdNow;
}