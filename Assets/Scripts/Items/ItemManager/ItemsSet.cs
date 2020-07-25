using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Структура содержащая предмет, его шанс выпадения и возможные цены
/// </summary>
[Serializable]
public struct ItemStruct
{
    public GameObject item;
    public float chance;
    public List<int> prices;
}


/// <summary>
/// Сет предметов
/// </summary>
[CreateAssetMenu]
public class ItemsSet : ScriptableObject
{
    public List<ItemStruct> items;
    public float maxchance
    {
        get
        {
            float overall = 0;
            if (items.Count != 0)
            {
                foreach (ItemStruct itemStruct in items)
                {
                    overall += itemStruct.chance;
                }
            }

            return overall;
        }
    }

    public GameObject GetRandomItem(bool withPrice)
    {
        float rand = UnityEngine.Random.Range(0f, maxchance);
        float temp = 0f;

        foreach (ItemStruct itemStruct in items)
        {
            if (rand < itemStruct.chance + temp)
            {
                if (itemStruct.item != null)
                {
                    GameObject g = Instantiate(itemStruct.item, new Vector2(2, 2), Quaternion.identity);
                    if (withPrice)
                    {
                        g.GetComponent<Item>().SetItem(itemStruct.prices[UnityEngine.Random.Range(0, itemStruct.prices.Count)]);
                    }
                    return g;
                }
            }
            temp += itemStruct.chance;
        }

        return null;
    }
}

