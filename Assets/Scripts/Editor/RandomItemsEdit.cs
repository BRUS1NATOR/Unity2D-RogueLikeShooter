using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomItems))]
public class RandomItemsEdit : Editor
{
    ItemManager manager;
    RandomItems rndItems;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (rndItems.items.Count != 0)
        {
            float temp = 0f;
            foreach (ItemRarity itemRarity in rndItems.items)
            {
                temp += manager.GetOverAllChance(manager.GetItemsDb(itemRarity));
            }
            foreach (ItemRarity itemRarity in rndItems.items)
            {
                float chance = manager.GetOverAllChance(manager.GetItemsDb(itemRarity));
                EditorGUILayout.TextArea(itemRarity.ToString() + " chance: " + SimpleFunctions.CountPercent(chance,temp) +"% (" + chance + ")");
            }
            EditorGUILayout.TextArea("Overall chance: " + temp);
        }
    }

    void OnEnable()
    {
        if (rndItems == null)
        {
            rndItems = (RandomItems)target;
        }

        if (manager == null)
        {
            manager = GetManager();
        }
    }

    public static ItemManager GetManager()
    {
        var ItemManager = Resources.Load<ItemManager>("Item_Manager");
        return ItemManager;
    }
}
