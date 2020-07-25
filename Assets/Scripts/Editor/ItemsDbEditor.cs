using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemsSet))]
public class ItemsDbEdit : Editor
{
    ItemsSet items;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (items != null)
        {
            EditorGUILayout.LabelField("Overall Chance: " + items.maxchance.ToString());
        }
    }

    void OnEnable()
    {
        if (items == null)
        {
            items = (ItemsSet)target;
        }
    }
}