using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_RefillAll : PassiveItem
{
    public float percentToRefill;

    protected override void DoStuff(NPC npc)
    {
        foreach (WeaponItem weaponItem in npc.weapons)
        {
            if (weaponItem.weapon != null)
            {
                int refill = Mathf.CeilToInt(weaponItem.weapon.ammoMax * percentToRefill);
                weaponItem.weapon.ammo += refill;
            }
        }
        RaiseItemSoldEvent();
        Destroy(this.gameObject);
    }
}
