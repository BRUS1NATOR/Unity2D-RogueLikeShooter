using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExplosionManager : ScriptableObject
{
    public enum ExplosionType { dynamite, bomb }
    static int layerMask;
    static int wallMask;

    public GameObject dynamiteExplosion;
    public GameObject bombExplosion;

    public static ExplosionManager instance;

    public static void Initialize()
    {
        instance = Resources.Load<ExplosionManager>("Explosion_Manager");
        layerMask = 1 << LayerMask.NameToLayer("HitBox") | 1 << LayerMask.NameToLayer("Particles");
        wallMask = 1 << LayerMask.NameToLayer("Walls");
    }

    public static void Explode(Vector2 position, float radius, int damage, float power, ExplosionType type)
    {
        switch (type)
        {
            case ExplosionType.bomb:
                GameManager.instance.audioManager.PlayExplosion();
                Instantiate(instance.bombExplosion, position, Quaternion.identity);
                break;
            case ExplosionType.dynamite:
                GameManager.instance.audioManager.PlayExplosion();
                Instantiate(instance.dynamiteExplosion, position, Quaternion.identity);
                break;
        }

        Collider2D[] hitColliders=Physics2D.OverlapCircleAll(position, radius,layerMask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            RaycastHit2D hit;
            hit = Physics2D.Linecast(position, hitColliders[i].gameObject.transform.position, wallMask);

            if (hit.collider == null)
            {
                if (hitColliders[i].CompareTag("HitBox") || hitColliders[i].CompareTag("PlayerHitBox") || hitColliders[i].CompareTag("Destructible"))
                {
                    IHealth obj = hitColliders[i].GetComponent<IHealth>();
                    if (obj != null)
                    {
                        Vector2 temp = hitColliders[i].gameObject.transform.position;
                        Vector2 normalized = (temp - position).normalized;
                        DoDamage(obj, damage, power, normalized);
                    }
                }
                else
                {
                    Rigidbody2D rb = hitColliders[i].GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 temp = rb.transform.position;
                        Vector2 normalized = (temp - position) * power * 0.4f;
                        rb.velocity = normalized;
                    }
                }
            }
            i++;
        }
    }

    private static void DoDamage(IHealth objectWithHealth, int damage, float power, Vector2 normalized)
    {
        if (power > 0)
        {
            if (objectWithHealth.ignoreBulletImpulse == false)
            {
                objectWithHealth.PushBack(normalized * 0.4f, power);
            }

        }
        objectWithHealth.TakeDamage(damage);
    }
}
