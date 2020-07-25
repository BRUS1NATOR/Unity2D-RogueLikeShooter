using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRooms : Architecture
{
    public override direction GetRandom()
    {
        int temp = randomGen.Next(4, 9);
        if (temp == 8)
        {
            return direction.None;
        }
        return (direction)temp;
    }

    public override void Create(Room room, direction dir)
    {
        pos_remove = new List<Vector3>();
        List<Vector3> floorInside = new List<Vector3>();
        pos_init = new List<Vector3>();

        int length = randomGen.Next(7, System.Convert.ToInt32(room.width / 1.5f));
        int depth = randomGen.Next(7, System.Convert.ToInt32(room.height / 1.5f));
        int door = randomGen.Next(4, length);

        Vector2 end = room.end;
        Vector2 start = room.start;      //Начинаем с небольшим отступом для проверки 
        //колизии с дверью

        Vector2 startCorner = Vector2.zero;
        switch (dir)
        {
            case direction.DownLeft:
                startCorner = new Vector2(start.x+1, start.y+1);
                break;
            case direction.DownRight:
                right = -right;
                startCorner = new Vector2(end.x-1, start.y + 1);
                break;
            case direction.UpRight:
                startCorner = new Vector2(end.x-1, end.y - 1);
                right = -right;
                up = -up;
                break;
            case direction.UpLeft:
                startCorner = new Vector2(start.x+1, end.y - 1);
                up = -up;
                break;
        }

        if (dir != direction.None)
        {
            
            pos_now = startCorner;

            for (int i = 0; i <= depth; i++)
            {
                for (int j = 0; j <= length; j++)
                {
                    if (i < depth - 3)
                    {
                        if (j > length - 3 && j != length)
                        {
                            pos_remove.Add(pos_now);
                        }
                        else if (j == 0 || j == length - 3 || j == length)
                        {
                            pos_init.Add(pos_now);
                        }
                        else
                        {
                            if (i == depth - 4)
                            {
                                pos_init.Add(pos_now);
                            }
                            else if (i > 0)
                            {
                                floorInside.Add(pos_now);
                            }
                        }

                    }

                    else if (i == depth)
                    {
                        pos_init.Add(pos_now);
                    }

                    else if (i > depth-4)
                    {
                        pos_remove.Add(pos_now);
                        if (j == length)
                        {
                            pos_init.Add(pos_now);
                        }
                        if (j == length - door - 1 || j == length - door)
                        {
                            pos_init.Add(pos_now);
                        }
                    }
                    pos_now = new Vector2(pos_now.x + right, pos_now.y);
                }
                pos_now = new Vector2(startCorner.x, pos_now.y + up);
            }

            if (CheckDoorCollision(room, pos_remove) == false && CheckArchitectureCollision(room, floorInside) == false && CheckArchitectureCollision(room, pos_remove) == false)
            {
                GenerateArchitecture(room, pos_remove, pos_init, false);

                foreach (Vector2 v in floorInside)
                {
                    room.notRemovable.Add(v);
                    room.CreateTile( v, FloorType.inside);     //Можно сделать ковер ?_?   //Создаем внутренние тайлы
                }
            }
        }
    }
}