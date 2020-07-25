using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SeveralPoints
{
    public Transform startPos;
    public Transform endPos;
}
public class FireArmWeapon : Weapon
{
    public GameObject bulletObj;
    protected Bullet bullet;
    public float accuracy;

    public int bullet_speed;
    // Start is called before the first frame update

    protected override void PlaySound()
    {
        audioSource.PlayOneShot(shoot, 0.5f);
    }

    public override void Attack(params BulletMods[] mods)
    {
        if (magazine > 0)
        {
            if (coolDown == false && reloadCoolDown == false)
            {
                PlayShootAnim();
                PlaySound();

                magazine -= 1;
                foreach (SeveralPoints points in startAndEndPoints)
                {
                    BulletInfo bul = new BulletInfo(points.startPos.transform.position, points.endPos.transform.position, bullet_speed, damage, power, accuracy);
                    bullet = Instantiate(bulletObj, points.startPos.transform.position, Quaternion.identity).GetComponent<Bullet>();
                    bullet.Setup(bul, weaponItem.npc.bodySprite.transform.localScale.x, transform.rotation, belongsToPlayer, mods);
                }
            }
        }
    }
}
