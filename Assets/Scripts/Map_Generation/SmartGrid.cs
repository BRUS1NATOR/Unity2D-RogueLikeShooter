using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Позволяет создавать, удалять стены и пол основываясь на помещении
/// </summary>
public class SmartGrid : MonoBehaviour
{
    public Placement place;

    public TileSet tiles;

    public List<GameObject> floorTilesInside = new List<GameObject>();
    public List<GameObject> floorTilesOutSide = new List<GameObject>();      //Тайлы расположенные с краю

    public List<Wall> UpWalls = new List<Wall>();
    public List<Wall> DownWalls = new List<Wall>();
    public List<Wall> LeftWalls = new List<Wall>();
    public List<Wall> RightWalls = new List<Wall>();

    public void Init(Placement place)
    {
        this.place = place;
        tiles = Map.tiles;
        tiles.Init();
    }


    //   ----------------------------- CREATE -------------------------- //

    public void CreateWallAroundTiles(Vector2[] tilePos)
    {
        if (place as Room)
        {
            Room room = place as Room;

            CreateWalls(tilePos);
        }
    }

    public Wall CreateWall(Vector2 position, Direction4D direction)
    {
        GameObject[] tileSet = GetWallByDirection(direction);
        Wall wall = new Wall(direction)
        {
            gameObject = GameObject.Instantiate(tileSet[UnityEngine.Random.Range(0, tileSet.Length)], position, tileSet[0].transform.rotation) as GameObject
        };

        wall.gameObject.transform.parent = place.wallGrid.transform;

        switch (direction)
        {
            case Direction4D.Up:
                UpWalls.Add(wall);
              //  Vector2 upper = new Vector2(position.x, position.y + 1);
                break;

            case Direction4D.Down:
                DownWalls.Add(wall);
                break;

            case Direction4D.Left:
                LeftWalls.Add(wall);
                break;

            case Direction4D.Right:
                RightWalls.Add(wall);
                break;
        }

        wall.info = wall.gameObject.name + "  pos: " + wall.gameObject.transform.position;

        return wall;

    }

    private void CreateUpWall(Vector2 outSideTilePos)
    {
        Vector2 temp = new Vector2(outSideTilePos.x, outSideTilePos.y + 1);

        if (CheckWallCollision(temp) == false)
        {
            if (CheckFloorCollision(temp) == false)
            {
                if (GetCorridorWallAtPosition(temp) == null)
                {
                    CreateWall(temp, Direction4D.Up);
                }
            }
        }
    }
    private void CreateSideWall(Vector2 outSideTilePos)
    {
        //Справа
        Vector2 temp = new Vector2(outSideTilePos.x + 1, outSideTilePos.y);
        if (CheckWallCollision(temp) == false)
        {
            if (CheckFloorCollision(temp) == false)
            {
                if (GetCorridorWallAtPosition(temp) == null)
                {
                    CreateWall(temp, Direction4D.Right);
                }
            }
        }

        //Справа сверху
        temp = new Vector2(outSideTilePos.x + 1, outSideTilePos.y + 1);
        if (CheckWallCollision(temp) == false)
        {
            if (CheckFloorCollision(temp) == false)
            {
                if (GetCorridorWallAtPosition(temp) == null)
                {
                    CreateWall(temp, Direction4D.Right);
                }
            }
        }

        //Слева
        temp = new Vector2(outSideTilePos.x - 1, outSideTilePos.y);
        if (CheckWallCollision(temp) == false)
        {
            if (CheckFloorCollision(temp) == false)
            {
                if (GetCorridorWallAtPosition(temp) == null)
                {
                    CreateWall(temp, Direction4D.Left);
                }
            }
        }

        //Слева сверху
        temp = new Vector2(outSideTilePos.x - 1, outSideTilePos.y + 1);
        if (CheckWallCollision(temp) == false)
        {
            if (CheckFloorCollision(temp) == false)
            {
                if (GetCorridorWallAtPosition(temp) == null)
                {
                    CreateWall(temp, Direction4D.Left);
                }
            }
        }

    }
    private void CreateRightWall(Vector2 outSideTilePos)
    {
        //Справа
        Vector2 temp = new Vector2(outSideTilePos.x + 1, outSideTilePos.y);

        if (CheckWallCollision(temp) == false)
        {
            if (CheckFloorCollision(temp) == false)
            {
                Wall wall = GetCorridorWallAtPosition(temp);

                if (wall == null)
                {
                    CreateWall(temp, Direction4D.Right);
                }
                else if (wall.direction == Direction4D.Down)
                {
                    CreateWall(temp, Direction4D.Right);
                }
            }
        }
    }
    private void CreateLeftWall(Vector2 outSideTilePos)
    {
        // Слева
        Vector2 temp = new Vector2(outSideTilePos.x - 1, outSideTilePos.y);

        if (CheckWallCollision(temp) == false)
        {
            if (CheckFloorCollision(temp) == false)
            {
                Wall wall = GetCorridorWallAtPosition(temp);
                if (wall == null)
                {
                    CreateWall(temp, Direction4D.Left);
                }
                else if (wall.direction == Direction4D.Down)
                {
                    CreateWall(temp, Direction4D.Left);
                }
            }
        }
    }
    private void CreateDownWall(Vector2 outSideTilePos)
    {
        Vector2 temp = new Vector2(outSideTilePos.x, outSideTilePos.y - 1);

        Wall wall = GetWallAtPosition(temp);

        if (wall == null)
        {
            if (CheckFloorCollision(temp) == false)
            {
                CreateWall(temp, Direction4D.Down);
            }
        }
        else
        {
            if (wall.direction == Direction4D.Left)
            {
                CreateWall(temp, Direction4D.Down);
            }
            else if (wall.direction == Direction4D.Right)
            {
                CreateWall(temp, Direction4D.Down);
            }
        }
    }
    private void CreateLongWall(Vector2 downWallPos)
    {
        Vector2 pos = new Vector2(downWallPos.x - 1, downWallPos.y + 1);
        Wall wall = GetWallAtPosition(pos);

        if (wall != null)
        {
            if (wall.direction == Direction4D.Left)
            {
                CreateWall(wall.gameObject.transform.position, Direction4D.Left);
                RemoveWall(wall);
            }
        }

        pos = new Vector2(downWallPos.x + 1, downWallPos.y + 1);
        wall = GetWallAtPosition(pos);
        if (wall != null)
        {
            if (wall.direction == Direction4D.Right)
            {
                CreateWall(wall.gameObject.transform.position, Direction4D.Right);
                RemoveWall(wall);
            }
        }
    }


