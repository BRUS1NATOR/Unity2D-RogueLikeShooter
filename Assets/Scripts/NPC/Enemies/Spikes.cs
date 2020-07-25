using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    private Player player_stats;

    float timeBetweenDamage = 0.5f;
    float timer;
    bool allowDamage = true;
    public byte damage;

    // Use this for initialization
    void Start () {
        player_stats = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHitBox"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            timer += Time.deltaTime;

            if (timer <= timeBetweenDamage)
            {
                allowDamage = false;
            }
            else
            {
                allowDamage = true;
            }

            if (allowDamage == true)
            {
                enemy.TakeDamage(damage);
                timer = 0;
            }
        }
        if (collision.CompareTag("Player"))
        {
            player_stats.TakeDamage(damage);
        }
    }
}
