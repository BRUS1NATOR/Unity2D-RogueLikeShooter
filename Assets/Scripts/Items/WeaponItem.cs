using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предмет содержащий оружие
/// </summary>
public class WeaponItem : Item
{
    /// <summary>
    /// Оружие которое хранит предмет
    /// </summary>
    public Weapon weapon;

    protected override void Awake()
    {
        base.Awake();
        itemType = ItemTypes.WeaponItem;
        name = transform.name;
        

        if (item_renderer != null)
        {
            item_renderer.enabled = true;
        }

        weapon = GetComponentInChildren<Weapon>();
        weapon.Hide();
    }


    public override bool Give(NPC npc)
    {
        bool took = npc.TakeItem(this);

        if (took)
        {
            RaiseItemSoldEvent();

            HideItem();

            if (npc.npcType == NPCType.Player)
            {
                GetComponentInChildren<Weapon>().belongsToPlayer = true;
            }

            else if (npc.npcType == NPCType.Enemy)
            {
                weapon.infiniteAmmo = true;
                weapon.Show();
                GetComponentInChildren<Weapon>().belongsToPlayer = false;
            }

            if (npc.movement.body != null)
            {
                if (weapon.oneHanded == true)
                {
                    if (npc.movement.body.hands != null)
                    {
                        SetParent(npc,npc.movement.body.hands.frontHand.transform);
                        return true;
                    }

                }
                else
                {
                    if (npc.movement.body.hands != null)
                    {
                        SetParent(npc, npc.movement.body.hands.rearHand.transform);
                        return true;
                    }
                }
            }

            SetParent(npc, npc.transform);

            return true;
        }
        return false;
    }

    public override void Drop()
    {
        weapon.Hide();
        ShowItem();
    }
}
