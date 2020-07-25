using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Positioning { Horizontal, Vertical}

public class Corridor : Placement
{
    public Positioning positioning;

    private BoxCollider2D boxCollider;
    public Room[] Rooms;
    public List<GameObject> Doors;  //Двери

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        Rooms = new Room[2];
        Doors = new List<GameObject>();
    }

    public void CloseDoors()
    {
        foreach(GameObject door in Doors)
        {
            door.GetComponent<Door>().Lock();
        }
    }

    public void OpenDoors()
    {
        foreach (GameObject door in Doors)
        {
            door.GetComponent<Door>().UnLock();
        }
    }

    public void Setup(Vector2 Start, Vector2 End, Room First, Room Second, Direction4D corDirection)
    {
        CorridorForRoom corForRoom = new CorridorForRoom();
        corForRoom.corridor = this;
        corForRoom.positionByRoom = Direction4D.Down;

        if (First.roomType == roomType.secret || Second.roomType == roomType.secret)
        {
            tag = "SecretCorridor";
            name = "SecretCorridor";
        }
        else
        {
            tag = "Corridor";
            name = "Corridor " + First.number;
        }

        positioning = Positioning.Vertical;
        switch (corDirection)
        {
            case Direction4D.Down:
                {
                    corForRoom.positionByRoom = Direction4D.Down;
                    First.corridors.Add(corForRoom);
                    corForRoom.positionByRoom = Direction4D.Up;
                    Second.corridors.Add(corForRoom);
                    break;
                }
            case Direction4D.Up:
                {
                    corForRoom.positionByRoom = Direction4D.Up;
                    First.corridors.Add(corForRoom);
                    corForRoom.positionByRoom = Direction4D.Down;
                    Second.corridors.Add(corForRoom);
                    break;
                }
            case Direction4D.Right:
                {
                    corForRoom.positionByRoom = Direction4D.Right;
                    First.corridors.Add(corForRoom);
                    corForRoom.positionByRoom = Direction4D.Left;
                    Second.corridors.Add(corForRoom);
                    positioning = Positioning.Horizontal;
                    break;
                }
            case Direction4D.Left:
                {
                    corForRoom.positionByRoom = Direction4D.Left;
                    First.corridors.Add(corForRoom);
                    corForRoom.positionByRoom = Direction4D.Right;
                    Second.corridors.Add(corForRoom);
                    positioning = Positioning.Horizontal;
                    break;
                }
        }
        

        Rooms[0] = First;
        Rooms[1] = Second;

        start = Start;
        end = End;

        boxCollider = this.gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        SetHeightAndWidth();

        Map.corridors.Add(this);
    }

    public void IncreaseHeightDown(int increase)
    {
        if (height < 0)
        {
            end = new Vector2(end.x, end.y - increase);
            SetHeightAndWidth();
        }
        else
        {
            start = new Vector2(start.x, start.y - increase);
            SetHeightAndWidth();
        }
    }

    public void IncreaseWidthLeft(int increase)
    {
        if (width < 0)
        {
            end = new Vector2(end.x - increase, end.y);
            SetHeightAndWidth();
        }
        else
        {
            start = new Vector2(start.x - increase, start.y);
            SetHeightAndWidth();
        }
    }

    private void SetHeightAndWidth()
    {
        width = System.Convert.ToInt32(end.x - start.x);
        height = System.Convert.ToInt32(end.y - start.y);

        if (width == 0)
        {
            width = 1;
        }

        if (height == 0)
        {
            height = 1;
        }

        if (width != 1 || height != 1)
        {
            if (width < 0 || height < 0)
            {
                transform.position = end;
            }
            else
            {
                transform.position = start;
            }
        }
    }

    public void SetupCollider()
    {
        if (positioning == Positioning.Horizontal)
        {
            float tempW = width < 0 ? width + 1 : width - 1;    //Укорачиваем на 1

            boxCollider.offset = new Vector2(tempW / 2, height / 2);
            boxCollider.size = new Vector2(Mathf.Round(Mathf.Abs(width)), Mathf.Round(Mathf.Abs(height)));
        }
        else
        {
            float heightW = height < 0 ? height + 1 : height - 1;    //Укорачиваем на 1

            boxCollider.offset = new Vector2(width / 2, heightW / 2);
            boxCollider.size = new Vector2(Mathf.Round(Mathf.Abs(width)), Mathf.Round(Mathf.Abs(height)));
        }
    }
}
