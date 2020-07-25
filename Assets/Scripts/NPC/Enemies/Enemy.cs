using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Враг
/// </summary>
public class Enemy : NPC
{
    /// <summary>
    /// Номер врага в комнате
    /// </summary>
    public int index;

    /// <summary>
    ///  Преследует ли игрока
    /// </summary>
    private bool chasePlayer = false;
    /// <summary>
    /// Атакует ли в ближнем бою
    /// </summary>
    public bool isMeleeAttacking = false;
    /// <summary>
    /// Касается ли игрока
    /// </summary>
    public bool isTouchingPlayer = false;

    /// <summary>
    /// Перезаряжается ли
    /// </summary>
    protected bool isReloading=false;
    /// <summary>
    /// Время между атаками
    /// </summary>
    public float timeBetweenAttack;
    /// <summary>
    /// Определяет сколько времени атаковать
    /// </summary>
    public float timeToShoot;
    /// <summary>
    /// Стреляет ли враг
    /// </summary>
    protected bool isShooting;

    /// <summary>
    /// Стандартный материал
    /// </summary>
    private Material defaultMaterial;
    /// <summary>
    /// Корутин "вспышка"
    /// </summary>
    private Coroutine flashCor;

    protected virtual void Awake()
    {
        npcType = NPCType.Enemy;

        WakeUpEvent += PlayerEntersRoom;

        SleepEvent += PlayerLeavesRoom;

        stats.speed = movement.navAgent.speed;
        if (timeBetweenAttack <= 0)
        {
            timeBetweenAttack = 1f;
        }
        defaultMaterial = bodySprite.material;
    }

    protected virtual void PlayerEntersRoom(System.Object sender, EventArgs e)
    {

    }

    protected virtual void PlayerLeavesRoom(System.Object sender, EventArgs e)
    {

    }

    public override void WakeUp()
    {
        if (chasePlayer == false)
        {
            chasePlayer = true;
            base.WakeUp();
        }
    }
    public override void Sleep()
    {
        chasePlayer = false;
        base.Sleep();
    }

    protected void StartToShoot()
    {
        if (weapon != null)
        {
            StartCoroutine(StartShooting());
        }
    }

    protected IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, 2f));
        while (chasePlayer)
        {
            isShooting = true;
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(timeToShoot);
            isShooting = false;
            yield return new WaitForSeconds(timeBetweenAttack);
        }
    }

    protected IEnumerator Shoot()
    {
        while (isShooting)
        {
            if (isAlive && !isReloading)
            {
                if (weapon.coolDown == false)
                {
                    weapon.Attack(bulletModificators);

                    if (weapon.magazine == 0)
                    {
                        StartCoroutine(Reload(weapon));
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return 0;
    }

    protected void StopToShoot()
    {
        isShooting = false;
    }

    public IEnumerator AttackMelee(NPC npc)
    {
        while (isTouchingPlayer && isAlive && npc.isAlive)
        {
            isMeleeAttacking = true;

            npc.TakeDamage(10);
            animator.SetBool("Melee Attack", true);
            yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speed);

            animator.SetBool("Melee Attack", false);
            isMeleeAttacking = false;
            yield return new WaitForSeconds(0.5f);
        }
        yield return 0;
    }

    protected virtual IEnumerator CastSpell()
    {
        yield return 0;
    }

    public void PushPlayer()
    {
       Vector2 normalized = -(transform.position - Player.instance.transform.position).normalized;

        Player.instance.movement.rb.velocity = normalized * 25;
    }

    public override void TakeDamage(int damage)
    {
        if(flashCor != null)
        {
            StopCoroutine(flashCor);
        }
        flashCor = StartCoroutine(Flash());

        base.TakeDamage(damage);
    }

    public IEnumerator Reload(Weapon weapon)
    {
        if (weapon.magazine != weapon.magazineMax)
        {
            isReloading = true;
            yield return StartCoroutine(weapon.Reload());
            isReloading = false;
        }
        yield return 0;
    }

    public override IEnumerator Die(bool destroy)
    {
        isAlive = false;
        room.RemoveEnemy(this);

        Sleep();
        hitBox.enabled = false;

        yield return StartCoroutine(PlayDeathAnimation());
        DisableNPC();

        DropLoot();

        if (this.npcType != NPCType.Player)
        {
            Player.instance.gameStatistic.EnemiesDefeatedCount += 1;
        }

        if (destroy)
        {
            Destroy();
        }

        yield return 0;
    }

    public IEnumerator Flash()
    {
        Material material = new Material(Shader.Find("Custom/Flash"));
        material.color = Color.white;

        bodySprite.material = material;
        yield return new WaitForSeconds(0.2f);
        bodySprite.material = defaultMaterial;
    }

    protected void Flip(bool right)
    {
        if (FacingRight != right)
        {
            FacingRight = right;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
