using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предмет "Бомба C4"
/// </summary>
public class Wireless_C4 : UseableItem
{
    public Bomb dropOnUse;
    private Bomb dropedBomb;
    bool bombDroped;
    protected override void Awake()
    {
        base.Awake();
        dropOnUse.time = useTime;
        charge = chargeMax;
    }

    public override void Use()
    {
        if (canUse)
        {
            if (!bombDroped)
            {
                bombDroped = true;
                isActive = true;
                dropedBomb = Instantiate(dropOnUse, transform.position, Quaternion.identity, null);
                
                RaiseItemUseEvent();
            }
            else
            {
                if (dropedBomb != null)
                {
                    canUse = false;
                    dropedBomb.ExplodeNow();
                    UnUse();
                }             
            }
        }
    }

    public override void UnUse()
    {
        base.UnUse();
        bombDroped = false;
    }
}
