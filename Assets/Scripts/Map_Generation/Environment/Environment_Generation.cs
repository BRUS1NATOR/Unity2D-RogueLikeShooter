using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct onWallEnvironment
{
    public List<GameObject> wideWallObjects;
    public List<GameObject> wallObjects;
}

[CreateAssetMenu]
public class Environment_Generation : ScriptableObject {

    public IconSet icons;
    public Color paintColor;

    public Teleport teleport;
    public GameObject shopToLeft;
    public GameObject shopToRight;

    public onWallEnvironment onWallEnvironment;

    public GameObject torch;
    public List<GameObject> wallModificators;
    public GameObject chest;

    [Header("Максимальное количество разных объектов в комнате")]
    public int maxUnderWallObjects;
    public GameObject[] underWallsObjects;
    [Header("Максимальное количество разных объектов в комнате")]
    public int maxSideWallObjects;
    public GameObject[] sideWallObjects;

    public GameObject table;

    public GameObject exit;

    public IEnumerator Create() {
        foreach (Room room in Map.roomObjs)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            Environment.PaintRoomOutSide(room, paintColor);

            CreatePaintings(room, 0.2f);
            CreateWallModifications(room, 0.25f);

            switch (room.roomType)
            {
                case roomType.common:
                    {
                        CreateUnderWallDecoration(room, false);
                        CreateSideToWallDecoration(room, true);

                        if (Random.Range(0, 5) == 0)    //20%
                        {
                            CreateTeleport(room);
                        }

                        CreateTables(room, true);
                        break;
                    }
                case roomType.playerStart:
                    {
                        CreateUnderWallDecoration(room, false);
                        CreateSideToWallDecoration(room, true);

                        break;
                    }
                case roomType.last:
                    {
                        Environment.PaintLine(room, true, Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f));
                        Environment.PaintLine(room, false, Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f));

                        room.SetMiniMapIcon(icons.bossIcon,null);
                        room.HideFromMap(true);

                        CreateUnderWallDecoration(room, false);
                        CreateSideToWallDecoration(room, true);

                        break;
                    }
                case roomType.secret:
                    {
                        CreateUnderWallDecoration(room, false);
                        CreateSideToWallDecoration(room, true);

                        Environment.SpawnChest(room, chest, new Vector2(room.transform.position.x + (room.width) / 2, room.transform.position.y + (room.height) / 2 +1));

                        break;
                    }
                case roomType.shop:
                    {
                        Environment.CreateShop(room, shopToLeft, shopToRight);

                        room.SetMiniMapIcon(icons.shopIcon,null);
                        room.HideFromMap(true);

                        CreateUnderWallDecoration(room, true);
                        CreateSideToWallDecoration(room, true);

                        break;
                    }
                case roomType.nextlevel:
                    {
                        CreateExit(room);
                       
                        break;
                    }
            }
     
        }

        foreach(Corridor corridor in Map.corridors)
        {
            CreateTorchesNearDoors(corridor);
        }

        yield return 0;
    }

    private void CreateTeleport(Room room)
    {
        Vector2 tmp = Vector2.zero;
        bool ok = false;

        List<Vector2> tiles = new List<Vector2>();

        foreach (Wall wall in room.smartGrid.UpWalls)
        {
            if (wall.gameObject.transform.position.x > room.start.x + 2)
            {
                tmp = wall.gameObject.transform.position;
                Wall checkDoor1 = room.GetWallAtPosition(new Vector3(tmp.x + 2, tmp.y));
                Wall checkDoor2 = room.GetWallAtPosition(new Vector3(tmp.x - 2, tmp.y));

                if (checkDoor1 != null && checkDoor2 != null)
                {
                    Wall w1 = room.GetWallAtPosition(new Vector3(tmp.x + 1, tmp.y));
                    Wall w2 = room.GetWallAtPosition(new Vector3(tmp.x - 1, tmp.y));

                    if (w1 != null && w2 != null)
                    {
                        tiles.Add(w1.gameObject.transform.position);
                        tiles.Add(w2.gameObject.transform.position);
                        tiles.Add(wall.gameObject.transform.position);
                        ok = true;
                        break;
                    }
                }
            }
        }
        if (ok)
        {
            Vector2 lowest = tiles[0];
            Vector2 highest = tiles[0];

            foreach (Vector2 v in tiles)
            {
                lowest = SimpleFunctions.LowestVector(lowest, v);
            }
            foreach (Vector2 v in tiles)
            {
                highest = SimpleFunctions.HighestVector(highest, v);
            }
            List<Wall> walls = room.smartGrid.GetWallsRange(lowest, highest);
            for(int i = 0; i < walls.Count; i++)
            {
                room.smartGrid.RemoveWall(walls[i]);
            }

            List<Vector2> rect = SimpleFunctions.GetRectangle(lowest, highest);

            foreach (Vector2 r in rect)
            {
                room.CreateTile(r, FloorType.outside);
            }

            room.smartGrid.CreateWallAroundTiles(rect.ToArray());

            GameObject tel = room.CreateEnvironmentObject(teleport.gameObject, tmp);
            room.teleport = tel.GetComponent<Teleport>();
            room.SetMiniMapIcon(icons.teleportIcon, tmp);
        }
    }

    public void CreateUnderWallDecoration(Room room, bool checkEnvCollision) //На крайних тайлах
    {
        List<int> objectsIndex = new List<int>();            // Типы объектов которые будут спавнится в комнате
        for (int i = 0; i < maxUnderWallObjects; i++)
        {
            objectsIndex.Add(Random.Range(0, underWallsObjects.Length));
        }

        if (checkEnvCollision)
        {
            foreach (GameObject tile in room.smartGrid.floorTilesOutSide)
            {
                int spawn = Random.Range(0, 5);
                if (spawn == 0)         //  20%
                {
                    if (room.CheckEnvironmentCollision(tile.transform.position) == false)
                    {

                        room.CreateEnvironmentObject(underWallsObjects[objectsIndex[Random.Range(0, objectsIndex.Count)]], tile.transform.position);
                    }
                }
            }
        }
        else
        {
            foreach (GameObject tile in room.smartGrid.floorTilesOutSide)
            {
                int spawn = Random.Range(0, 5);
                if (spawn == 0)         //  20%
                {
                    Vector3 left = new Vector2(tile.transform.position.x-1, tile.transform.position.y);
                    Vector3 right = new Vector2(tile.transform.position.x+1, tile.transform.position.y);
                    if (room.smartGrid.CheckFloorOutsideCollision(left) || room.smartGrid.CheckFloorOutsideCollision(right))
                    {
                        room.CreateEnvironmentObject(underWallsObjects[objectsIndex[Random.Range(0, objectsIndex.Count)]], tile.transform.position);
                    }
                }
            }
        }
    }  

    public void CreateSideToWallDecoration(Room room, bool checkEnvCollision)   //Слева или справа от стены
    {
        List<int> objectsIndex = new List<int>();            // Типы объектов которые будут спавнится в комнате
        for (int i = 0; i < maxSideWallObjects; i++)
        {
            objectsIndex.Add(Random.Range(0, sideWallObjects.Length));
        }

        if (checkEnvCollision)
        {
            foreach (GameObject tile in room.smartGrid.floorTilesOutSide)
            {
                int spawn = Random.Range(0, 10);
                if (spawn == 0)         //  10%
                {
                    Vector3 up = new Vector2(tile.transform.position.x, tile.transform.position.y +1);
                    Vector3 down = new Vector2(tile.transform.position.x, tile.transform.position.y-1);
                    if (room.smartGrid.CheckFloorOutsideCollision(up) || room.smartGrid.CheckFloorOutsideCollision(down))
                    {
                        if (room.CheckEnvironmentCollision(tile.transform.position) == false)
                        {
                            room.CreateEnvironmentObject(sideWallObjects[objectsIndex[Random.Range(0, objectsIndex.Count)]], tile.transform.position);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject tile in room.smartGrid.floorTilesOutSide)
            {
                int spawn = Random.Range(0, 5);
                if (spawn == 0)         //  20%
                {
                    Vector3 up = new Vector2(tile.transform.position.x, tile.transform.position.y+1);
                    Vector3 down = new Vector2(tile.transform.position.x, tile.transform.position.y-1);
                    if (room.smartGrid.CheckFloorOutsideCollision(up) || room.smartGrid.CheckFloorOutsideCollision(down))
                    {
                        room.CreateEnvironmentObject(sideWallObjects[objectsIndex[Random.Range(0, objectsIndex.Count)]], tile.transform.position);
                    }
                }
            }
        }
    }

    public void CreatePaintings(Room room, float chance)
    {
        foreach (Wall wall in room.smartGrid.UpWalls)     //Картины
        {
            float spawn = Random.Range(0, 1f);
            if (spawn < chance)
            {
                int type = Random.Range(0, 2);
                if (type == 0)  //большая картина
                {
                    if (onWallEnvironment.wideWallObjects.Count != 0)
                    {
                        Vector3 wallToRight = new Vector3(wall.gameObject.transform.position.x + 1, wall.gameObject.transform.position.y, 0);

                        foreach (Wall w in room.smartGrid.UpWalls) //Проверяем есть ли стена справа
                        {
                            if (w.gameObject.transform.position == wallToRight)
                            {
                                if (w.hasDecoration == false)
                                {
                                    wall.CreateDecoration(onWallEnvironment.wideWallObjects[Random.Range(0, onWallEnvironment.wideWallObjects.Count)]);
                                    w.hasDecoration = true;
                                }
                                break;
                            }
                        }
                    }
                }
                else if (type == 1) //Средняя
                {
                    if (onWallEnvironment.wallObjects.Count != 0)
                    {
                        wall.CreateDecoration(onWallEnvironment.wallObjects[Random.Range(0, onWallEnvironment.wallObjects.Count)]);
                    }
                }
            }
        }
    }

    public void CreateWallModifications(Room room, float chance)
    {
        if (wallModificators.Count != 0)
        {
            foreach (Wall wall in room.smartGrid.UpWalls)     //Картины
            {
                float spawn = Random.Range(0, 1f);
                if (spawn < chance)
                {
                    wall.CreateDecoration(wallModificators[Random.Range(0, wallModificators.Count)], new Vector2(0, 0.5f));
                }
            }
        }
    }

    public void CreateTorchesNearDoors(Corridor cor)
    {
        foreach (GameObject d in cor.Doors)            // Факелы
        {
            //Стена справа
            var wall = cor.GetWallAtPosition(new Vector3(d.transform.position.x + 1, d.transform.position.y));
            if (wall != null)
            {
                if (wall.direction == Direction4D.Up) //Если стена верхняя
                {
                    wall.CreateDecoration(torch, new Vector2(0, 0.5f));
                }
            }

            //Стена слева
            wall = cor.GetWallAtPosition(new Vector3(d.transform.position.x - 1, d.transform.position.y));
            if (wall != null)
            {
                if (wall.direction == Direction4D.Up)
                {
                    wall.CreateDecoration(torch, new Vector2(0, 0.5f));
                }
            }

        }
    }

    public void CreateTables(Room room, bool checkEnvCollision)
    {
        if (checkEnvCollision)
        {
            foreach (GameObject tile in room.smartGrid.floorTilesInside)
            {

                int spawn = Random.Range(0, 75);
                if (spawn == 0)         //  1.5%
                {
                    Destructible env = table.GetComponent<Destructible>();
                    if (CheckEnvCollision(env, room, tile.transform.position) == false)
                    {
                        room.CreateEnvironmentObject(table, tile.transform.position);
                    }
                }

            }
        }
    }

    private bool CheckEnvCollision(Destructible env, Room room, Vector2 position)
    {
        foreach (Vector2 v in env.coordinates)
        {
            if (room.CheckEnvironmentCollision(new Vector2(position.x + v.x, position.y + v.y)) == true)
            {
                return true;
            }
        }

        foreach (Vector2 v in env.coordinates)
        {
            if (room.CheckWallCollision(new Vector2(position.x + v.x, position.y + v.y)) == true)
            {
                return true;
            }
        }

        return false;
    }

    private void CreateExit(Room room)
    {
        room.CreateEnvironmentObject(exit, room.GetCenterPosition());
    }
}
