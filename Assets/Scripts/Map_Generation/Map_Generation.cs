using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Generation : MonoBehaviour
{
    public bool dontHideMap;
    public System.Random randomGenerator;
    public int randSeed;

    public int roomsAmount;

    public int shopsAmount;
    public int secretAmount;

    public bool createArchitecture;

    //шанс разветвление (у одной комнаты более 2 выходов)
    public float branchChance;

    //chances
    public float chanceUp;
    public float chanceDown;
    public float chanceRight;
    public float chanceLeft;

    //room
    public byte RoomMinWidth;
    public byte RoomMaxWidth;
    public byte RoomMinHeight;
    public byte RoomMaxHeight;

    //corridor
    public byte minCorridorLength;

    public PlacementPlane placementPlane;

    public Text loadingText;

    public IEnumerator Begin(int roomsAmount, Text loadText, bool firstLevel)
    {
        this.roomsAmount = roomsAmount;
        if (firstLevel)
        {
            randSeed = GameManager.instance.randomSeed;
            randomGenerator = new System.Random(randSeed);
        }

        if (Map.roomObjs != null)
        {
            foreach (Room r in Map.roomObjs)
            {
                if (r != null)
                {
                    Destroy(r.gameObject);
                }
            }
            Map.roomObjs.Clear();
        }
        if (Map.corridors != null)
        {
            foreach (Corridor c in Map.corridors)
            {
                if (c != null)
                {
                    Destroy(c.gameObject);
                }
            }
            Map.corridors.Clear();
        }
        
        Map.mapIsGenerated = false;

        if (loadText != null)
        {
            loadingText = loadText;
        }

        transform.position = new Vector2(256, 256);   //Начальное положение 256 256


        if (minCorridorLength < 1)
        {
            minCorridorLength = 1;
        }

        Map.roomObjs = new List<Room>();
        Map.corridors = new List<Corridor>();

        yield return StartCoroutine(GenerateLevel());
    }

    public void ChangeLoadText(string s)
    {
        if (loadingText != null)
        {
            if (s != null)
            {
                loadingText.text = s;
                return;
            }
            loadingText.text = "";
        }
    }

    private IEnumerator GenerateLevel()
    {
        ChangeLoadText("CREATING ROOMS");
        yield return StartCoroutine(GenerateRooms());

        if (GameManager.instance.sceneNow != SceneNow.MainMenu)
        {
            ChangeLoadText("CREATING SHOPS");
            yield return StartCoroutine(GenerateShops());
        }

        ChangeLoadText("ADDING ADDITIONAL CORRIDORS");
        yield return StartCoroutine(CreateAdditionalCorridors());

        ChangeLoadText("CREATING ADDITIONAL ROOMS");
        yield return StartCoroutine(GenerateSecret());
        yield return StartCoroutine(GenerateNextLevel());

        ChangeLoadText("RESIZING ROOMS");
        yield return StartCoroutine(ResizeRooms());

        ChangeLoadText("CREATING CORRIDORS");
        yield return StartCoroutine(CreateCorridors());

        Map.Calculate();

        ChangeLoadText("CREATING ARCHITECTURE");
        yield return StartCoroutine(Architecture_Generator.Generate(randomGenerator));

        ChangeLoadText("CREATING WALLS");
        yield return StartCoroutine(CreateWallsAndNavigation());

        if (Map.tiles.environment_gen != null)
        {
            ChangeLoadText("CREATING ENVIRONMENT");
            yield return StartCoroutine(Map.tiles.environment_gen.Create());
        }

        Debug.Log(Map.mapIsGenerated);
        Map.mapIsGenerated = true;


        Map.HideMap(!dontHideMap);


        ChangeLoadText(null);

        yield return true;
    }

    IEnumerator GenerateRooms()
    {
        for (byte roomCount = 0; roomCount < roomsAmount+1; roomCount++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            if (roomCount == 0)
            {
                GameObject roomObj = new GameObject
                {
                    name = "Room" + roomCount,
                    tag = "Room"
                };

                Room room = roomObj.AddComponent<Room>();
                Map.roomObjs.Add(room);

                room.SetupRoom(roomCount, 16, 16, transform.position, roomType.playerStart, RoomMaxWidth, RoomMaxHeight);
                CreateRoom(Map.roomObjs[roomCount]);
            }
            else
            {
                byte corridor_length = minCorridorLength;
                int roombehind = roomCount - 1;  // комната сзади

                byte height = System.Convert.ToByte(randomGenerator.Next(RoomMinHeight, RoomMaxHeight));
                byte width = System.Convert.ToByte(randomGenerator.Next(RoomMinWidth, RoomMaxWidth));    //Рандоманая высота и ширина

                Direction4D randDirection = RandomDirection((float)randomGenerator.NextDouble());    //Рандомное направление

                if (branchChance > (float)randomGenerator.NextDouble() && roomCount > 5) //Дополнительное разветвление
                {
                    roombehind -= 1;
                }
                if (roomCount == roomsAmount)
                {
                    TryCreateRoom(roomCount, roombehind, width, height, randDirection, roomType.last);
                }
                else
                {
                    TryCreateRoom(roomCount, roombehind, width, height, randDirection, roomType.common);
                }
            }
        }
        yield return 0;
    }

    void GenerateRoom(byte number, Room prevRoom, byte width, byte height, Direction4D dir, roomType type)      //Дверь входящая в комнату
    {
        GameObject roomObj;
        switch (type)
        {
            case roomType.playerStart:
                {
                    roomObj = new GameObject
                    {
                        name = "Start Room " + number,
                        tag = "Room"
                    };
                    break;
                }
            case roomType.common:
                {
                    roomObj = new GameObject
                    {
                        name = "Room " + number,
                        tag = "Room"
                    };
                    break;
                }
            case roomType.shop:
                {
                    roomObj = new GameObject
                    {
                        name = "Shop " + number,
                        tag = "Shop"
                    };
                    break;
                }
            case roomType.last:
                {
                    roomObj = new GameObject
                    {
                        name = "Last Room",
                        tag = "Room"
                    };
                    break;
                }
            case roomType.secret:
                {
                    roomObj = new GameObject
                    {
                        name = "Secret Room " + number,
                        tag = "Room"
                    };
                    break;
                }
            case roomType.nextlevel:
                {
                    roomObj = new GameObject
                    {
                        name = "Next Level Room " + number,
                        tag = "Room"
                    };
                    break;
                }
            default:
                {
                    roomObj = new GameObject
                    {
                        name = "Room " + number,
                        tag = "Room"
                    };
                    break;
                }
        }

        Room room = roomObj.AddComponent<Room>();
        Map.roomObjs.Add(room);

        room.SetupRoom(number, width, height, transform.position, prevRoom, type, RoomMaxWidth, RoomMaxHeight);  //room count - roombehind 
        room.SetupCorridor(DoorInvert(dir), minCorridorLength, randomGenerator);                                                                        //Обычно count-1 - пред.комната
                                                                                                                                                        //Но иногда приходится идти на несколько комнат назад
        CreateRoom(room);
    }

    private void GetDistanceBetweenRooms()
    {
        foreach (Room thisRoom in Map.roomObjs)
        {
            foreach (Room nextRoom in Map.roomObjs)
            {
                if (thisRoom != nextRoom)
                {
                    thisRoom.distances.Add(SimpleFunctions.Check_SuperimposeDistanceVector(thisRoom, nextRoom));
                }
            }
        }
    }

    private IEnumerator CreateAdditionalCorridors()
    {
        GetDistanceBetweenRooms();
        bool skip = true; // if true skips corridor
        foreach (Room thisRoom in Map.roomObjs)
        {
            skip = !skip;
            if (skip == false)
            {
                foreach (DistanceToTheRoom d in thisRoom.distances)
                {
                    if (d.closestDistance < 7)
                    {
                        bool alreadyHasConnectionWithThisRoom = false;
                        foreach (CorridorForRoom cor in thisRoom.corridors)
                        {
                            foreach (Room room in cor.corridor.Rooms)
                            {
                                if (room == d.room)
                                {
                                    alreadyHasConnectionWithThisRoom = true;
                                    break;
                                }
                            }
                        }

                        if (alreadyHasConnectionWithThisRoom == false)
                        {
                            List<Room> rooms = new List<Room>();
                            rooms.Add(thisRoom);
                            rooms.Add(d.room);
                            Vector2 start = d.closestCorner;
                            Vector2 end = d.toOtherRoomClosestCorner;

                            if (d.closestCorner.x != d.toOtherRoomClosestCorner.x && d.closestCorner.y == d.toOtherRoomClosestCorner.y)   //Слева или справо
                            {
                                Direction4D direction = Direction4D.Right;

                                if (d.closestCorner.y == thisRoom.end.y)  //Если справа
                                {
                                    start = new Vector2(start.x, start.y - 1 - randomGenerator.Next(0, thisRoom.height));
                                    end = new Vector2(end.x + 1, start.y);
                                }
                                else
                                {
                                    start = new Vector2(start.x, start.y + 1 + randomGenerator.Next(0, thisRoom.height));
                                    end = new Vector2(end.x - 1, start.y);
                                    direction = Direction4D.Left;
                                }

                                if (Check_All_Superimpose(start, end) == false)
                                {
                                    GameObject cor = new GameObject();
                                    cor.name = "Corridor ROOM " + thisRoom.number + " and " + d.room.number;
                                    Corridor corridor = cor.AddComponent<Corridor>();
                                    cor.transform.position = start;

                                    corridor.Setup(start, end, thisRoom, d.room, direction);
                                }
                            }
                            else if (d.closestCorner.y != d.toOtherRoomClosestCorner.y && d.closestCorner.x == d.toOtherRoomClosestCorner.x)
                            {
                                Direction4D direction = Direction4D.Up;

                                if (d.closestCorner.x == thisRoom.end.x)
                                {
                                    start = new Vector2(start.x - 1 - randomGenerator.Next(0, thisRoom.width), start.y);
                                    end = new Vector2(start.x, end.y + 1);
                                }
                                else
                                {
                                    start = new Vector2(start.x + 1 + randomGenerator.Next(0, thisRoom.width), start.y);
                                    end = new Vector2(start.x, end.y - 1);
                                    direction = Direction4D.Down;
                                }

                                if (Check_All_Superimpose(start, end, rooms) == false)
                                {
                                    GameObject cor = new GameObject();
                                    cor.name = "Corridor ROOM " + thisRoom.number + " and " + d.room.number;
                                    Corridor corridor = cor.AddComponent<Corridor>();
                                    cor.transform.position = start;

                                    corridor.Setup(start, end, thisRoom, d.room, direction);
                                }

                            }
                        }
                    }
                }
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator GenerateShops()
    {
        for (int i = 0; i < shopsAmount; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            byte roomNumber = System.Convert.ToByte(roomsAmount + i);

            int roombehind = System.Convert.ToByte(randomGenerator.Next(0, roomsAmount));  // комната сзади

            byte height = 14;
            byte width = 16;

            Direction4D randDirection = RandomDirection((float)randomGenerator.NextDouble());    //Рандомное направление

            TryCreateRoom(roomNumber, roombehind, width, height, randDirection, roomType.shop);
        }

        yield return 0;
    }

    private IEnumerator GenerateSecret()
    {
        for (int i = 0; i < secretAmount; i++)
        {
            byte roomNumber = System.Convert.ToByte(roomsAmount + shopsAmount + i);

            int roombehind = System.Convert.ToByte(randomGenerator.Next(0, roomsAmount));  // комната сзади

            byte height = 9;
            byte width = 9;

            Direction4D randDirection = RandomDirection((float)randomGenerator.NextDouble());    //Рандомное направление

            TryCreateRoom(roomNumber, roombehind, width, height, randDirection, roomType.secret);
        }
        yield return 0;
    }

    private IEnumerator GenerateNextLevel()
    {
        byte roomNumber = System.Convert.ToByte(roomsAmount + shopsAmount + secretAmount);

        int roombehind = System.Convert.ToByte(roomsAmount);  // комната сзади (last)

        byte height = 6;
        byte width = 6;

        Direction4D randDirection = RandomDirection((float)randomGenerator.NextDouble());    //Рандомное направление

        TryCreateRoom(roomNumber, roombehind, width, height, randDirection, roomType.nextlevel);

        yield return 0;
    }

    IEnumerator ResizeRooms()
    {
        foreach (Room room in Map.roomObjs)
        {
            ResizeRoom(room);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private void ResizeRoom(Room room)
    {
        int heightNow = RoomMaxHeight;
        int widthNow = RoomMaxWidth;

        Vector2 removeFrom = new Vector2(room.start.x + 1, room.end.y - 1);

        while (heightNow > room.height) //Уменьшаем высоту комнаты
        {
            DestroyRect(removeFrom, 1, widthNow, room);

            foreach(CorridorForRoom corridorForRoom in room.corridors)
            {
                Corridor corridor = corridorForRoom.corridor;
                if(corridor.start.y == removeFrom.y)
                {
                    corridor.start = new Vector2(corridor.start.x, corridor.start.y-1);
                    corridor.end = new Vector2(corridor.end.x, corridor.end.y-1);
                }
                if (corridorForRoom.positionByRoom == Direction4D.Up)
                {
                    corridor.IncreaseHeightDown(1);
                }
            }

            heightNow -= 1;
            removeFrom = new Vector2(removeFrom.x, removeFrom.y - 1);
        }

        DestroyRect(removeFrom, 1, widthNow, room);
        CreateRect(removeFrom,1,widthNow,room);

        removeFrom = new Vector2(room.end.x - 1, room.start.y + 1);
        while (widthNow > room.width)   //Уменьшаем ширину комнаты
        {
            DestroyRect(removeFrom, heightNow, 1, room);

            foreach (CorridorForRoom corridorForRoom in room.corridors)
            {
                Corridor corridor = corridorForRoom.corridor;
                if (corridor.start.x == removeFrom.x)
                {
                    corridor.start = new Vector2(corridor.start.x - 1, corridor.start.y);
                    corridor.end = new Vector2(corridor.end.x - 1, corridor.end.y);
                }
                if (corridorForRoom.positionByRoom == Direction4D.Right)
                {
                    corridor.IncreaseWidthLeft(1);
                }
            }

            widthNow -= 1;
            removeFrom = new Vector2(removeFrom.x - 1, removeFrom.y);
        }

        DestroyRect(removeFrom, heightNow, 1, room);
        CreateRect(removeFrom, heightNow, 1, room);

        room.end = new Vector2(room.start.x + widthNow +1, room.start.y + heightNow +1);
    }

    private void TryCreateRoom(byte number, int prevRoomIndex, byte width, byte height, Direction4D direction, roomType type)
    {
        Room prevRoom = Map.roomObjs[prevRoomIndex].GetComponent<Room>();

        MoveRoomGenerator(prevRoom, direction);  //Сдвигаем в начало новой комнаты

        Vector2 roomstartpos = new Vector2(transform.position.x, transform.position.y);
        Vector2 roomendpos = new Vector2(transform.position.x + RoomMaxWidth, transform.position.y + RoomMaxHeight);

        bool fixDir = Check_All_Superimpose(roomstartpos, roomendpos);
        //FixDir == true => комнаты пересекаются

        for (int k = 1; k <= 4; k++)
        {
            if (fixDir == true)
            {
                direction = Increase_direction(direction);  //Меняем направление

                prevRoom = Map.roomObjs[prevRoomIndex].GetComponent<Room>();
                MoveRoomGenerator(prevRoom, direction);  //Двигаем в начало следующей комнаты

                roomstartpos = new Vector2(transform.position.x, transform.position.y);
                roomendpos = new Vector2(transform.position.x + RoomMaxWidth, transform.position.y + RoomMaxHeight);

                fixDir = Check_All_Superimpose(roomstartpos, roomendpos);

                if (fixDir == false)
                {
                    break;
                }

                if (k == 4) // Разветвление если не подошло 4 направления
                {
                    prevRoomIndex -= 1;
                    if (prevRoomIndex == 1)
                    {
                        Debug.Log("GGGGG");
                        return;  //Если дошло до ласт комнаты то GG
                    }
                    prevRoom = Map.roomObjs[prevRoomIndex].GetComponent<Room>();
                    k = 1;
                }
            }
        }

        GenerateRoom(number, prevRoom, width, height, direction, type);
    }

    Direction4D Increase_direction(Direction4D direction)
    {
        direction += 1;
        if ((int)direction >= 4)
        {
            direction = 0;
        }

        return direction;
    }

    Direction4D DoorInvert(Direction4D door)
    {
        switch (door)
        {
            case Direction4D.Up:
                return Direction4D.Down;
            case Direction4D.Down:
                return Direction4D.Up;
            case Direction4D.Left:
                return Direction4D.Right;
            case Direction4D.Right:
                return Direction4D.Left;
        }
        return Direction4D.Up;
    }

    void CreateRoom(Room room)
    {
        room.transform.position = new Vector2(room.start.x+1, room.start.y+1);
        transform.position = room.transform.position;

        int height = room.height;
        int width = room.width;

        BoxCollider2D box = room.gameObject.AddComponent<BoxCollider2D>();

        box.transform.position = room.transform.position;
        box.size = new Vector2(width +1, height + 1);

        box.offset = new Vector2((width / 2), (height / 2));

        box.isTrigger = true;

        CreateRect(transform.position,RoomMaxHeight,RoomMaxWidth, room);
        
    }

    IEnumerator CreateCorridors()
    {
        foreach (Corridor corridor in Map.corridors)
        {
            corridor.transform.position = corridor.start;
            CreateRect(corridor.start, corridor.height, corridor.width, corridor);
            corridor.smartGrid.CreateDoors();
            corridor.SetupCollider();
        }
        yield return 0;
    }

    public IEnumerator CreateWallsAndNavigation()
    {
        foreach (Corridor corridor in Map.corridors)
        {
            corridor.smartGrid.CreateWalls();
            corridor.CreateNavigation(placementPlane);
        }
        foreach (Room room in Map.roomObjs)
        {
            room.smartGrid.CreateWalls();
            room.CreateNavigation(placementPlane);
        }
        yield return 0;
    }

    void CreateRect(Vector2 startPos, int height, int width, Placement place)
    {
        transform.position = startPos;

        Direction4D moveSide = Direction4D.Right;
        Direction4D moveUpDown = Direction4D.Up;

        if (height == 0)
        {
            height = 1;
        }
        if (width == 0)
        {
            width = 1;
        }
        if (height < 0)
        {
            height = Mathf.Abs(height);
            moveUpDown = Direction4D.Down;
        }
        if (width < 0)
        {
            width = Mathf.Abs(width);
            moveSide = Direction4D.Left;
        }

        for (int i = 1; i <= height; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                if (i == 1 || j == 1 || i == height || j == width)
                {
                    place.CreateTile(transform.position, FloorType.outside);
                }
                else
                {
                    place.CreateTile(transform.position, FloorType.inside);
                }

                if (j != width)
                {
                    MoveGenerator(moveSide);
                }
            }
            if (i != height)
            {
                MoveGenerator(moveUpDown);
                transform.position = new Vector2(startPos.x, transform.position.y);
            }
        }
    }

    void DestroyRect(Vector2 startPos, int height, int width, Placement place)
    {
        transform.position = startPos;

        Direction4D moveSide = Direction4D.Right;
        Direction4D moveUpDown = Direction4D.Up;

        if (height == 0)
        {
            height = 1;
        }
        if (width == 0)
        {
            width = 1;
        }
        if (height < 0)
        {
            height = Mathf.Abs(height);
            moveUpDown = Direction4D.Down;
        }
        if (width < 0)
        {
            width = Mathf.Abs(width);
            moveSide = Direction4D.Left;
        }

        for (int i = 1; i <= height; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                if (i == 1 || j == 1 || i == height || j == width)
                {
                    place.smartGrid.RemoveFloor(transform.position);
                }
                else
                {
                    place.smartGrid.RemoveFloor(transform.position);
                }

                if (j != width)
                {
                    MoveGenerator(moveSide);
                }
            }
            if (i != height)
            {
                MoveGenerator(moveUpDown);
                transform.position = new Vector2(startPos.x, transform.position.y);
            }
        }
    }

    Direction4D RandomDirection(float rand_direction)
    {
        if (rand_direction < chanceUp)
        {
            return Direction4D.Up;
        }
        else if (rand_direction < chanceDown + chanceUp) //Chance Down
        {
            return Direction4D.Down;
        }
        else if (rand_direction < chanceDown + chanceUp + chanceRight) // Chance Right
        {
            return Direction4D.Right;
        }
        else
        {
            return Direction4D.Left;
        }
    }

    void MoveGenerator(Direction4D direction)
    {
        switch (direction)
        {
            case Direction4D.Up:
                transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                break;
            case Direction4D.Down:
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                break;
            case Direction4D.Right:
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                break;
            case Direction4D.Left:
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                break;
        }
    }

    void MoveRoomGenerator(Room previousRoom, Direction4D direction)
    {
        //Выставляем генератор в нужное положение
        switch (direction)
        {
            case Direction4D.Up:
                transform.position = new Vector2(previousRoom.transform.position.x, previousRoom.transform.position.y + minCorridorLength + RoomMaxHeight);
                break;
            case Direction4D.Down:
                transform.position = new Vector2(previousRoom.transform.position.x, previousRoom.transform.position.y - (minCorridorLength + RoomMaxHeight));
                break;
            case Direction4D.Right:
                transform.position = new Vector2(previousRoom.transform.position.x + minCorridorLength + RoomMaxWidth, previousRoom.transform.position.y);
                break;
            case Direction4D.Left:
                transform.position = new Vector2(previousRoom.transform.position.x - (minCorridorLength + RoomMaxWidth), previousRoom.transform.position.y);
                break;
        }
    }

    bool Check_All_Superimpose(Vector2 startpos, Vector2 endpos)
    {
        foreach (Room temproom in Map.roomObjs)
        {
            if (SimpleFunctions.Check_Superimpose(temproom.start, temproom.end, startpos, endpos, 2) == true || transform.position.x <= 0 || transform.position.y <= 0)
            {
                return true;
            }
        }
        return false;
    }

    bool Check_All_Superimpose(Vector2 startpos, Vector2 endpos, List<Room> exceptions)
    {
        foreach (Room temproom in Map.roomObjs)
        {
            bool ok = true;
            foreach (Room r in exceptions)
            {
                if (temproom == r)
                {
                    ok = false;
                    break;
                }
            }
            if (ok)
            {
                if (SimpleFunctions.Check_Superimpose(temproom.start, temproom.end, startpos, endpos, 2) == true || transform.position.x <= 0 || transform.position.y <= 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}