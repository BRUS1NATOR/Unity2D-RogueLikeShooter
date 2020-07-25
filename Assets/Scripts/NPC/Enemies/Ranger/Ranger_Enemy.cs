using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger_Enemy : Enemy
{
    protected override void PlayerEntersRoom(System.Object sender, EventArgs e)
    {
        StartToShoot();
    }

    protected override void PlayerLeavesRoom(System.Object sender, EventArgs e)
    {
        StopToShoot();
    }

    protected virtual void FixedUpdate()
    {
        lookAt = Player.instance.transform.position - transform.position;

        if (movement.playerInTheRoom)
        {
            if (stats.pushBack == false)
            {
                if (isTouchingPlayer == true)
                {
                    Player.instance.TakeDamage(1);
                }
            }
        }
    }

    //protected override void CheckForPath()
    //{
    //    if (Player.instance.room != null && dontMove == false)
    //    {
    //        if (Player.instance.room.number == room.number)
    //        {
    //            movement.SeekForPath();
    //            movement.CastRay();
    //        }
    //        else
    //        {
    //            movement.Sleep();
    //        }
    //    }
    //}
}
