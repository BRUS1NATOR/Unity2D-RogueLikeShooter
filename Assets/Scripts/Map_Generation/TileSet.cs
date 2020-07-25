using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct RandomTile
{
    public GameObject tile;
    public float chance;
}

public enum TileType { floor, wall}

[CreateAssetMenu]
public class TileSet : ScriptableObject
{
    public Environment_Generation environment_gen;
    //Объекты на карте

    public RandomTiles floortiles;

    public GameObject[] walltiles;
    public GameObject[] downWallTiles;

    public GameObject[] leftWallTiles;
    public GameObject[] leftLongWallTiles;

    [HideInInspector]
    public GameObject[] rightWallTiles;
    [HideInInspector]
    public GameObject[] rightLongWallTiles;

    public GameObject[] pitTilesUp;
    public GameObject[] pitTilesUpLeft;
    public GameObject[] pitTilesUpRight;

    public GameObject[] waterTiles;
    public GameObject[] waterTilesDown;
    public GameObject[] waterTilesRight;
    public GameObject[] waterTilesLeft;
    public GameObject[] waterTilesDownLeft;
    public GameObject[] waterTilesDownRight;

    public GameObject door_upper;
    public GameObject door_right;
    public GameObject door_left;
    public GameObject door_bottom;

    public void Init()
    {
        if (rightLongWallTiles != null)
        {
            for (int i = 0; i < rightLongWallTiles.Length; i++)
            {
                Destroy(rightLongWallTiles[i]);
            }
        }
        if (rightWallTiles != null)
        {
            for (int i = 0; i < rightWallTiles.Length; i++)
            {
                Destroy(rightWallTiles[i]);
            }
        }

        rightWallTiles = new GameObject[leftWallTiles.Length];
        for (int i = 0; i < leftWallTiles.Length; i++)
        {
            rightWallTiles[i] = Instantiate(leftWallTiles[i]);
            rightWallTiles[i].name = "walltile_right" + i;
            rightWallTiles[i].transform.Rotate(0, 0, 180);
        }

        rightLongWallTiles = new GameObject[leftLongWallTiles.Length];
        for (int i = 0; i < leftLongWallTiles.Length; i++)
        {
            rightLongWallTiles[i] = Instantiate(leftLongWallTiles[i]);
            rightLongWallTiles[i].name = "long_right" + i;
            Vector3 theScale = rightLongWallTiles[i].transform.localScale;
            theScale.x *= -1;
            rightLongWallTiles[i].transform.localScale = theScale;
        }
    }

    public GameObject GetFloorTile()
    {
        return floortiles.GetRandomTile();
    }
}