    public void CreateWalls(Vector2[] tilePositions)
    {
        foreach (Vector2 tilePos in tilePositions)
        {
            CreateUpWall(tilePos);
        }

        foreach (Vector2 tilePos in tilePositions)
        {
            Wall w = GetWallAtPosition(new Vector2(tilePos.x, tilePos.y + 1));
            if (w != null)
            {
                CreateSideWall(w.gameObject.transform.position);
            }
        }
        foreach (Vector2 tilePos in tilePositions)
        {
            CreateSideWall(tilePos);
            CreateLeftWall(tilePos);
            CreateRightWall(tilePos);
        }

        //for (int i = 0; i < DownWalls.Count; i++)
        //{
        //    CreateLongWall(DownWalls[i].gameObject.transform.position);
        //}
    }


    public void CreateWalls()
    {
        Vector2 temp;
        if (place as Room)
        {
            Room room = place as Room;
            foreach (GameObject outsidetile in floorTilesOutSide)
            {
                CreateUpWall(outsidetile.transform.position);
            }

            // Сбоку от стены сверху
            foreach (Wall wall in UpWalls)
            {
                CreateSideWall(wall.gameObject.transform.position);
            }

            // Справа слева
            foreach (GameObject outsidetile in room.smartGrid.floorTilesOutSide)
            {
                CreateLeftWall(outsidetile.transform.position);
                CreateRightWall(outsidetile.transform.position);
            }
            // Снизу
            foreach (GameObject outsidetile in floorTilesOutSide)
            {
                CreateDownWall(outsidetile.transform.position);
            }

            // Длинные стены

            for (int i = 0; i < DownWalls.Count; i++)
            {
                CreateLongWall(DownWalls[i].gameObject.transform.position);
            }
        }

        else if (place as Corridor)
        {
            Corridor corridor = place as Corridor;
            if (corridor.positioning == Positioning.Horizontal)
            {
                foreach (GameObject tile in corridor.smartGrid.floorTilesOutSide)
                {
                    Vector2 pos = new Vector2(tile.transform.position.x, tile.transform.position.y + 1);
                    if (CheckFloorOutsideCollision(pos) == false)
                    {
                        CreateWall(pos, Direction4D.Up);
                    }
                    pos = new Vector2(tile.transform.position.x, tile.transform.position.y - 1);
                    if (CheckFloorOutsideCollision(pos) == false)
                    {
                        CreateWall(pos, Direction4D.Down);
                    }
                }
            }
            else
            {
                Vector2 startPoint = Vector2.zero;
                if (corridor.height > 0)
                {
                    startPoint = corridor.start;
                }
                else
                {
                    startPoint = new Vector2(corridor.end.x, corridor.end.y + 1);
                }

                foreach (GameObject tile in corridor.smartGrid.floorTilesOutSide)
                {
                    Vector2 pos = tile.transform.position;

                    if (pos.y != startPoint.y && pos.y != startPoint.y + 1)
                    {
                        pos = new Vector2(tile.transform.position.x + 1, tile.transform.position.y);
                        if (CheckFloorOutsideCollision(pos) == false)
                        {
                            CreateWall(pos, Direction4D.Right);
                        }
                        pos = new Vector2(tile.transform.position.x - 1, tile.transform.position.y);
                        if (CheckFloorOutsideCollision(pos) == false)
                        {
                            CreateWall(pos, Direction4D.Left);
                        }
                    }
                    else if (pos.y == startPoint.y)
                    {
                        Room roomUnder = corridor.Rooms[0];
                        if (corridor.height < 0)
                        {
                            roomUnder = corridor.Rooms[1];
                        }
                        pos = new Vector2(tile.transform.position.x + 1, tile.transform.position.y);
                        if (roomUnder.smartGrid.CheckFloorOutsideCollision(new Vector2(pos.x, pos.y - 1)) == false)
                        {
                            CreateWall(pos, Direction4D.Right);
                            CreateWall(new Vector2(pos.x, pos.y + 1), Direction4D.Right);
                        }

                        pos = new Vector2(tile.transform.position.x - 1, tile.transform.position.y);
                        if (roomUnder.smartGrid.CheckFloorOutsideCollision(new Vector2(pos.x, pos.y - 1)) == false)
                        {
                            CreateWall(pos, Direction4D.Left);
                            CreateWall(new Vector2(pos.x, pos.y + 1), Direction4D.Left);
                        }
                    }
                }
            }
        }
    }

