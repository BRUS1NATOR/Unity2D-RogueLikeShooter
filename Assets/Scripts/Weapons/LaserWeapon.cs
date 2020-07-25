using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    public Laser laser;
    private int mask;



    private Coroutine waitForDest;
    private bool attack;


    protected override void Awake()
    {
        base.Awake();
        mask = 1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("HitBox") | 1 << LayerMask.NameToLayer("Environment");

        laser.rayCast = true;
        laser.lineWidth = 0.15f;
    }

    void FixedUpdate()
    {
        if (attack)
        {
            CastRay();
        }
    }

    protected override void PlaySound()
    {
        audioSource.clip = shoot;
        audioSource.Play();
        GameManager.instance.audioManager.AddLiveSource(audioSource);
    }

    protected override void StopSound()
    {
        audioSource.Stop();
        GameManager.instance.audioManager.RemoveLiveSource(audioSource);
    }

    public override void Attack(params BulletMods[] mods)
    {
        if (magazine > 0)
        {
            attack = true;
            if (waitForDest == null)
            {
                PlaySound();
                waitForDest = StartCoroutine(WaitForDestroy());
            }
        }
        else
        {
            attack = false;
            laser.Hide();
        }
    }


    void CastRay()
    {
        attack = false;
        magazine -= 1;

        RaycastHit2D[] hits = laser.Show(startAndEndPoints[0].startPos.position, startAndEndPoints[0].endPos.position,belongsToPlayer);

        if (hits != null)
        {
            for (int i = 0; i < hits.Length && i < 2; i++)
            {
                if (!belongsToPlayer == (hits[i].collider.name == "PlayerHitBox"))
                {
                    DoDamage(hits[i].collider);

                    break;
                }
            }
        }
    }


    protected virtual void DoDamage(Collider2D collider)
    {
        IHealth objectWithHealth = collider.gameObject.GetComponent<IHealth>();
        if (objectWithHealth != null)
        {
            if (power > 0)
            {
                Vector2 normalized = -(transform.position - collider.gameObject.transform.position).normalized;
                if (objectWithHealth.ignoreBulletImpulse == false)
                {
                    objectWithHealth.PushBack(normalized, (float)power);
                }
                //if (flameDamage > 0)
                //{
                //    objectWithHealth.EffectDamage(flameDamage, EffectType.Flame);
                //}
                objectWithHealth.TakeDamage(damage);
            }
            else
            {
                //if (flameDamage > 0)
                //{
                //    objectWithHealth.EffectDamage(flameDamage, EffectType.Flame);
                //}
                objectWithHealth.TakeDamage(damage);
            }

        }
    }

    IEnumerator WaitForDestroy()
    {
        while (attack)
        {
            yield return new WaitForSeconds(0.05f);
        }
        StopSound();
        laser.Hide();
        waitForDest = null;
    }
}
