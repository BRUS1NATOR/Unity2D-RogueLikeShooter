using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomTiles : ScriptableObject
{
    public List<RandomTile> tiles;

    public GameObject GetRandomTile()
    {
        float overall = 0;
        foreach (RandomTile tile in tiles)
        {
            overall += tile.chance;
        }

        float rand = UnityEngine.Random.Range(0f, overall);
        float temp = 0f;

        foreach (RandomTile tile in tiles)
        {
            if (rand < tile.chance + temp)
            {
                if (tile.tile != null)
                {
                    return tile.tile;
                }
            }
            temp += tile.chance;
        }

        return null;
    }
}
