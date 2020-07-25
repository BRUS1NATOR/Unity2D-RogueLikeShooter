using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { zero, one, two, three}

public class Boss : Enemy
{
    public Transform bulStartPoint;
    public GameObject bulletObj;
    public int bullet_speed;

    public bool cast = false;
    public float timeBetweenCast;

    public Stage stageNow = Stage.one;

    public int firstStageHealth;
    public int secondStageHealth;
    public int thirdStageHealth;

    protected override void PlayerEntersRoom(System.Object sender, EventArgs e)
    {
        cast = true;
        StartCoroutine(CastSpells());
    }

    protected override void PlayerLeavesRoom(System.Object sender, EventArgs e)
    {
        cast = false;
    }

    protected virtual IEnumerator CastSpells()
    {
        while (isAlive && cast)
        {
            yield return new WaitForSeconds(timeBetweenCast);
            yield return StartCoroutine(CastSpell());
        }
    }

    protected override IEnumerator CastSpell()
    {
        animator.SetInteger("SpellID", (int)stageNow);
        animator.SetBool("Spell", true);
        switch (stageNow)
        {
            case Stage.zero:
            {
                yield return StartCoroutine(SpellZero());
                break;
            }
            case Stage.one:
                {
                    yield return StartCoroutine(SpellOne());
                    break;
                }
            case Stage.two:
                {
                    yield return StartCoroutine(SpellTwo());
                    break;
                }
            case Stage.three:
                {
                    yield return StartCoroutine(SpellThree());
                    break;
                }
        }
        animator.SetBool("Spell", false);
    }

    protected virtual IEnumerator SpellZero()
    {
        yield return 0;
    }
    protected virtual IEnumerator SpellOne()
    {
        yield return 0;
    }
    protected virtual IEnumerator SpellTwo()
    {
        yield return 0;
    }
    protected virtual IEnumerator SpellThree()
    {
        yield return 0;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (stats.curHealth <= thirdStageHealth)
        {
            stageNow = Stage.three;
        }
        else if (stats.curHealth <= secondStageHealth)
        {
            stageNow = Stage.two;
        }
        else if (stats.curHealth <= firstStageHealth)
        {
            stageNow = Stage.one;
        }
    }
}
