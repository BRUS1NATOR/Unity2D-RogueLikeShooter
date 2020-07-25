using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomItems))]
public class Table : Destructible
{
    RandomItems randItems;
    public bool spawnItems;

    [HideInInspector]
    public List<Item> tableItems;

    public List<Transform> positions;

    public void Start()
    {
        if (spawnItems)
        {
            randItems = GetComponent<RandomItems>();

            if (randItems != null)
            {
                foreach (Transform p in positions)
                {
                    //   Debug.Log("Chance: " + GameManager.instance.items.rareWeapons.chance);
                    GameObject g = randItems.GetItem(true);
                    PlaceOnTable(g, p);
                }
            }
        }
    }

    public void PlaceOnTable(GameObject item, Transform pos)
    {
        if (item != null)
        {
            item.transform.position = pos.position;
            item.transform.parent = transform;
        }
    }
}