    public GameObject CreateTile(Vector2 position, FloorType tileType, Transform parent)
    {
        GameObject tileobj = Instantiate(Map.tiles.GetFloorTile(), position, Quaternion.identity) as GameObject;

        tileobj.transform.parent = parent;

        switch (tileType)
        {
            case FloorType.outside:
                {
                    tileobj.name = "OutSide Tile";
                    floorTilesOutSide.Add(tileobj);
                    break;
                }
            case FloorType.inside:
                {
                    tileobj.name = "Tile";
                    floorTilesInside.Add(tileobj);
                    break;
                }
        }

        return tileobj;
    }

    public void CreateDoors()
    {
        if (place as Corridor)
        {
            Corridor corridor = place as Corridor;
            if (corridor.positioning == Positioning.Horizontal)
            {
                if (corridor.width > 0)
                {
                    corridor.Doors.Add(Instantiate(tiles.door_left, corridor.start, Quaternion.identity, this.transform));
                    corridor.Doors.Add(Instantiate(tiles.door_right, new Vector2(corridor.end.x - 1, corridor.end.y), Quaternion.identity, this.transform));
                }
                else
                {
                    corridor.Doors.Add(Instantiate(tiles.door_left, new Vector2(corridor.end.x + 1, corridor.end.y), Quaternion.identity, this.transform));
                    corridor.Doors.Add(Instantiate(tiles.door_right, corridor.start, Quaternion.identity, this.transform));
                }
            }
            else
            {
                if (corridor.height > 0)
                {
                    corridor.Doors.Add(Instantiate(tiles.door_bottom, new Vector2(corridor.end.x, corridor.end.y - 1), Quaternion.identity, this.transform));
                    corridor.Doors.Add(Instantiate(tiles.door_upper, corridor.start, Quaternion.identity, this.transform));
                }
                else
                {
                    corridor.Doors.Add(Instantiate(tiles.door_upper, new Vector2(corridor.end.x, corridor.end.y + 1), Quaternion.identity, this.transform));
                    corridor.Doors.Add(Instantiate(tiles.door_bottom, corridor.start, Quaternion.identity, this.transform));
                }
            }
            foreach (Room r in corridor.Rooms)
            {
                foreach (GameObject d in corridor.Doors)
                {
                    r.Doors.Add(d);
                }
            }
        }

    }

    //   ----------------------------- REMOVE -------------------------- //

