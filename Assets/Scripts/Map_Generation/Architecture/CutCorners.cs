using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCorners : Architecture
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
        pos_init = new List<Vector3>();

        int length = randomGen.Next(room.height / 4, room.height/2);

        Vector2 end = new Vector2(room.end.x, room.end.y);
        Vector2 start = new Vector2(room.start.x, room.start.y);

        Vector2 startCorner = Vector2.zero;

        switch (dir)
        {
            case direction.DownLeft:
                startCorner = new Vector2(start.x + 1, start.y + 1);
                break;
            case direction.DownRight:
                startCorner = new Vector2(end.x-1, start.y + 1);
                right = -right;
                break;
            case direction.UpLeft:
                startCorner = new Vector2(start.x + 1, end.y - 1);
                up = -up;
                break;
            case direction.UpRight:
                startCorner = new Vector2(end.x-1, end.y - 1);
                up = -up;
                right = -right;
                break;
        }
        if (dir != direction.None)
        {
            int increase = 0;
            pos_now = startCorner;

            for (int i = 0; i <= length; i++)
            {
                for (int j = 0; j <= length; j++)
                {
                    if (j < length-increase)
                    {
                        pos_remove.Add(pos_now);
                    }
                    else
                    {
                        pos_init.Add(pos_now);
                        break;
                    }
                    pos_now = new Vector2(pos_now.x + right, pos_now.y);
                }
                pos_now = new Vector2(startCorner.x, pos_now.y + up);
                increase++;
            }

            if (CheckDoorCollision(room, pos_remove) == false && CheckArchitectureCollision(room, pos_remove) == false)
            {
                GenerateArchitecture(room, pos_remove, pos_init,false);
            }
        }

        
    }
}
