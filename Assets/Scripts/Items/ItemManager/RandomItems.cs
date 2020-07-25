using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItems : MonoBehaviour
{
    public List<ItemRarity> items;

    public bool withLight;

    // Update is called once per frame
    public GameObject GetItem(bool withPrice)
    {
        return ItemManager.GetItem(items,withPrice, withLight);
    }
}