    public void RemoveWall(Wall wall)
    {
        var list = new List<Wall>();
        switch (wall.direction)
        {
            case Direction4D.Down:
                {
                    list = DownWalls;
                    break;
                }
            case Direction4D.Up:
                {
                    list = UpWalls;
                    break;
                }
            case Direction4D.Left:
                {
                    list = LeftWalls;
                    break;
                }
            case Direction4D.Right:
                {
                    list = RightWalls;
                    break;
                }
        }

        list.Remove(wall);
        wall.Destroy();
    }

    public bool RemoveFloor(Vector3 position)
    {
        bool removed = false;

        foreach (GameObject roomTile in floorTilesInside)
        {
            if (roomTile.transform.position == position)
            {
                floorTilesInside.Remove(roomTile);
                Destroy(roomTile);
                return true;
            }
        }

        foreach (GameObject roomTile in floorTilesOutSide)
        {
            if (roomTile.transform.position == position)
            {
                floorTilesOutSide.Remove(roomTile);
                Destroy(roomTile);
                return true;
            }
        }

        return removed;
    }


    //   ----------------------------- GET -------------------------- //
    private GameObject[] GetWallByDirection(Direction4D direction)
    {
        switch (direction)
        {
            case Direction4D.Down:
                {
                    return Map.tiles.downWallTiles;
                }
            case Direction4D.Up:
                {
                    return Map.tiles.walltiles;
                }
            case Direction4D.Left:
                {
                    return Map.tiles.leftWallTiles;
                }
            case Direction4D.Right:
                {
                    return Map.tiles.rightWallTiles;
                }
        }
        return null;
    }

    public Wall GetWallAtPosition(Vector3 position)
    {
        foreach (Wall wall in UpWalls)
        {
            if (wall.gameObject.transform.position == position)
            {
                return wall;
            }
            if (wall.gameObject.transform.position == new Vector3(position.x, position.y - 1, position.z))
            {
                return wall;
            }
        }

        foreach (Wall wall in LeftWalls)
        {
            if (wall.gameObject.transform.position == position)
            {
                return wall;
            }
        }

        foreach (Wall wall in RightWalls)
        {
            if (wall.gameObject.transform.position == position)
            {
                return wall;
            }
        }

        foreach (Wall wall in DownWalls)
        {
            if (wall.gameObject.transform.position == position)
            {
                return wall;
            }
        }

        return null;
    }

    public List<Wall> GetWallsRange(Vector2 lowest, Vector2 highest)
    {
        List<Vector2> positions = SimpleFunctions.GetRectangle(lowest, new Vector2(highest.x,highest.y+1));
        List<Wall> walls = new List<Wall>();

        foreach(Vector2 pos in positions)
        {
            Wall w = GetWallAtPosition(pos);
            if (w != null)
            {
                walls.Add(w);
            }
        }

        return walls;
    }

    public GameObject GetFloor(Vector3 position)
    {
        foreach (GameObject tile in floorTilesInside)
        {
            if (position == tile.transform.position)
            {
                return tile;
            }
        }

        foreach (GameObject tile in floorTilesOutSide)
        {
            if (position == tile.transform.position)
            {
                return tile;
            }
        }
        return null;
    }


    //   ----------------------------- CHECK -------------------------- //
    public bool CheckFloorInsideCollision(Vector3 position)
    {
        foreach (GameObject tile in floorTilesInside)
        {
            if (position == tile.transform.position)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckFloorOutsideCollision(Vector3 position)
    {
        foreach (GameObject tile in floorTilesOutSide)
        {
            if (position == tile.transform.position)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckFloorCollision(Vector3 position)
    {
        if (CheckFloorInsideCollision(position))
        {
            return true;
        }

        if (CheckFloorOutsideCollision(position))
        {
            return true;
        }

        foreach (Vector3 v in place.obstacles)
        {
            if (position == v)
            {
                return true;
            }
        }

        if (place as Room)
        {
            Room room = place as Room;
            foreach (CorridorForRoom corridor in room.corridors)
            {
                if (corridor.corridor.CheckFloorCollision(position))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckWallCollision(Vector2 position)
    {
        Wall wall = GetWallAtPosition(position);
        if (wall != null)
        {
            return true;
        }
        return false;
    }

    public Wall GetCorridorWallAtPosition(Vector3 position)
    {
        if (place as Room)
        {
            Room room = place as Room;
            foreach (CorridorForRoom corridor in room.corridors)
            {
                Wall wall = corridor.corridor.GetWallAtPosition(position);
                if (wall != null)
                {
                    return wall;
                }
            }
        }
        return null;
    }

}
