using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FloorType { inside, outside }

/// <summary>
/// Класс от которого наследуется комнаты и корридоры
/// </summary>
public class Placement : MonoBehaviour
{
    public SmartGrid smartGrid;
    public List<EnvironmentTile> EnvironmentTiles;

    public List<Vector2> Environment;
    public List<Vector2> obstacles;

    public GameObject grid;
    public GameObject floorGrid;
    public GameObject wallGrid;
    public GameObject EnvironmentGrid;
    public GameObject areasGrid;

    public Vector2 start;
    public Vector2 end;

    public int width;
    public int height;

    protected PlacementPlane placementPlane;

    public bool hereWasPlayer;

    protected virtual void Awake()
    {
        smartGrid = this.gameObject.AddComponent<SmartGrid>();
        smartGrid.Init(this);
        gameObject.layer = LayerMask.NameToLayer("Rooms");

        EnvironmentTiles = new List<EnvironmentTile>();

        obstacles = new List<Vector2>();
        Environment = new List<Vector2>();

        grid = new GameObject()
        {
            name = "Grid"
        };
        grid.transform.parent = this.transform;

        floorGrid = new GameObject()
        {
            name = "Floor"
        };
        floorGrid.transform.parent = grid.transform;

        wallGrid = new GameObject()
        {
            name = "Walls"
        };
        wallGrid.transform.parent = grid.transform;

        EnvironmentGrid = new GameObject()
        {
            name = "Environment"
        };
        EnvironmentGrid.transform.parent = grid.transform;

        areasGrid = new GameObject()
        {
            name = "Areas"
        };
        areasGrid.transform.parent = grid.transform;
    }

    public void CreateNavigation(PlacementPlane navPlane)
    {
        placementPlane = Instantiate(navPlane);
        placementPlane.Setup(this);
    }

    public void TurnOnLight()
    {
        placementPlane.LightOut();
    }

    public void RemoveWall(Wall wall)
    {
        smartGrid.RemoveWall(wall);
    }

    public GameObject CreateEnvironmentObject(GameObject obj, Vector2 position)
    {
        GameObject tileobj = Instantiate(obj, position, obj.transform.rotation) as GameObject;
        tileobj.transform.parent = EnvironmentGrid.transform;
        EnvironmentTiles.Add(new EnvironmentTile(tileobj, this));
        EnvironmentObjectWithHealth o = tileobj.GetComponent<EnvironmentObjectWithHealth>();
        if (o != null)
        {
            o.place = this;
        }
        return tileobj;
    }

    public Wall GetWallAtPosition(Vector3 position)
    {
        return smartGrid.GetWallAtPosition(position);
    }

    public GameObject CreateTile(Vector2 position, FloorType tileType)
    {
        return smartGrid.CreateTile(position, tileType, floorGrid.transform);
    }

    public bool CheckWallCollision(Vector2 position)
    {
        return smartGrid.CheckWallCollision(position);
    }

    public bool CheckFloorCollision(Vector3 position)
    {
        bool coll = false;
        coll = smartGrid.CheckFloorInsideCollision(position);
        coll = smartGrid.CheckFloorOutsideCollision(position);

        return coll;
    }


    public Vector2 GetCenterPosition()
    {
        float X = start.x + ((width +1f) / 2f);
        float Y = start.y + (height +1f) / 2f;

        Vector3 spawnPoint = new Vector3(X, Y);
        int i = 0;

        while (smartGrid.CheckFloorInsideCollision(spawnPoint) == false && smartGrid.CheckFloorInsideCollision(new Vector2(spawnPoint.x + 0.5f,spawnPoint.y+0.5f))==false)
        {
            i++;
            spawnPoint = new Vector3(spawnPoint.x + 1, spawnPoint.y + 1);
            if (i > 6)
            {
                spawnPoint = smartGrid.floorTilesInside[0].transform.position;
                break;
            }
        }
        return spawnPoint;
    }

    public Vector2 GetCenter()
    {
        return new Vector2(transform.position.x + (width) / 2, transform.position.y + (height) / 2);
    }

    public void SetMiniMapIcon(Sprite icon, Vector2? cord)
    {
        if (placementPlane != null)
        {
            placementPlane.SetMiniMapIcon(icon, cord);
        }
    }

    public void HideFromMap(bool hide)
    {
        if (placementPlane != null)
        {
            placementPlane.HideFromMap(hide);
        }
    }
}
