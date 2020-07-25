using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Boss
{
    public override IEnumerator Die(bool destroy)
    {
        yield return StartCoroutine(base.Die(destroy));

        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        sp.color = new Color(255, 255, 255, 0.5f);
    }

    protected override IEnumerator CastSpells()
    {
        while (isAlive && cast)
        {
            Debug.Log("ISALIVE!");
            movement.dontMove = false;
            yield return new WaitForSeconds(timeBetweenCast);
            movement.dontMove = true;
            yield return StartCoroutine(CastSpell());
            animator.SetBool("Spell", false);
        }
    }

    protected override IEnumerator SpellZero()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        float wait = (animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speed);
        float offset = 0f;
        yield return new WaitForSeconds(wait);
        for (int i = 0; i < 10; i++)
        {
            List<Vector3> startPositions = SimpleFunctions.RandomCircle(bulStartPoint.position, 0.5f, 18,offset);
            List<Vector3> endPositions = SimpleFunctions.RandomCircle(bulStartPoint.position, 1.0f, 18,offset);
            for (int j =0;j<startPositions.Count;j++)
            {
                BulletInfo bul = new BulletInfo(startPositions[j], endPositions[j], bullet_speed, 1, 1, 1);
                Bullet bullet = Instantiate(bulletObj, startPositions[j], Quaternion.identity).GetComponent<Bullet>();
                bullet.Setup(bul, 1, transform.rotation, false);
            }

            yield return new WaitForSeconds(0.5f);
            offset += 5f;
        }
        yield return new WaitForEndOfFrame();
    }

    //protected override IEnumerator SpellOne()
    //{
    //    yield return new WaitForEndOfFrame();
    //    yield return new WaitForEndOfFrame();
    //    float wait = (animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speed);
    //    Debug.Log("WAIT: " + wait);
    //    yield return new WaitForSeconds(wait);
    //    for (int i = 0; i < 10; i++)
    //    {
    //        for (int j = 0; j < 20; j++)
    //        {
    //            int a = j * 18;
    //            Vector3 pos = SimpleFunctions.RandomCircle(bulStartPoint.position, 0.5f, a);
    //            Vector3 endPos = SimpleFunctions.RandomCircle(bulStartPoint.position, 1.0f, a);
    //            Bullet bullet = Instantiate(bulletObj, pos, Quaternion.identity).GetComponent<Bullet>();
    //            bullet.Setup(pos, endPos, bullet_speed, 1, transform.rotation, false, 1, 0);
    //        }
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //    yield return new WaitForEndOfFrame();
    //}
}
