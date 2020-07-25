using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase_Health : PassiveItem
{
    public int health;

    protected override void DoStuff(NPC npc)
    {
        npc.IncreaseMaxHealth(health);
    }
}
