using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    protected override void DoDamage(Collider2D collider)
    {
        Explode();
    }

    protected override void WallCollision()
    {
        Explode();
    }

    private void Explode()
    {
        if (ready)
        {
            ready = false;
            StartCoroutine(PlayAnimationAndDestroy());
            ExplosionManager.Explode(transform.position, 3, 50, power, ExplosionManager.ExplosionType.bomb);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
