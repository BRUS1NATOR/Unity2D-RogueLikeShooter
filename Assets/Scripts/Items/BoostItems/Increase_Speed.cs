using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase_Speed : PassiveItem
{
    public int speedUp;
    // Start is called before the first frame update
    protected override void DoStuff(NPC npc)
    {
        npc.stats.speed += speedUp;
    }
}
