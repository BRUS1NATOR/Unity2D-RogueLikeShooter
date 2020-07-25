using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemManager))]
public class ItemManagerEdit : Editor
{
    ItemManager manager;



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        for (int i = 0; i <= manager.phaseMax; i++)
        {
            GuiLine(Color.black, 8);
            GUILayout.Label("PHASE № " + i);
            float overallChance = manager.CalculateOverallChances(i);

            EditorGUILayout.TextArea("Overall: " + overallChance.ToString());

            float chance = manager.nothing.chance[i];
            EditorGUILayout.TextArea("Nothing chance: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");


            chance = manager.ammoItems.chance[i];
            EditorGUILayout.TextArea("Ammo: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");

            chance = manager.healItems.chance[i];
            EditorGUILayout.TextArea("Heal: " + SimpleFunctions.CountPercent(chance, overallChance) + " % (" + chance + ")");


            chance = manager.common_boostItems.chance[i];
            EditorGUILayout.TextArea("Common Boosts: " + SimpleFunctions.CountPercent(chance, overallChance) + " % (" + chance + ")");
            chance = manager.rare_BoostItems.chance[i];
            EditorGUILayout.TextArea("Rare Boosts: " + SimpleFunctions.CountPercent(chance, overallChance) + " % (" + chance + ")");


            chance = manager.common_ModificatorItems.chance[i];
            EditorGUILayout.TextArea("Common Mods: " + SimpleFunctions.CountPercent(chance, overallChance) + " % (" + chance + ")");
            chance = manager.rare_ModificatorItems.chance[i];
            EditorGUILayout.TextArea("Rare Mods: " + SimpleFunctions.CountPercent(chance, overallChance) + " % (" + chance + ")");


            chance = manager.common_Items.chance[i];
            EditorGUILayout.TextArea("Common items: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");
            chance = manager.rare_Items.chance[i];
            EditorGUILayout.TextArea("Rare items: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");
            chance = manager.legendary_Items.chance[i];
            EditorGUILayout.TextArea("Legendary items: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");


            chance = manager.common_Weapons.chance[i];
            EditorGUILayout.TextArea("Common weapons: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");

            chance = manager.rare_Weapons.chance[i];
            EditorGUILayout.TextArea("Rare weapons: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");

            chance = manager.legendary_Weapons.chance[i];
            EditorGUILayout.TextArea("Legendary weapons: " + SimpleFunctions.CountPercent(chance, overallChance) + "% (" + chance + ")");


            GuiLine(4);
            float[] weapChances = manager.GetChances(ItemRarity.weapons,i);
            EditorGUILayout.TextArea("Weapons: " + SimpleFunctions.CountPercent(weapChances[0], overallChance) + "% (" + weapChances[0] + ")");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextArea("Com: " + SimpleFunctions.CountPercent(weapChances[1], weapChances[0]).ToString("F" + 1) + "%(" + weapChances[1] + ")");
            EditorGUILayout.TextArea("Rare: " + SimpleFunctions.CountPercent(weapChances[2], weapChances[0]).ToString("F" + 1) + "%(" + weapChances[2] + ")");
            EditorGUILayout.TextArea("Legend: " + SimpleFunctions.CountPercent(weapChances[3], weapChances[0]).ToString("F" + 1) + "%(" + weapChances[3] + ")");
            EditorGUILayout.EndHorizontal();

            GuiLine(4);
            float[] useItemsChances = manager.GetChances(ItemRarity.useItems, i);
            EditorGUILayout.TextArea("UseItems: " + SimpleFunctions.CountPercent(useItemsChances[0], overallChance) + "% (" + useItemsChances[0] + ")");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextArea("Com: " + SimpleFunctions.CountPercent(useItemsChances[1], useItemsChances[0]).ToString("F" + 1) + "%(" + useItemsChances[1] + ")");
            EditorGUILayout.TextArea("Rare: " + SimpleFunctions.CountPercent(useItemsChances[2], useItemsChances[0]).ToString("F" + 1) + "%(" + useItemsChances[2] + ")");
            EditorGUILayout.TextArea("UltraRare: " + SimpleFunctions.CountPercent(useItemsChances[3], useItemsChances[0]).ToString("F" + 1) + " %(" + useItemsChances[3] + ")");
            EditorGUILayout.EndHorizontal();

            GuiLine(4);
            float[] otherChances = manager.GetChances(ItemRarity.other, i);
            EditorGUILayout.TextArea("Other: " + SimpleFunctions.CountPercent(otherChances[0], overallChance) + "% (" + otherChances[0] + ")");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextArea("Heal: " + SimpleFunctions.CountPercent(otherChances[1], otherChances[0]).ToString("F" + 1) + "%(" + otherChances[1] + ")");
            EditorGUILayout.TextArea("Ammo: " + SimpleFunctions.CountPercent(otherChances[2], otherChances[0]).ToString("F" + 1) + "%(" + otherChances[2] + ")");
            EditorGUILayout.TextArea("Boost: " + SimpleFunctions.CountPercent(otherChances[3], otherChances[0]).ToString("F" + 1) + " %(" + otherChances[3] + ")");
            EditorGUILayout.TextArea("Mods: " + SimpleFunctions.CountPercent(otherChances[4], otherChances[0]).ToString("F" + 1) + " %(" + otherChances[4] + ")");
            EditorGUILayout.EndHorizontal();


        }
    }

    void OnEnable()
    {
        if (manager == null)
        {
            manager = (ItemManager)target;
        }
    }

    void GuiLine(Color color, int i_height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, color);
    }

    void GuiLine(int i_height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}

