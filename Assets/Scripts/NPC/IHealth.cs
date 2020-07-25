using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Интерфейс здоровья
/// </summary>
interface IHealth
{
    int maxHealth { get; set; }
    int curHealth { get; set; }

    bool ignoreBulletImpulse { get; set; }
    bool pushBack { get; set; }

    void TakeDamage(int dmg);
    void EffectDamage(float flame, EffectType effect);
    bool Heal(int hp);
    void IncreaseMaxHealth(int hp);
    void PushBack(Vector2 normalized, float power);
}