using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Range : MonoBehaviour
{
    public Enemy parent;
    public bool closeDamage;

    private void Awake()
    {
        parent = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerHitBox")
        {
            parent.isTouchingPlayer = true;
            if (closeDamage)
            {
                if (parent.stats.pushBack == false && parent.isMeleeAttacking == false)
                {
                    StartCoroutine(parent.AttackMelee(Player.instance));
                    parent.PushPlayer();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerHitBox")
        {
            parent.isTouchingPlayer = false;
        }
    }
}
