using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : PassiveItem
{
    public int health;

    public override bool Give(NPC npc)
    {
        if (npc.npcType == NPCType.Player)
        {
            int npcHp = npc.GetHp();
            int npcMaxHp = npc.stats.maxHealth;

            if (npcHp < npcMaxHp)
            {
                if (npc.GetComponent<Player>().TakeItem(this))
                {
                    npc.Heal(health);
                    RaiseItemSoldEvent();
                    Destroy(this.gameObject);
                    return true;
                }
            }
        }
        return false;
    }
}
