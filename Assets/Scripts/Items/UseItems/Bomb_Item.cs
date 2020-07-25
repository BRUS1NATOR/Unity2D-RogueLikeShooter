using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предмет "Бомба"
/// </summary>
public class Bomb_Item : UseableItem
{
    public Bomb dropOnUse;
    protected override void Awake()
    {
        base.Awake();
        charge = chargeMax;
        dropOnUse.time = useTime;
    }

    public override void Use()
    {
        if (canUse)
        {
            canUse = false;
            isActive = true;

            Instantiate(dropOnUse,transform.position,Quaternion.identity,null);

            RaiseItemUseEvent();
            charge = 0;
            UnUse();
        }
    }
}
