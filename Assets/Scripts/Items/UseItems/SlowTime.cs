using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : UseableItem
{
    protected override void Awake()
    {
        base.Awake();
        chargeMax = useTime * chargeStep;
        charge = chargeMax;
    }

    public override void Use()
    {
        if (canUse)
        {
            canUse = false;
            isActive = true;
            GameManager.instance.gameTime = 0.5f;
            RaiseItemUseEvent();
            StartCoroutine(Time());
        }
    }

    private IEnumerator Time()
    {
        for (charge = charge; charge >= 0; charge -= chargeStep / 2)
        {
            yield return new WaitForSeconds(useTime * GameManager.instance.gameTime / (chargeMax / chargeStep * 2));
        }    
        UnUse();
    }

    public override void UnUse()
    {
        isActive = false;
        GameManager.instance.gameTime = 1f;
        RaiseItemUnUseEvent();
    }
}
