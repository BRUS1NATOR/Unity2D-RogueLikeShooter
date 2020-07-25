using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Здоровье NPC
/// </summary>
public class NPCHealth : MonoBehaviour, IHealth
{
    [SerializeField]
    private int _maxHealth;
    
    [SerializeField]
    private int _curHealth;
    [SerializeField]
    private int _maxArmor;
    [SerializeField]
    private int _curArmor;


    [SerializeField]
    private bool _ignoreImpulse;
    [SerializeField]
    private bool _pushBack;

    public int maxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int curHealth { get { return _curHealth; } set { _curHealth = value; } }
    public int maxArmor { get { return _maxArmor; } set { _maxArmor = value; } }
    public int curArmor { get { return _curArmor; } set { _curArmor = value; } }

    public bool ignoreBulletImpulse { get { return _ignoreImpulse; } set { _ignoreImpulse = value; } }
    public bool pushBack { get { return _pushBack; } set { _pushBack = value; } }

    protected virtual void Awake()
    {
        curHealth = maxHealth;
        curArmor = maxArmor;
    }

    public virtual void EffectDamage(float flame, EffectType effect)
    {
        
    }

    /// <summary>
    /// Поднять здоровье
    /// </summary>
    /// <param name="hp">Количество очков</param>
    /// <returns>Поднялось ли здоровье</returns>
    public bool Heal(int hp)
    {
        if (curHealth != maxHealth)
        {
            curHealth += hp;
            if (curHealth > maxHealth)
            {
                curHealth = maxHealth;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Увеличить максимально возможное здоровье
    /// </summary>
    /// <param name="hp">Количество очков</param>
    public virtual void IncreaseMaxHealth(int hp)
    {
        maxHealth += hp;
        curHealth += hp;
    }

    /// <summary>
    /// Увеличить броню
    /// </summary>
    /// <param name="hp">Количество очков</param>
    /// <returns>Поднялось ли здоровье</returns>
    public bool HealArmor(int hp)
    {
        if (curArmor != maxArmor)
        {
            curHealth += hp;
            if (curArmor > maxArmor)
            {
               curArmor = maxArmor;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Увеличить максимально возможное кол-во брони
    /// </summary>
    /// <param name="hp">Количество очков</param>
    public virtual void IncreaseMaxHealthArmor(int hp)
    {
        maxArmor += hp;
        curArmor += hp;
    }

    /// <summary>
    /// Получить урон
    /// </summary>
    /// <param name="hp">Количество очков урона</param>
    public virtual void TakeDamage(int hp)
    {

    }

    /// <summary>
    /// Оттолкнуть
    /// </summary>
    /// <param name="normalized">Направление толчка</param>
    /// <param name="power">Сила</param>
    public virtual void PushBack(Vector2 normalized, float power)
    {

    }
}
