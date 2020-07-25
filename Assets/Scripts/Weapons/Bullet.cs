using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletInfo
{
    public Vector2 start;
    public Vector2 end;
    public int speed;
    public int damage;
    public int power;
    public float accuracy;
    public BulletInfo(Vector2 Start, Vector2 End, int Speed, int Damage, int Power, float Accuracy)
    {
        start = Start;
        end = End;
        speed = Speed;
        damage = Damage;
        power = Power;
        accuracy = Accuracy;
    }
}

public class Bullet : MonoBehaviour {

    public Animator anim;
    Vector3 movementVector;

    BulletInfo info;

    protected int damage;
    protected int power;

    protected float flameDamage;
    protected bool isCircle;

    public bool belongsToPlayer;
    public bool ready = false;

    public GameObject rotationOffset;

    public void Setup(BulletInfo info, float xScale, Quaternion rotation, bool belongstoPlayer, params BulletMods[] mods)
    {
        this.info = info;
        rotationOffset = new GameObject();

        transform.localScale = new Vector2(transform.localScale.x * xScale, transform.localScale.y);
        rotationOffset.transform.parent = this.transform;
        rotationOffset.transform.localPosition = new Vector2(0, 0.25f);

        transform.position = info.start;
        transform.rotation = rotation;

        belongsToPlayer = belongstoPlayer;
        damage = info.damage;
        power = info.power;

        int r = Random.Range(0, 2);
     //   float acc = (1 - accuracy) / 4; //разброс от 0 до 0.25
        if (r == 0)
        {
            movementVector = (info.end - info.start).normalized;
           // movementVector = new Vector2(movementVector.x - acc, movementVector.y - info.acc);
        }
        else
        {
            movementVector = (info.end - info.start).normalized;
            // movementVector = new Vector2(movementVector.x + acc, movementVector.y + info.acc);
        }

        for (int i = 0; i < mods.Length; i++)
        {
            if (mods[i].mod == WeaponModification.circle)
            {
                isCircle = true;
            }
            if (mods[i].mod == WeaponModification.fire)
            {
                flameDamage = mods[i].power;
            }
        }

        movementVector *= info.speed;
        ready = true;
        if (belongstoPlayer == true)
        {
            Player.instance.gameStatistic.ShotsBeenFired += 1;
        }
    }

    virtual protected void FixedUpdate()
    {
        if (ready)
        {
            transform.position += movementVector * Time.deltaTime;
            if (isCircle)
            {
                transform.RotateAround(rotationOffset.transform.position, Vector3.forward, 14.8f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Destructible"))
        {
            DoDamage(collision.collider);
        }
        else
        {
            WallCollision();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("HitBox"))
        {
            if (belongsToPlayer)
            {
                if (ready)
                {
                    DoDamage(collider);
                }
            }
        }
        else if (collider.gameObject.CompareTag("PlayerHitBox"))
        {
            if (!belongsToPlayer)
            {
                if (ready)
                {
                    DoDamage(collider);
                }
            }
        }
        else if (collider.gameObject.CompareTag("Destructible"))
        {
            if (ready)
            {
                DoDamage(collider);
            }
        }
    }

    protected virtual void DoDamage(Collider2D collider)
    {
        IHealth objectWithHealth = collider.gameObject.GetComponent<IHealth>();
        if (objectWithHealth != null)
        {
            ready = false;
            if (power > 0)
            {
                StartCoroutine(PlayAnimation());
                Vector2 normalized = -(transform.position - collider.gameObject.transform.position).normalized;
                if (objectWithHealth.ignoreBulletImpulse == false)
                {
                    objectWithHealth.PushBack(normalized, (float)power);
                }
                if (flameDamage > 0)
                {
                    objectWithHealth.EffectDamage(flameDamage, EffectType.Flame);
                }
                objectWithHealth.TakeDamage(damage);
                DestroyBullet();
            }
            else
            {
                if (flameDamage > 0)
                {
                    objectWithHealth.EffectDamage(flameDamage, EffectType.Flame);
                }
                objectWithHealth.TakeDamage(damage);
                StartCoroutine(PlayAnimationAndDestroy());
            }

        }
        else
        {
            ready = true;
        }
    }

    protected virtual void WallCollision()
    {
        ready = false;
        StartCoroutine(PlayAnimationAndDestroy());
    }

    public void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator PlayAnimation()
    {
        anim.SetBool("Destroy", true);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / anim.GetFloat("Destroy Speed"));
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    public virtual IEnumerator PlayAnimationAndDestroy()
    {
        anim.SetBool("Destroy", true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / anim.GetFloat("Destroy Speed"));
        DestroyBullet();
    }
}
