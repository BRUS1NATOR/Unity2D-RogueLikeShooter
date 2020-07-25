using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Типы предметов
/// </summary>
public enum ItemRarity {
    ammo, heals, boosts, modificators,
    commonItems, rareItems, legendaryItems,
    commonWeapons, rareWeapons, legendaryWeapons,
    weapons, useItems, other,
    nothing }


/// <summary>
/// Структура содержащая предмет
/// </summary>
[Serializable]
public struct ItemsDbStruct
{
    public ItemsSet itemsDb;
    public float[] chance;
}

/// <summary>
/// Объект хранящий всевозможные предметы
/// </summary>
[CreateAssetMenu]
public class ItemManager : ScriptableObject
{
    /// <summary>
    /// Текущая фаза
    /// </summary>
    public int phaseNow = 0; // 0 - 1 уровень; 1 - 2 уровень; 2 - 3..5 уровни;

    /// <summary>
    /// Максимальная фаза
    /// </summary>
    public int phaseMax = 2;

    /// <summary>
    /// Сумма всех шансов
    /// </summary>
    private float AllChance;

    public GameObject itemLight;
    public Material highLightMaterial;

    /// <summary>
    /// Ничего
    /// </summary>
    [HideInInspector]
    public ItemsDbStruct nothing;

    /// <summary>
    /// Предметы содержащие патроны
    /// </summary>
    [SerializeField]
    public ItemsDbStruct ammoItems;

    /// <summary>
    /// Лечащие предметы
    /// </summary>
    [SerializeField]
    public ItemsDbStruct healItems;

    /// <summary>
    /// Бусты
    /// </summary>
    [SerializeField]
    public ItemsDbStruct common_boostItems;
    /// <summary>
    /// Редкие бусты
    /// </summary>
    [SerializeField]
    public ItemsDbStruct rare_BoostItems;
    /// <summary>
    /// Обычные модификаторы
    /// </summary>
    [SerializeField]
    public ItemsDbStruct common_ModificatorItems;
    /// <summary>
    /// Редкие модификаторы
    /// </summary>
    [SerializeField]
    public ItemsDbStruct rare_ModificatorItems;

    /// <summary>
    /// Обычные предметы
    /// </summary>
    [SerializeField]
    public ItemsDbStruct common_Items;
    /// <summary>
    /// Редкие предметы
    /// </summary>
    [SerializeField]
    public ItemsDbStruct rare_Items;
    /// <summary>
    /// Легендарные предметы
    /// </summary>
    [SerializeField]
    public ItemsDbStruct legendary_Items;

    /// <summary>
    /// Обычное оружие
    /// </summary>
    [SerializeField]
    public ItemsDbStruct common_Weapons;
    /// <summary>
    /// Редкое оружие
    /// </summary>
    [SerializeField]
    public ItemsDbStruct rare_Weapons;
    /// <summary>
    /// Легендарное оружие
    /// </summary>
    [SerializeField]
    public ItemsDbStruct legendary_Weapons;

    /// <summary>
    /// Синглтон
    /// </summary>
    public static ItemManager instance;

    /// <summary>
    /// Инициализация синглтона
    /// </summary>
    public static void Initialize()
    {
        instance = Resources.Load<ItemManager>("Item_Manager");
    }

    /// <summary>
    /// Инициализация синглтона
    /// </summary>
    public List<ItemsDbStruct> GetItemsDb(ItemRarity rarity)
    {
        List<ItemsDbStruct> ITEMS = new List<ItemsDbStruct>();
        switch (rarity)
        {
            case ItemRarity.ammo:   // Патроны
                ITEMS.Add(ammoItems);
                break;
            case ItemRarity.heals:  // Аптечки
                ITEMS.Add(healItems);
                break;
            case ItemRarity.boosts: // Улучшения
                ITEMS.Add(common_boostItems);
                break;
            case ItemRarity.modificators: // Модификаторы
                ITEMS.Add(common_ModificatorItems);
                ITEMS.Add(rare_ModificatorItems);
                break;
            case ItemRarity.commonItems: // Обычные предметы
                ITEMS.Add(common_Items);
                break;
            case ItemRarity.rareItems: // Редкие предметы
                ITEMS.Add(rare_Items);
                break;
            case ItemRarity.legendaryItems: // Легендарные предметы
                ITEMS.Add(legendary_Items);
                break;
            case ItemRarity.commonWeapons: // Обычное оружие
                ITEMS.Add(common_Weapons);
                break;
            case ItemRarity.rareWeapons: // Редкое оружие
                ITEMS.Add(rare_Weapons);
                break;
            case ItemRarity.legendaryWeapons: //  Легенадрное оружие
                ITEMS.Add(legendary_Weapons);
                break;

            case ItemRarity.weapons: //  Оружие
                ITEMS.Add(common_Weapons);
                ITEMS.Add(rare_Weapons);
                ITEMS.Add(legendary_Weapons);
                break;
            case ItemRarity.useItems: //  Предметы
                ITEMS.Add(common_Items);
                ITEMS.Add(rare_Items);
                ITEMS.Add(legendary_Items);
                break;
            case ItemRarity.other: //  Остальное
                ITEMS.Add(healItems);
                ITEMS.Add(ammoItems);
                ITEMS.Add(common_boostItems);
                ITEMS.Add(common_ModificatorItems);
                break;

            case ItemRarity.nothing:
                ITEMS.Add(nothing);
                break;

            default:
                ITEMS.Add(common_Items);
                break;
        }
        return ITEMS;
    }

