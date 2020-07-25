using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessed_Shield : UseableItem
{
    public Material forceField;
    protected override void Awake()
    {
        base.Awake();
        chargeMax = useTime * chargeStep;
        charge = chargeMax;
    }

    /// <summary>
    /// Использовать предмет
    /// </summary>
    /// <returns></returns>
    public override void Use()
    {
        if (canUse)
        {
            canUse = false;
            isActive = true;
            Player.instance.invicible = true;
            RaiseItemUseEvent();
            StartCoroutine(ForceFieldShader());
            StartCoroutine(ForceField());
        }
    }

    private IEnumerator ForceField()
    {
        for (charge = charge; charge >= 0; charge -= chargeStep / 2)
        {
            yield return new WaitForSeconds(0.5f);
        }
        charge = 0;
        UnUse();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ForceFieldShader()
    {
        Material def = npc.bodySprite.material;
        npc.bodySprite.material = forceField;
        float pos = 1f;
        while (isActive)
        {
            pos -= 0.1f;
            if (pos < 0)
            {
                pos = 1f;
            }
            forceField.SetFloat("_Position", pos);
            yield return new WaitForSeconds(0.1f);
        }
        npc.bodySprite.material = def;
    }

    public override void UnUse()
    {
        isActive = false;
        Player.instance.invicible = false;
        RaiseItemUnUseEvent();
    }
}
