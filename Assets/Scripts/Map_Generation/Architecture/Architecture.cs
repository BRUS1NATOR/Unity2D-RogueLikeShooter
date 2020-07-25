using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Architecture
{
    public System.Random randomGen;
    protected int right = 1;
    protected int up = 1;

    protected List<Vector3> pos_remove;
    protected List<Vector3> pos_init;

    protected Vector2 pos_now;
    public enum direction {Up,Down,Right,Left,DownRight,DownLeft,UpRight,UpLeft,Inside, None};
    protected direction dir;

    public GameObject[] wall;

    public abstract void Create(Room room, direction dir);
    public abstract direction GetRandom();


    public bool RemoveFloor(Room room, Vector3 position)
    {
        bool removed = room.smartGrid.RemoveFloor(position);

        return removed;
    }

    public bool CheckArchitectureCollision(Room room, List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            foreach (Vector3 floor in room.notRemovable)
            {
                if (floor == position)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CheckDoorCollision(Room room, List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            foreach (CorridorForRoom cor in room.corridors)
            {
                Corridor c = cor.corridor;
                foreach (GameObject door in c.Doors)
                {
                    if (door.transform.position == position || 
                        new Vector3(door.transform.position.x + 1, door.transform.position.y) == position ||
                         new Vector3(door.transform.position.x - 1, door.transform.position.y) == position||
                          new Vector3(door.transform.position.x, door.transform.position.y + 1) == position ||
                          new Vector3(door.transform.position.x, door.transform.position.y - 1) == position)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public void GenerateArchitecture(Room room, List<Vector3> positionsToRemove, List<Vector3> positionsToInitOutSide, bool overridable)
    {
        foreach (Vector3 v in positionsToInitOutSide)
        {
            RemoveFloor(room, v);
        }

        switch (overridable)
        {
            case true:
                foreach (Vector3 v in positionsToRemove)
                {
                    RemoveFloor(room, v);
                }
                break;
            case false:
                foreach (Vector3 v in positionsToRemove)
                {
                    RemoveFloor(room, v);
                    room.notRemovable.Add(v);
                }
                break;
        }

        foreach (Vector3 v in positionsToInitOutSide)  //Создаем внешние тайлы(для генерации стен)
        {
            room.CreateTile(v, FloorType.outside);
        }
    }
}