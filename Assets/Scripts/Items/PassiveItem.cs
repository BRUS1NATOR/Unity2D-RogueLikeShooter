using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предметы, единовременно изменяющие характеристики оружия или персонажа
/// </summary>
public class PassiveItem : Item
{
    protected override void Awake()
    {
        base.Awake();
        itemType = ItemTypes.PassiveItem;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.price < 1)
            {
                Give(collision.gameObject.GetComponent<NPC>());
            }
        }
    }

    public override bool Give(NPC npc)
    {
        bool ok = base.Give(npc);
        if (ok)
        {
            DoStuff(npc);
            RaiseItemSoldEvent();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Метод который изменяет характеристики перcонажа
    /// </summary>
    protected virtual void DoStuff(NPC npc)
    {

    }
}
