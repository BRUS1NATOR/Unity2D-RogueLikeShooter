using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType { Flame, Water, Slime, Oil }


[Serializable]
public class EffectManager
{
    public StatsManager npc;

    [Header("1 = flame  |  -1 = water")]
    public float flameEffect = 0f;
    public GameObject flameObj;

    [Header("Cool Down per 0.5 seconds")]
    public float coolDownFlameEffect;

    [Header("1 = slime  |  -1 = oil")]
    public float slimeEffect = 0f;
    [Header("Cool Down per 0.5 seconds")]
    public float coolDownSlimeEffect;

    private bool flameAffected = false;
    public bool flameImmune;

    private bool waterAffected = false;
    public bool waterImmune;

    private bool slimeAffected = false;
    public bool slimeImmune;

    private bool oilAffected = false;
    public bool oilImmune;

    public void AddFlameEffect(float value)
    {
        if (!flameImmune)
        {
            flameEffect += value;
            if (flameEffect > 1)
            {
                flameEffect = 1;
            }
        }
    }

    public void AddWaterEffect(float value)
    {
        if (!waterImmune)
        {
            if (flameEffect > 0)
            {
                flameEffect -= value * 2;
            }
            else
            {
                flameEffect -= value;
            }
        }
        if (flameEffect < -1)
        {
            flameEffect = -1;
        }
    }

    

    public void AddSlimeEffect(float value)
    {
        if (!slimeImmune)
        {
            slimeEffect += value;

            if (slimeEffect > 1)
            {
                slimeEffect = 1;
            }
        }
    }

    public void AddOilEffect(float value)
    {
        if (!oilImmune)
        {
            slimeEffect -= value;

            if (slimeEffect < -1)
            {
                slimeEffect = -1;
            }
        }
    }

    public IEnumerator CheckEffects()
    {
        while (npc.npc.isAlive)
        {
            float prevFlameEffect = flameEffect;
            float prevSlimeEffect = slimeEffect;
            yield return new WaitForSeconds(0.5f);

            if(prevFlameEffect == flameEffect)     
            {
                flameEffect = ReduceEffect(flameEffect,coolDownFlameEffect);
            }

            if (flameEffect > -0.75f && flameEffect < 0.75f)
            {
                WaterAffect(false);
                FlameAffect(false);
            }
            else if(flameEffect<-0.75f)
            {
                WaterAffect(true);
            }
            else
            {
                FlameAffect(true);
            }

            if (prevSlimeEffect == slimeEffect)
            {
                slimeEffect = ReduceEffect(slimeEffect,coolDownSlimeEffect);
            }

            if (slimeEffect > -0.75f && slimeEffect < 0.75f)
            {
                OilAffect(false);
                SlimeAffect(false);
            }
            else if(slimeEffect < -0.75f)
            {
                OilAffect(true);
            }

            else
            {
                SlimeAffect(true);
            }
        }
    }

    private float ReduceEffect(float value, float step)
    {
        if (value > 0)
        {
            value -= step;
            if (value < 0)
            {
                return 0;
            }
            return value;
        }
        else if(value < 0)
        {
            value += step;
            if (value > 0)
            {
                return 0;
            }
            return value;
        }
        return 0;
    }

    void FlameAffect(bool flame)
    {
        if (flame)
        {
            flameObj.SetActive(true);
            if (flameImmune == false)
            {
                npc.TakeDamage(6);
                flameAffected = true;
            }
        }
        else
        {
            flameAffected = false;
            flameObj.SetActive(false);
        }
    }
    void WaterAffect(bool swim)
    {
        if (swim && waterAffected==false)
        {
            waterAffected = true;
            npc.speed /= 1.25f;
        }
        else if(swim == false && waterAffected ==true)
        {
            waterAffected = false;
            npc.speed *= 1.25f;
        }
    }
    void OilAffect(bool oil)
    {
        if(oil && oilAffected == false)
        {
            oilAffected = true;
            npc.speed *= 1.25f;
        }
        else if(oil==false && oilAffected == true)
        {
            oilAffected = false;
            npc.speed /= 1.25f;
        }
    }
    void SlimeAffect(bool slime)
    {
        if (slime && slimeAffected == false)
        {
            slimeAffected = true;
            npc.speed /= 1.5f;
        }
        else if (slime == false && slimeAffected == true)
        {
            slimeAffected = false;
            npc.speed *= 1.5f;
        }

    }
}

public class StatsManager : NPCHealth
{
    public NPC npc;

    [SerializeField]
    public EffectManager effects = new EffectManager();

    [SerializeField]
    private float Speed;
    public float speed
    {
        get
        {
            return Speed;
        }
        set
        {
            Speed = value;
            if (npc.npcType == NPCType.Enemy)
            {
                npc.movement.navAgent.speed = value;
            }
        }
    }

    protected override void Awake()
    {
        curHealth = maxHealth;

        StartCoroutine(effects.CheckEffects());
    }

    public override void EffectDamage(float value, EffectType effect)
    {
        AddEffect(effect, value);
    }

    public override void TakeDamage(int hp)
    {
        npc.TakeDamage(hp);
    }

    public override void PushBack(Vector2 normalized, float power)
    {
        npc.movement.PushBack(normalized * power*3, power*0.1f);
    }

    public void AddEffect(EffectType areaType, float value)
    {
        switch (areaType)
        {
            case EffectType.Flame:
                {
                    effects.AddFlameEffect(value);
                    break;
                }
            case EffectType.Water:
                {
                    effects.AddWaterEffect(value);
                }
                break;

            case EffectType.Slime:
                {
                    effects.AddSlimeEffect(value);

                    break;
                }
            case EffectType.Oil:
                {
                    effects.AddOilEffect(value);

                    break;
                }
            default:
                {
                    effects.AddWaterEffect(value);
                }
                break;
        }
    }
}
