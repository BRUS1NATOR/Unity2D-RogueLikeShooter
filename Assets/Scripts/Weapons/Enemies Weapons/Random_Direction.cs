using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Direction : FireArmWeapon
{
    public float coolDownTime;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInParent<Enemy>().animator;      //Это временно мб, лучше передавать аниматор через функцию
    }

    public override void Attack(params BulletMods[] mods)
    {
        if (coolDown == false)
        {
            coolDown = true;
            foreach (SeveralPoints points in startAndEndPoints)
            {
                BulletInfo bul = new BulletInfo(points.startPos.transform.position, points.endPos.transform.position, bullet_speed, 1, 1, 1);
                bullet = Instantiate(bulletObj, points.startPos.transform.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.Setup(bul, 1, transform.rotation, belongsToPlayer);
            }

            StartCoroutine(WaitForCoolDown());
        }
    }

    private IEnumerator WaitForCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        coolDown = false;
    }

    //private void PlayEnemyAttackAnim()
    //{
    //    coolDown = true;
    //    animator.SetFloat("Attack_Speed", attack_speed);
    //    animator.SetBool("Spell", true);
    //}

    //private void StopEnemyAttackAnim()
    //{
    //    coolDown = false;
    //    animator.SetBool("Spell", false);
    //}
}
