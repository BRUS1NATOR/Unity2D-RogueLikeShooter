using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlask : UseableItem
{
    // Start is called before the first frame update
    public int health;
    protected override void Awake()
    {
        base.Awake();
        charge = chargeMax;
    }

    public override void Use()
    {
        if (canUse)
        {
            canUse = false;
            isActive = true;
            npc.Heal(health);

            RaiseItemUseEvent();
            charge = 0;
            UnUse();
        }
    }
   
}