    public float GetOverAllChance(List<ItemsDbStruct> itemsDb)
    {
        float overall = 0f;

        foreach (ItemsDbStruct db in itemsDb)
        {
            overall += db.chance[phaseNow];
        }
        return overall;
    }

    public float[] GetChances(ItemRarity rarity, int phase) // [0] = overall [1]..
    {
        List<ItemsDbStruct> itemsDb = GetItemsDb(rarity);
        if (itemsDb != null)
        {
            float[] chances = new float[itemsDb.Count + 1];
            float overall = 0f;
            for (int i = 0; i < itemsDb.Count; i++)
            {
                chances[i + 1] = itemsDb[i].chance[phase];
                overall += itemsDb[i].chance[phase];
            }
            chances[0] = overall;
            return chances;
        }
        return null;
    }

    public static GameObject GetItem(List<ItemRarity> itemsType, bool withPrice, bool withLight)
    {
        float overAllChance = 0f;

        foreach (ItemRarity type in itemsType)
        {
            List<ItemsDbStruct> tmp = instance.GetItemsDb(type);
            overAllChance += instance.GetOverAllChance(tmp);
        }

        float rand = UnityEngine.Random.Range(0f, overAllChance);
        float temp = 0f;
        float chance;

        List<ItemsDbStruct> items = new List<ItemsDbStruct>();

        foreach (ItemRarity type in itemsType)
        {
            items = instance.GetItemsDb(type);
            chance = instance.GetOverAllChance(items);
            if (rand < chance + temp)
            {
                break;
            }
            temp += overAllChance;
        }
        GameObject item = instance.GetRandomItem(items, withPrice);

        if (item.GetComponent<Item>() != null)
        {

            if (withLight)
            {
                GameObject light = Instantiate(instance.itemLight);
                light.transform.parent = item.transform;
                light.transform.localPosition = new Vector3(0, 0, 9);
            }
        }

        return item;
    }

    private GameObject GetRandomItem(List<ItemsDbStruct> itemsDb, bool withPrice)
    {
        float overAllChance = 0f;


        foreach (ItemsDbStruct it in itemsDb)
        {
            overAllChance += it.chance[phaseNow];
        }

        ItemsDbStruct items = new ItemsDbStruct();
        float rand = UnityEngine.Random.Range(0f, overAllChance);
        float temp = 0f;

        foreach (ItemsDbStruct itemStr in itemsDb)
        {
            items = itemStr;
            float chance = itemStr.chance[phaseNow];
            if (rand < chance + temp)
            {
                break;
            }
            temp += chance;
        }

        if (items.itemsDb != null)
        {
            GameObject item = items.itemsDb.GetRandomItem(withPrice);
         //   Debug.Log("Overall chance: " + overAllChance + " Rand: " + rand + " Item: " + item.name);
            return item;
        }
        else
        {
            return new GameObject();
        }
    }

    public void CreateItem(GameObject item, Vector2 position, int price)
    {
        GameObject i = Instantiate(item, position, Quaternion.identity);
        i.GetComponent<Item>().SetItem(price);
    }

    public void CreateItem(GameObject item, Vector2 position)
    {
        GameObject i = Instantiate(item, position, Quaternion.identity);
    }

    public float CalculateOverallChances(int phase)
    {
        AllChance = 0;
        AllChance += ammoItems.chance[phase];
        AllChance += healItems.chance[phase];

        AllChance += common_boostItems.chance[phase];
        AllChance += rare_BoostItems.chance[phase];

        AllChance += common_ModificatorItems.chance[phase];
        AllChance += rare_ModificatorItems.chance[phase];

        AllChance += common_Items.chance[phase];
        AllChance += rare_Items.chance[phase];
        AllChance += legendary_Items.chance[phase];

        AllChance += common_Weapons.chance[phase];
        AllChance += rare_Weapons.chance[phase];
        AllChance += legendary_Weapons.chance[phase];

        AllChance += nothing.chance[phase];

        return AllChance;
    }
}