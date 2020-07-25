using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Modificator { none, regenerateHealthWhenKilling, fireResist, slimeResist }
public enum WeaponModification { none, circle, fire, slime, fireArea, slimeArea }

public struct BulletMods
{
    public WeaponModification mod;
    public float power;
}

public struct NPCMods
{
    public Modificator mod;
    public float power;
}

/// <summary>
/// Предметы "Модификаторы", единовременно изменяющие характеристики оружия или персонажа
/// </summary>
public class ModificatorItem : PassiveItem
{
    /// <summary>
    /// Коэфициент изменения
    /// </summary>
    public float power;

    /// <summary>
    /// Является ли модификация, модификацией оружия
    /// </summary>
    public bool isBulletModification;

    /// <summary>
    /// Модификация
    /// </summary>
    public Modificator modification;

    /// <summary>
    /// Модификация оружия
    /// </summary>
    public WeaponModification weaponModification;

    /// <summary>
    /// Модификация оружия
    /// </summary>
    protected override void DoStuff(NPC npc)
    {
        if (isBulletModification)
        {
            npc.AddBulletModification(weaponModification, power);
        }
        else
        {
            npc.AddModification(modification, power);
        }
        RaiseItemSoldEvent();
        Destroy(this.gameObject);
    }
}
