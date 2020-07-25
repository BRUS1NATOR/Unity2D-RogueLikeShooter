using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : Architecture
{
    int quantity;
    public override direction GetRandom()
    {
        quantity = randomGen.Next(1, 3);
        int temp = randomGen.Next(8, 10);
        return (direction)temp;
    }

    public override void Create(Room room, direction dir)
    {

        int length = randomGen.Next(4, room.width / 3);
        int depth = randomGen.Next(4, room.height / 5);

        List<Vector2> startPositions = new List<Vector2>();

        if (quantity == 1)
        {
            startPositions.Add(new Vector2(room.start.x + room.width / 2 - length / 2, room.start.y + room.height / 2 - depth / 2));
        }
        else if (quantity == 2)
        {
            int temp = randomGen.Next(0, 2);
            if (temp == 0)
            {
                int offsetX = randomGen.Next(3, room.width / 3);
                int offsetY = randomGen.Next(3, room.height - depth - 1);

                startPositions.Add(new Vector2(room.start.x + offsetX, room.start.y + offsetY));
                startPositions.Add(new Vector2(room.end.x - offsetX - length, room.start.y + offsetY));
            }
            else
            {
                int offsetX = randomGen.Next(3, room.width - length - 1);
                int offsetY = randomGen.Next(3, room.height / 3);

                startPositions.Add(new Vector2(room.start.x + offsetX, room.start.y + offsetY));
                startPositions.Add(new Vector2(room.start.x + offsetX, room.end.y - offsetY - depth));
            }
        }

        foreach (Vector2 start in startPositions)
        {
            pos_remove = new List<Vector3>();
            pos_init = new List<Vector3>();

            pos_now = start;

            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    pos_remove.Add(pos_now);
                    
                    pos_now = new Vector2(pos_now.x + right, pos_now.y);
                }
                pos_now = new Vector2(start.x, pos_now.y + up);
            }

            if (CheckDoorCollision(room, pos_remove) == false && CheckArchitectureCollision(room, pos_remove) == false)
            {
                foreach (Vector2 v in pos_remove)
                {
                    RemoveFloor(room, v);
                    room.notRemovable.Add(v);
                }

                GameObject pool = new GameObject();
                pool.name = "PoolCollider";

                BoxCollider2D poolColl = pool.AddComponent<BoxCollider2D>();

                if (length % 2 != 0)
                {
                    poolColl.offset = new Vector2(start.x - room.start.x + length / 2, start.y - room.start.y + depth / 2 - 0.25f);
                }
                else
                {
                    poolColl.offset = new Vector2(start.x - room.start.x + length / 2 - 0.5f, start.y - room.start.y + depth / 2 - 0.25f);
                }
                poolColl.size = new Vector2(length, depth - 0.5f);

                pool.transform.position = room.gameObject.transform.position;
                pool.transform.parent = room.gameObject.transform;

                CreateWalls(room, length, depth);
            }
        }
    }

    private void CreateWalls(Room room, int length, int depth)
    {
        Vector2 start = pos_remove[0];
        Vector2 pos = start;

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < length; j++)
            {
                {
                    //if (i == depth - 1)// Сверху
                    //{
                    //    if (j == 0)
                    //    {
                    //        room.CreateTile(Map.tiles.pitTilesUpLeft, null, pos, FloorType.Environment);
                    //    }
                    //    else if (j == length - 1)
                    //    {
                    //        room.CreateTile(Map.tiles.pitTilesUpRight, null, pos, FloorType.Environment);
                    //    }
                    //    else
                    //    {
                    //        room.CreateTile(Map.tiles.pitTilesUp, null, pos, FloorType.Environment);
                    //    }
                    //}

                    //else if (i == 0)
                    //{
                    //    if (j == 0)
                    //    {
                    //        room.CreateTile(Map.tiles.waterTilesDownLeft, null, pos, FloorType.Environment);
                    //    }
                    //    else if (j == length - 1)
                    //    {
                    //        room.CreateTile(Map.tiles.waterTilesDownRight, null, pos, FloorType.Environment);
                    //    }
                    //    else
                    //    {
                    //        room.CreateTile(Map.tiles.waterTilesDown, null, pos, FloorType.Environment);
                    //    }
                    //}
                    //else
                    //{
                    //    if (j == 0)
                    //    {
                    //        room.CreateTile(Map.tiles.waterTilesLeft, null, pos, FloorType.Environment);
                    //    }
                    //    else if (j == length - 1)
                    //    {
                    //        room.CreateTile(Map.tiles.waterTilesRight, null, pos, FloorType.Environment);
                    //    }
                    //    else
                    //    {
                    //        room.CreateTile(Map.tiles.waterTiles, null, pos, FloorType.Environment);
                    //    }
                    //}
                }

                room.notRemovable.Add(pos);

                pos = new Vector2(pos.x + right, pos.y);
            }
            pos = new Vector2(start.x, pos.y + up);
        }
    }
}