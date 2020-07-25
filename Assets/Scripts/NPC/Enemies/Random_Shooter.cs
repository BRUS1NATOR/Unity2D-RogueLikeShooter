using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Shooter : Enemy
{
    public int bulSpeed;
    public GameObject bulletObj;

    protected override void Start()
    {
        base.Start();
        WakeUpEvent += StartCast;
    }

    protected virtual void FixedUpdate()
    {
        if (isTouchingPlayer == true && isMeleeAttacking == false)
        {
            StartCoroutine(AttackMelee(Player.instance));
            PushPlayer();
            Player.instance.TakeDamage(1);
        }
    }

    public void StartCast(object sender, EventArgs e)
    {
        StartCoroutine(CastSpell());
    }

    protected override IEnumerator CastSpell()
    {
        while(isAlive)
        {
            List<Vector3> startPositions = SimpleFunctions.RandomCircle(transform.position, 0.5f, 4, 0);
            List<Vector3> endPositions = SimpleFunctions.RandomCircle(transform.position, 1.0f, 4, 0);
            for (int j = 0; j < startPositions.Count; j++)
            {
                BulletInfo bul = new BulletInfo(startPositions[j], endPositions[j], bulSpeed, 1, 1, 1);
                Bullet bullet = Instantiate(bulletObj, startPositions[j], Quaternion.identity).GetComponent<Bullet>();
                bullet.Setup(bul, 1, transform.rotation, false);
            }

            yield return new WaitForSeconds(timeBetweenAttack);
        }
    }
}
