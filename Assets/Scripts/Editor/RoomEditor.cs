using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    Room room;
    GameManager manager;

    List<GameObject> _choices = new List<GameObject>();
    string[] _choicesNames;
    int _choiceIndex = 0;

    GUIContent but = new GUIContent("DropDown");
    GUILayoutOption[] gui = {GUILayout.Width(100) };
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Kill Enemies"))
        {
            for (int i = room.Enemies.Count - 1; i >= 0; i--)
            {
                room.Enemies[i].TakeDamage(10000);
            }
        }

        if (GUILayout.Button("Teleport Player"))
        {
            GameManager.instance.TeleportPlayer(room);
        }

        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choicesNames);
        if (GUILayout.Button("Spawn Enemy"))
        {
            EnemiesManager.SpawnEnemy(room, _choices[_choiceIndex]);
        }
    }

    void OnEnable()
    {
        if (room == null)
        {
            room = (Room)target;
        }

        if (manager == null)
        {
            manager = GameManager.instance;
        }
        if (EnemiesManager.instance != null)
        {

            if (_choices.Count == 0)
            {
                List<string> tmp = new List<string>();
                for (int i = 0; i < EnemiesManager.instance.BossEnemies.enemies.Count; i++)
                {
                    _choices.Add(EnemiesManager.instance.BossEnemies.enemies[i].enemy);
                    tmp.Add(EnemiesManager.instance.BossEnemies.enemies[i].enemy.name);
                }
                for (int i = 0; i < EnemiesManager.instance.RangeEnemies.enemies.Count; i++)
                {
                    _choices.Add(EnemiesManager.instance.RangeEnemies.enemies[i].enemy);
                    tmp.Add(EnemiesManager.instance.RangeEnemies.enemies[i].enemy.name);
                }
                for (int i = 0; i < EnemiesManager.instance.CQCEnemies.enemies.Count; i++)
                {
                    _choices.Add(EnemiesManager.instance.CQCEnemies.enemies[i].enemy);
                    tmp.Add(EnemiesManager.instance.CQCEnemies.enemies[i].enemy.name);
                }
                _choicesNames = tmp.ToArray();
            }
        }
    }
}