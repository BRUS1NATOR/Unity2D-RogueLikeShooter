using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Destructible),true)]
public class DestructibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var myScript = target as Destructible;
        if (myScript.destructible)
        {
            myScript.breakWithAnimation = EditorGUILayout.Toggle("Break with animation: ", myScript.breakWithAnimation);

            serializedObject.FindProperty("_curHealth").intValue = EditorGUILayout.IntField("Current Health: ", myScript.curHealth);
            serializedObject.FindProperty("_maxHealth").intValue = EditorGUILayout.IntField("Max Health: ", myScript.maxHealth);

            myScript.destroyOnBrake =(GameObject)EditorGUILayout.ObjectField("Destroy on brake: ", myScript.destroyOnBrake, typeof(GameObject), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
