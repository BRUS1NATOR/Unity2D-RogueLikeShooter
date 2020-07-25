using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Hands : MonoBehaviour
{
    public Vector2 defaultHandsPos;
    public Vector2 lookDownHandsPos;

    public GameObject frontHand;
    public GameObject rearHand;
    public SortingGroup handsSort;

    private void SetHandsLayer(int layer)
    {
        handsSort.sortingOrder = layer;
    }

    public void SetLocalPosition(Direction4D position, int sortLayer)
    {
        
        switch (position)
        {
            case Direction4D.Down:
                SetHandsLayer(sortLayer);
                transform.localPosition = new Vector3(lookDownHandsPos.x, lookDownHandsPos.y, 0);
                break;
            case Direction4D.Up:
                SetHandsLayer(sortLayer);
                transform.localPosition = new Vector3(defaultHandsPos.x, defaultHandsPos.y, 0);
                break;
            default:
                SetHandsLayer(sortLayer);
                transform.localPosition = new Vector3(defaultHandsPos.x, defaultHandsPos.y, 0);
                break;
        }
    }
}
