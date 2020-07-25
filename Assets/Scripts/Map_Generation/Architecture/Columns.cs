using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Columns : Architecture
{
    int quantity;
    public override direction GetRandom()
    {
        quantity = randomGen.Next(1, 6);  //1..5
        int temp = 9;
        return (direction)temp;
    }

    public override void Create(Room room, direction dir)
    {
        int length = randomGen.Next(3, room.width / 5);
        int depth = randomGen.Next(3, room.height / 5);

        depth += 2;
        length += 2;

        List<Vector2> startPositions = new List<Vector2>();

        if (quantity == 1)
        {
            startPositions.Add(new Vector2(room.start.x + room.width/2 - length/2, room.start.y + room.height / 2 - depth/2));
        }
        else if (quantity == 2)
        {
            int temp = randomGen.Next(0, 2);
            if (temp == 0)
            {
                int offsetX = randomGen.Next(3, room.width / 3);
                int offsetY = randomGen.Next(3, room.height - depth-1);
                
                startPositions.Add(new Vector2(room.start.x + offsetX, room.start.y + offsetY));
                startPositions.Add(new Vector2(room.end.x - offsetX - length, room.start.y + offsetY));
            }
            else
            {
                int offsetX = randomGen.Next(3, room.width - length-1);
                int offsetY = randomGen.Next(3, room.height /3);

                startPositions.Add(new Vector2(room.start.x + offsetX, room.start.y + offsetY));
                startPositions.Add(new Vector2(room.start.x + offsetX, room.end.y - offsetY - depth));
            }
        }
        else if (quantity == 3)
        {
            int temp = randomGen.Next(0, 2);
            if (temp == 0)      //сверху 2 колоны
            {
                int offsetX = room.width / 3;
                int offsetY = randomGen.Next(3, room.height /3);

                startPositions.Add(new Vector2(room.start.x + offsetX, room.end.y - offsetY - depth));
                startPositions.Add(new Vector2(room.start.x + offsetX*2, room.end.y - offsetY - depth));
                startPositions.Add(new Vector2(room.start.x + room.width / 2 - length/2 +1, room.start.y + offsetY));
            }
            else
            {
                int offsetX = room.width / 3;
                int offsetY = randomGen.Next(3, room.height / 3);

                startPositions.Add(new Vector2(room.start.x + offsetX, room.start.y + offsetY));
                startPositions.Add(new Vector2(room.start.x + offsetX * 2, room.start.y + offsetY));
                startPositions.Add(new Vector2(room.start.x + room.width / 2 - length / 2 +1, room.end.y - offsetY -depth));
            }
        }
        else if(quantity == 4 || quantity ==5)
        {
            int offsetX = randomGen.Next(3, room.width/2 - length);
            int offsetY = randomGen.Next(3, room.height / 3);

            startPositions.Add(new Vector2(room.start.x + offsetX, room.start.y + offsetY));
            startPositions.Add(new Vector2(room.end.x - offsetX - length + 1, room.start.y + offsetY));
            startPositions.Add(new Vector2(room.start.x + offsetX, room.end.y - offsetY - depth));
            startPositions.Add(new Vector2(room.end.x - offsetX - length + 1, room.end.y - offsetY - depth));
            if (quantity == 5)
            {
                startPositions.Add(new Vector2(room.start.x + room.width / 2 - length / 2, room.start.y + room.height / 2 - depth / 2));
            }
        }

        foreach (Vector2 start in startPositions)
        {
            pos_remove = new List<Vector3>();
            pos_init = new List<Vector3>();

            pos_now = start;

            for (int i = 0; i <= depth; i++)
            {
                for (int j = 0; j <= length; j++)
                {
                    if (i == 0 || i == depth)
                    {
                        pos_init.Add(pos_now);
                    }
                    else if (j == 0 || j == length)
                    {
                        pos_init.Add(pos_now);
                    }
                    else
                    {
                        pos_remove.Add(pos_now);
                    }
                    pos_now = new Vector2(pos_now.x + right, pos_now.y);
                }
                pos_now = new Vector2(start.x, pos_now.y + up);
            }

            if (CheckDoorCollision(room, pos_init) == false && CheckArchitectureCollision(room, pos_init) == false)
            {
                GenerateArchitecture(room, pos_remove, pos_init, false);
            }
        }
    }

    private void CreateWall(Room room, GameObject[] walls, Vector2 pos, Direction4D direction)
    {
        int tileindex = randomGen.Next(0, walls.Length);
        room.smartGrid.CreateWall(pos, direction);
    }
}