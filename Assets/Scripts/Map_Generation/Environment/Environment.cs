using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Environment
{
    public static void CreateShop(Room room, GameObject shopToLeft, GameObject shopToRight)
    {
        bool created = Environment.CreateShopToLeft(room, shopToLeft);
        if (created == false)
        {
            created = Environment.CreateShopToRight(room, shopToRight);
        }
    }


    private static bool CreateShopToLeft(Room room, GameObject shop)
    {
        Vector3 position = new Vector3(room.start.x + 1,room.end.y - 4,0);
        Vector3 endposition = new Vector3(position.x + 4, position.y + 4);
        GameObject s = room.CreateEnvironmentObject(shop, position);

        foreach (GameObject d in room.Doors)    
        {   
            if (SimpleFunctions.Check_Superimpose(position, endposition, d.transform.position, d.transform.position, 2))
            {
                GameObject.Destroy(s);
                return false;
            }
        }

        Transform[] shopsEnv = s.GetComponentsInChildren<Transform>();
        foreach(Transform t in shopsEnv)
        {
            room.EnvironmentTiles.Add(new EnvironmentTile(t.gameObject, room));
        }

        return true;    //Если ок
    }

    private static bool CreateShopToRight(Room room, GameObject shop)
    {
        Vector3 position = new Vector3(room.end.x - 5, room.end.y - 4, 0);
        Vector3 endposition = new Vector3(position.x + 4, position.y + 4);
        GameObject s = room.CreateEnvironmentObject(shop, position);

        foreach (GameObject d in room.Doors)
        {
            if (SimpleFunctions.Check_Superimpose(position, endposition, d.transform.position, d.transform.position, 2))
            {
                GameObject.Destroy(s);
                return false;
            }
        }

        Transform[] shopsEnv = s.GetComponentsInChildren<Transform>();
        foreach (Transform t in shopsEnv)
        {
            room.EnvironmentTiles.Add(new EnvironmentTile(t.gameObject, room));
        }

        return true;
    }

    public static void PaintRoomOutSide(Room room, Color color)
    {
        foreach (GameObject tile in room.smartGrid.floorTilesOutSide)
        {
            tile.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public static void PaintLine(Room room, bool axisX, Color color)
    {
        if (axisX == false)
        {
            int Y = System.Convert.ToInt32(Random.Range(room.start.y, room.start.y + room.height));

            foreach (GameObject tile in room.smartGrid.floorTilesInside)
            {
                if (tile.transform.position.y == Y)
                {
                    tile.GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
        else
        {
            int X = System.Convert.ToInt32(Random.Range(room.start.x, room.start.x + room.width));

            foreach (GameObject tile in room.smartGrid.floorTilesInside)
            {
                if (tile.transform.position.x == X)
                {
                    tile.GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
    }

    public static void SpawnChest(Room room, GameObject chest, GameObject itemInside,Vector2 position)
    {
        chest.GetComponent<Chest>().itemInside = itemInside;

        GameObject c = Object.Instantiate(chest, position, Quaternion.identity) as GameObject;

        c.transform.parent = room.grid.transform;
    }

    public static void SpawnChest(Room room, GameObject chest, Vector2 position)
    {
        GameObject c = Object.Instantiate(chest, position, Quaternion.identity) as GameObject;

        c.transform.parent = room.grid.transform;
    }
}
