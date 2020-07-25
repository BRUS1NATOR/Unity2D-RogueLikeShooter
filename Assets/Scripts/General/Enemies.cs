using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyStruct
{
    public GameObject enemy;
    public float chance;

    [Header("Уровень начиная с которого будет появляться враг")]
    public int level;

    [Header("Уровни на котором желательно должен появиться враг")]
    public int targetLevel;
}

[CreateAssetMenu]
public class Enemies : ScriptableObject
{
    public List<EnemyStruct> enemies;

    public GameObject GetRandomEnemy(int levelNow)
    {
        GameObject en=null;
        float overallChance = 0;
        float levelAffect = UnityEngine.Random.Range(0f, 1f);

        if(levelAffect>0 && levelAffect < 0.5f) //Взять врага только с таргет левелом
        {
            foreach (EnemyStruct enemyStruct in enemies)
            {
                if (enemyStruct.targetLevel == levelNow)
                {
                    overallChance += enemyStruct.chance;
                }
            }
            if (overallChance != 0)
            {
                en = GetByTargetLevel(levelNow, overallChance);
            }
        }

        if(overallChance == 0)  //Взять любого врага больше либо равного текущему уровню
        {
            foreach (EnemyStruct enemyStruct in enemies)
            {
                if (enemyStruct.level <= levelNow)
                {
                    {
                        overallChance += enemyStruct.chance;
                    }
                }
            }
            if (overallChance != 0)  // (100% не равно 0)
            {
                en = GetByLevel(levelNow, overallChance);
            }
        }

        if (en == null)
        {
            Debug.Log("GET ENEMY = NULL!! overallChance = " + overallChance);
        }
        return en;
    }

    private GameObject GetByTargetLevel(int levelNow, float maxChance)
    {
        float rand = UnityEngine.Random.Range(0f, maxChance);
        float temp = 0f;

        foreach (EnemyStruct enemyStruct in enemies)
        {
            if (enemyStruct.targetLevel == levelNow)
            {
                temp += enemyStruct.chance;
                if (rand < temp)
                {
                    if (enemyStruct.enemy != null)
                    {
                        return enemyStruct.enemy;
                    }
                }
            }
        }
        return null;
    }
    private GameObject GetByLevel(int levelNow, float maxChance)
    {
        float rand = UnityEngine.Random.Range(0f, maxChance);
        float temp = 0f;

        foreach (EnemyStruct enemyStruct in enemies)
        {
            if (enemyStruct.level <= levelNow)
            {
                temp += enemyStruct.chance;
                if (rand <= temp)
                {
                    if (enemyStruct.enemy != null)
                    {
                        return enemyStruct.enemy;
                    }
                }
            }
        }
        return null;
    }
}
