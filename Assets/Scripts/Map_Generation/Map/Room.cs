using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public enum roomType { playerStart, common, shop, last, secret, nextlevel}

public class EnvironmentTile
{
    GameObject tile;
    public EnvironmentTile(GameObject tile, Placement room)
    {
        Destructible obs = tile.GetComponent<Destructible>();
        if (obs != null)
        {
            if (obs.coordinates.Count != 0)
            {
                foreach (Vector2 v in obs.coordinates)
                {
                    room.Environment.Add(new Vector2(tile.transform.position.x + v.x, tile.transform.position.y + v.y));
                }
            }
            else
            {
                room.Environment.Add(new Vector2(tile.transform.position.x, tile.transform.position.y));
            }
        }

        else
        {
            Destructible env = tile.GetComponent<Destructible>();
            if (env != null)
            {
                if (env.coordinates.Count != 0)
                {
                    foreach (Vector2 v in env.coordinates)
                    {
                        room.Environment.Add(new Vector2(tile.transform.position.x + v.x, tile.transform.position.y + v.y));
                    }
                }
                else
                {
                    room.Environment.Add(new Vector2(tile.transform.position.x, tile.transform.position.y));
                }
            }
            else
            {
                room.Environment.Add(new Vector2(tile.transform.position.x, tile.transform.position.y));
            }
        }
    }
}

[Serializable]
public struct DistanceToTheRoom
{
    public Room room;
    public float closestDistance;

    public Vector2 closestCorner;
    public Vector2 toOtherRoomClosestCorner;
}

[Serializable]
public struct CorridorForRoom
{
    public Corridor corridor;
    public Direction4D positionByRoom;
}

/// <summary>
/// Комната
/// </summary>
public class Room : Placement
{
    public List<GameObject> Doors;
    public Teleport teleport;
    public List<DistanceToTheRoom> distances;

    public byte number;
    public roomType roomType;

    public Direction4D corDirection;

    public Room prevRoom;

    public List<CorridorForRoom> corridors;           //У первой комнаты нету корридора

    public List<Vector3> notRemovable;      //Позиции на которых нельзя удалять или добавлять стены/тайлы при генерации архитектуры помогает избежать наслоений

    public List<GameObject> waterTiles;

    public List<Enemy> Enemies;  //Враги

    public int enemiesAlive
    {
        get
        {
            int count = Enemies.Where(e => e.isAlive == true).Count();
            return count;
        }
    }

    public int enemiesToSpawn;

    public GameObject enemiesGrid;

    protected override void Awake()
    {
        base.Awake();

        Doors = new List<GameObject>();
        distances = new List<DistanceToTheRoom>();

        corridors = new List<CorridorForRoom>();

        notRemovable = new List<Vector3>();

        Enemies = new List<Enemy>();


        enemiesGrid = new GameObject()
        {
            name = "Enemies"
        };
        enemiesGrid.transform.parent = grid.transform;

    }

    public void SetupCorridor(Direction4D direction, byte length, System.Random rand)
    {
        GameObject cor = new GameObject();

        Corridor corridor = cor.AddComponent<Corridor>();

        Vector2 startCorner = Vector2.zero;
        Vector2 endCorner = Vector2.zero;

        int offsetW = SimpleFunctions.Min(width, prevRoom.width);
        int offsetH = SimpleFunctions.Min(height, prevRoom.height);

        int tempX = rand.Next(2, offsetW);
        int tempY = rand.Next(1, offsetH);

        switch (direction)
        {
            case Direction4D.Up:
                startCorner = new Vector2(start.x + tempX, end.y);
                endCorner = new Vector2(startCorner.x, startCorner.y + length);
                break;
            case Direction4D.Down:
                startCorner = new Vector2(start.x + tempX, start.y);
                endCorner = new Vector2(startCorner.x, startCorner.y - length);
                break;
            case Direction4D.Right:
                startCorner = new Vector2(end.x, start.y + tempY);
                endCorner = new Vector2(startCorner.x + length, startCorner.y);
                break;
            case Direction4D.Left:
                startCorner = new Vector2(start.x, start.y + tempY);
                endCorner = new Vector2(startCorner.x-length, startCorner.y);
                break;
        }

        corridor.Setup(startCorner, endCorner, this, prevRoom, direction);
    }

    public void SetupRoom(byte number, byte roomWidth, byte roomHeight, Vector2 startPosition, Room previousRoom, roomType type, int RoomMaxWidth, int RoomMaxHeight)
    {
        start = new Vector2(startPosition.x - 1, startPosition.y - 1);
        end = new Vector2(startPosition.x + RoomMaxWidth, startPosition.y + RoomMaxHeight);

        roomType = type;
        enemiesToSpawn = UnityEngine.Random.Range(GameManager.instance.generator.minEnemies, GameManager.instance.generator.maxEnemies);
        this.number = number;
        prevRoom = previousRoom;
        width = roomWidth;
        height = roomHeight;
    }

    public void SetupRoom(byte number, byte roomWidth, byte roomHeight, Vector2 startPosition, roomType type, int RoomMaxWidth, int RoomMaxHeight)
    {
        start = new Vector2(startPosition.x - 1, startPosition.y - 1);
        end = new Vector2(startPosition.x + RoomMaxWidth, startPosition.y + RoomMaxHeight);

        roomType = type;
        enemiesToSpawn = UnityEngine.Random.Range(GameManager.instance.generator.minEnemies, GameManager.instance.generator.maxEnemies);
        this.number = number;
        width = roomWidth;
        height = roomHeight;
    }

    public void CreateHactch(GameObject hatch, Vector2 position)
    {
        GameObject d = Instantiate(hatch, position, transform.rotation) as GameObject;
        d.GetComponent<Next_Level>().room = this;
        d.transform.parent = grid.transform;
    }

    public void CloseDoors()
    {
        TurnOnLight();
        foreach (CorridorForRoom corridor in corridors)
        {
            corridor.corridor.CloseDoors();
        }
        EnemiesWakeUp();
    }

    public void EnemiesWakeUp()
    {
        foreach (Enemy enemy in Enemies)
        {
            if (enemy.isAlive)
            {
                enemy.WakeUp();
            }
        }
        GameManager.instance.audioManager.PlayMusic(MusicType.battle);
    }

    public void EnemiesSleep()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.Sleep();
        }
    }

    public void OpenDoors()
    {
        foreach (CorridorForRoom corridor in corridors)
        {
            corridor.corridor.OpenDoors();
        }
        if (teleport != null)
        {
            teleport.UnLock();
        }
        GameManager.instance.audioManager.PlayMusic(MusicType.ambient);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);

        if (Enemies.Count == 0)
        {
            foreach (Corridor cor in Map.corridors)
            {
                OpenDoors();
            }
            
        }
    }

    public bool CheckEnvironmentCollision(Vector3 position)
    {
        foreach(Vector3 v in Environment)
        {
            if(v == position)
            {
                return true;
            }
        }
        return false;
    }

    public void HighLight(bool light)
    {
         placementPlane.HighLight(light);
    }
}