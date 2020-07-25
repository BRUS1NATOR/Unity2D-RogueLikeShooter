using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Обьект отвечающий за нахождение пути
/// </summary>
public class Movement_Agent : MonoBehaviour
{
    /// <summary>
    /// NPC агента
    /// </summary>
    public NPC child;
    /// <summary>
    /// Тело NPC
    /// </summary>
    public Body body;

    public int stopAt;

    public int rayDistance;

    public NavMeshAgent navAgent;
    public Rigidbody2D rb;

    public bool playerInTheRoom = false;

    private bool _dontMove=false;
    public bool dontMove
    {
        get
        {
            return _dontMove;
        }
        set
        {
            _dontMove = value;
            if (value == true)
            {
                if (navAgent != null)
                {
                    navAgent.isStopped = true;
                }
            }
            else
            {
                if (navAgent != null)
                {
                    navAgent.isStopped = false;
                }
            }
        }
    }

    public Coroutine pushBackCoroutine;

    int mask;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        navAgent = GetComponent<NavMeshAgent>();

        if (child == null)
        {
            child = GetComponentInChildren<NPC>();
        }
        if (child is Enemy)
        {
            child.GetComponent<Enemy>().WakeUpEvent += StartChasing;
            child.GetComponent<Enemy>().SleepEvent += StopChasing;
        }
        mask = 1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("Player");
    }

    private void FixedUpdate()
    {
        if (child.isAlive)
        {
            if (navAgent != null)
            {
                child.animator.SetFloat("RunVertical", Mathf.Abs(navAgent.velocity.normalized.y));
                child.animator.SetFloat("RunHorizontal", Mathf.Abs(navAgent.velocity.normalized.x));
            }
            else
            {
                child.animator.SetFloat("RunVertical", Mathf.Abs(rb.velocity.normalized.y));
                child.animator.SetFloat("RunHorizontal", Mathf.Abs(rb.velocity.normalized.x));
            }
        }
    }

    private IEnumerator RecreatePath()
    {
        if (navAgent != null)
        {
            while (playerInTheRoom)
            {
                if (rayDistance != 0)
                {
                    CastRay();
                }
                if (dontMove == false && child.isAlive)
                {
                    navAgent.SetDestination(new Vector2(Player.instance.transform.position.x, Player.instance.transform.position.y));
                }
                yield return new WaitForSeconds(0.75f);
            }
        }
    }


    public void StopChasing(object sender, EventArgs e)
    {
        playerInTheRoom = false;
        if (navAgent != null)
        {
            navAgent.isStopped = true;
        }
    }
    public void StartChasing(object sender, EventArgs e)
    {
        playerInTheRoom = true;
        if (navAgent != null)
        {
            navAgent.isStopped = false;
            StartCoroutine(RecreatePath());
        }
    }

    public void PushBack(Vector2 push, float seconds)
    {
        if (child.isAlive)
        {
            if (pushBackCoroutine != null)
            {
                StopCoroutine(pushBackCoroutine);
            }
            pushBackCoroutine = StartCoroutine(Push(push, seconds));
        }
    }

    private IEnumerator Push(Vector2 normalized, float seconds)
    {
        child.stats.pushBack = true;
        dontMove = true;

        rb.velocity  = normalized * child.stats.speed * Time.timeScale * 0.5f;
        yield return new WaitForSeconds(seconds);
        rb.velocity = Vector2.zero;

        child.stats.pushBack = false;
        if (child.isAlive)
        {
            dontMove = false;
        }
    }

    protected int Distance(Vector2 destination)
    {
        int distance = (int)(Math.Abs(destination.x - child.transform.position.x) + Math.Abs(destination.y - child.transform.position.y));
        return distance;
    }

    public void CastRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, child.lookAt, rayDistance, mask);
        Debug.DrawRay(transform.position, child.lookAt, Color.green, 1);

        bool canShoot = false;
        if (hit.collider != null)
        {
            if (hit.collider.name == "player")
            {
                canShoot = true;
                if (hit.distance <= stopAt)
                {
                    dontMove = true;
                }
                else
                {
                    dontMove = false;
                }
            }
            else
            {
                dontMove = false;
            }
        }
        else
        {
            dontMove = false;
        }
        child.GetComponent<Ranger_Enemy>().canShoot = canShoot;
    }
}
