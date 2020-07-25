using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : EnvironmentObjectWithHealth
{
    private Collider2D coll;

    public bool destructible;

    [HideInInspector]
    public GameObject destroyOnBrake;
    [HideInInspector]
    public bool breakWithAnimation;

    private int hits;


    public List<Vector2> coordinates;

    protected Explodable expl;
    protected MyFragmentsAddon addon;

    protected override void Awake()
    {
        base.Awake();
        curHealth = maxHealth;
        coll = GetComponent<Collider2D>();
        expl = GetComponent<Explodable>();
        if (expl != null)
        {
            expl.deleteFragments();
            addon = GetComponent<MyFragmentsAddon>();
        }
    }

    protected virtual void Brake()
    {
        coll.enabled = false;
        Destroy(destroyOnBrake);

        if (breakWithAnimation == true)
        {
            BreakWithAnimation();
        }
        else
        {
            BreakWithFragments();
        }
    }

    private void BreakWithAnimation()
    {
        animator.SetFloat("Hits", 1);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = "Floor";
            renderer.sortingOrder = 0;
        }
    }

    private void BreakWithFragments()
    {
        expl.explode();
    }

    public override void TakeDamage(int dmg)
    {
        hits += dmg;
        curHealth -= dmg;
        if (curHealth > 0)
        {
            if (animator != null)
            {
                animator.SetFloat("Hits", (float)(hits / System.Convert.ToDecimal(maxHealth)));
            }
        }
        if (curHealth <= 0)
        {
            Brake();
        }
    }

    public override void PushBack(Vector2 normalized, float power)
    {
        if (addon != null)
        {
            addon.velocity = normalized * power;
        }
    }
}