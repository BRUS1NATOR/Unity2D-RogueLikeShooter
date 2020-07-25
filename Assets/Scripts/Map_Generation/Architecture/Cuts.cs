using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuts : Architecture
{
    public override direction GetRandom()
    {
        return (direction)1;
    }
    public override void Create(Room room, direction dir)
    {
        pos_remove = new List<Vector3>();
        pos_init = new List<Vector3>();


        int quantity = randomGen.Next(0, 5);
        for (int i = 0; i < quantity; i++)    //снизу
        {
            int rand = randomGen.Next(1, room.width);

            Vector3 pos = new Vector3(rand + room.start.x, 1 + room.start.y, 0);       // x rand y = 1

            pos_remove.Add(pos);
            pos_init.Add(new Vector3(pos.x, pos.y + 1, 0));
            pos_remove.Add(new Vector3(pos.x + 1, pos.y, 0));
            pos_init.Add(new Vector3(pos.x + 1, pos.y + 1, 0));

            bool ok = !CheckDoorCollision(room, pos_remove);
            foreach (Vector3 floortoinit in pos_init)
            {
                if (!room.smartGrid.CheckFloorCollision(floortoinit))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                GenerateArchitecture(room, pos_remove, pos_init, true);
            }

            pos_init.Clear();
            pos_remove.Clear();
        }

        quantity = randomGen.Next(0, 5);
        for (int i = 0; i < quantity; i++)    //сверху
        {
            int rand = randomGen.Next(1, room.width);

            Vector3 pos = new Vector3(rand + room.start.x, room.end.y -1, 0);       // x rand y = 1

            pos_remove.Add(pos);
            pos_init.Add(new Vector3(pos.x, pos.y - 1, 0));
            pos_remove.Add(new Vector3(pos.x + 1, pos.y, 0));
            pos_init.Add(new Vector3(pos.x + 1, pos.y - 1, 0));

            bool ok = !CheckDoorCollision(room, pos_remove);
            foreach (Vector3 floortoinit in pos_init)
            {
                if (!room.smartGrid.CheckFloorCollision(floortoinit))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                GenerateArchitecture(room, pos_remove, pos_init, true);
            }

            pos_init.Clear();
            pos_remove.Clear();
        }

        quantity = randomGen.Next(0, 5);
        for (int i = 0; i < quantity; i++)    //слева
        {
            int rand = randomGen.Next(2, room.height-1);

            Vector3 pos = new Vector3(room.start.x+1, room.start.y + rand, 0);       // x rand y = 1

            pos_remove.Add(pos);
            pos_init.Add(new Vector3(pos.x+1, pos.y, 0));
            pos_remove.Add(new Vector3(pos.x, pos.y-1, 0));
            pos_init.Add(new Vector3(pos.x + 1, pos.y - 1, 0));
            pos_remove.Add(new Vector3(pos.x, pos.y - 2, 0));
            pos_init.Add(new Vector3(pos.x + 1, pos.y - 2, 0));

            bool ok = !CheckDoorCollision(room, pos_remove);
            foreach (Vector3 floortoinit in pos_init)
            {
                if (!room.smartGrid.CheckFloorCollision(floortoinit))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                GenerateArchitecture(room, pos_remove, pos_init, true);
            }

            pos_init.Clear();
            pos_remove.Clear();
        }

        quantity = randomGen.Next(0, 5);
        for (int i = 0; i < quantity; i++)    //справа
        {
            int rand = randomGen.Next(2, room.height-1);

            Vector3 pos = new Vector3(room.end.x-1, room.start.y + rand, 0);       // x rand y = 1

            pos_remove.Add(pos);
            pos_init.Add(new Vector3(pos.x - 1, pos.y, 0));
            pos_remove.Add(new Vector3(pos.x, pos.y - 1, 0));
            pos_init.Add(new Vector3(pos.x - 1, pos.y - 1, 0));
            pos_remove.Add(new Vector3(pos.x, pos.y - 2, 0));
            pos_init.Add(new Vector3(pos.x - 1, pos.y - 2, 0));

            bool ok = !CheckDoorCollision(room, pos_remove);
            foreach (Vector3 floortoinit in pos_init)
            {
                if (!room.smartGrid.CheckFloorCollision(floortoinit))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                GenerateArchitecture(room, pos_remove, pos_init, true);
            }

            pos_init.Clear();
            pos_remove.Clear();
        }
    }
}
