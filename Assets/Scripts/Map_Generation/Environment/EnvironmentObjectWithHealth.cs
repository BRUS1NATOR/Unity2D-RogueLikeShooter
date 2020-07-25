using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectWithHealth : MonoBehaviour, IHealth
{
    public Animator animator;
    public Placement place;

    [HideInInspector]
    [SerializeField]
    private int _maxHealth;
    [HideInInspector]
    [SerializeField]
    private int _curHealth;
    [HideInInspector]
    private bool _ignoreImpulse;
    [HideInInspector]
    private bool _pushBack;

    public int maxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int curHealth { get { return _curHealth; } set { _curHealth = value; } }

    public bool ignoreBulletImpulse { get { return _ignoreImpulse; } set { _ignoreImpulse = value; } }
    public bool pushBack { get { return _pushBack; } set { _pushBack = value; } }

    public void EffectDamage(float flame, EffectType effect)
    {

    }

    private void OnBecameInvisible()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    private void OnBecameVisible()
    {
        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    protected virtual void Awake()
    {
        curHealth = maxHealth;
        if (animator != null)
        {
            animator.enabled = false;
        }
    }


    public bool Heal(int hp)
    {
        if (curHealth != maxHealth)
        {
            curHealth += hp;
            if (curHealth > maxHealth)
            {
                curHealth = maxHealth;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void IncreaseMaxHealth(int hp)
    {
        maxHealth += hp;
        curHealth += hp;
    }

    public virtual void TakeDamage(int hp)
    {

    }
    public virtual void PushBack(Vector2 normalized, float power)
    {

    }
}
