using UnityEngine;
using System.Collections.Generic;
using System;

public enum Direction4D { Up, Down, Left, Right };


/// <summary>
/// Статический класс содержащий информацию о текущей карте
/// </summary>
public static class Map
{
    public static List<Room> roomObjs;

    public static List<Corridor> corridors;

    public static TileSet tiles;

    public static bool mapIsGenerated = false;

    public static Vector2 lowestVector = Vector2.zero;
    public static Vector2 highestVector = Vector2.one;

    public static void Calculate()
    {
        lowestVector = Vector2.zero;
        if (roomObjs.Count > 0)
        {
            highestVector = roomObjs[0].end;
            lowestVector = roomObjs[0].start;
            foreach (Room room in roomObjs)
            {
                if (room.start.x < lowestVector.x)
                {
                    lowestVector.x = room.start.x;
                }
                if (room.start.y < lowestVector.y)
                {
                    lowestVector.y = room.start.y;
                }

                if (room.end.x > highestVector.x)
                {
                    highestVector.x = room.end.x;
                }
                if (room.end.y > highestVector.y)
                {
                    highestVector.y = room.end.y;
                }
            }
        }

    }

    public static void HideMap(bool hide)
    {
        foreach(Room r in roomObjs)
        {
            r.HideFromMap(hide);
        }
        foreach (Corridor c in corridors)
        {
            c.HideFromMap(hide);
        }
    }
}

public class WallType
{
    public enum Direction { Up, Down, Left, Right, LeftCorner, RightCorner };
}

[Serializable]
public struct MapType
{
    [SerializeField]
    public string name;
    [SerializeField]
    public TileSet tileSet;
}