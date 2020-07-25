using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChestRarity { common, rare, ultraRare, commonAmmo, rareAmmo, ultraRareAmmo, legendary }

[CreateAssetMenu]
public class ChestManager : ScriptableObject
{
    [Header("0.75 = 3/4 сундуков будут закрыты")]
    public float closedChance;

    [SerializeField]
    public Chest commonChest;
    [SerializeField]
    public Chest rareChest;
    [SerializeField]
    public Chest ultraRareChest;

    [SerializeField]
    public Chest commonAmmoChest;
    [SerializeField]
    public Chest rareAmmoChest;
    [SerializeField]
    public Chest ultraRareAmmoChest;

    [SerializeField]
    public Chest legendaryChest;

    public GameObject GetChestByType(ChestRarity rarity, bool closed)
    {
        Chest chest = null;
        switch (rarity)
        {
            case ChestRarity.common:
                chest = commonChest;
                break;
            case ChestRarity.rare:
                chest = rareChest;
                break;
            case ChestRarity.ultraRare:
                chest = ultraRareChest;
                break;
            case ChestRarity.commonAmmo:
                chest = commonAmmoChest;
                break;
            case ChestRarity.rareAmmo:
                chest = rareAmmoChest;
                break;
            case ChestRarity.ultraRareAmmo:
                chest = ultraRareAmmoChest;
                break;
            case ChestRarity.legendary:
                chest = legendaryChest;
                break;
            default:
                return null;
        }
        float rand = Random.Range(0, 1f);
        if (rand > closedChance)
        {
            chest.needKey = false;
        }
        else
        {
            chest.needKey = true;
        }

        GameObject g = Instantiate(chest.gameObject, new Vector2(2, 2), Quaternion.identity);
        return g;
    }
}
