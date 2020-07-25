using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Body : MonoBehaviour
{
    protected NPC parent;

    public bool animPlaying;
    public bool isRolling;
    public Hands hands;

    public float moveHorizontal;
    public float moveVertical;

    public int looksUp;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        hands.defaultHandsPos = hands.transform.localPosition;
        looksUp = 0;
        parent = GetComponentInParent<NPC>();
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if (!animPlaying)
        {
            if (parent.lookAt.x < 0 && parent.FacingRight == true) //Тело
            {
                Flip(false);
            }
            else if (parent.lookAt.x > 0 && parent.FacingRight == false)
            {
                Flip(true);
            }

            GameObject toRotate = hands.frontHand;

            bool oneHanded = true;

            Weapon parentWeap = parent.GetWeapon();

            hands.transform.right = new Vector3(0, 0, 0);

            if (parentWeap != null)
            {
                if (parentWeap.oneHanded == false)//Руки
                {
                    oneHanded = false;
                    toRotate = hands.gameObject;     //Если 2 руки
                }
            }

            if (parent.FacingRight == true)
            {
                toRotate.transform.right = parent.lookAt;

                if (Mathf.Abs(toRotate.transform.rotation.z) <= 0.2f) 
                {
                    Look(Direction4D.Right, oneHanded, parentWeap);
                }
                else if (toRotate.transform.rotation.z > -0.2f)
                {
                    Look(Direction4D.Up, oneHanded, parentWeap);
                }
                else
                {
                    Look(Direction4D.Down, oneHanded, parentWeap);
                }
            }
            else
            {
                toRotate.transform.right = -parent.lookAt;

                if (Mathf.Abs(toRotate.transform.rotation.z) <= 0.2f)
                {
                    Look(Direction4D.Right, oneHanded, parentWeap); //В сторону
                }
                else if (toRotate.transform.rotation.z < -0.2f)
                {
                    Look(Direction4D.Up, oneHanded, parentWeap); //Вверх
                }
                else
                {
                    Look(Direction4D.Down, oneHanded, parentWeap);    //Вниз
                }
            }
        }
    }

    void Flip(bool right)
    {
        parent.FacingRight = right;

        // Multiply the parent's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Look(Direction4D dir, bool oneHanded, Weapon weapon) //1 вверх 0 вправо -1 вниз
    {
        if (dir == Direction4D.Up) //Если смотрит вверх
        {
            parent.animator.SetFloat("LooksUp", 1f);
            hands.SetLocalPosition(Direction4D.Up, -2);
        }
        else if (dir == Direction4D.Right) //Если смотрит вправо
        {
            parent.animator.SetFloat("LooksUp", 0f);
            hands.SetLocalPosition(Direction4D.Right, 2);
        }
        else if (dir == Direction4D.Down) //Если смотрит вниз
        {
            parent.animator.SetFloat("LooksUp", -1f);

            if (oneHanded == false)
            {
                hands.SetLocalPosition(Direction4D.Down, 2);
            }
            else
            {
                hands.SetLocalPosition(Direction4D.Right, 2);
            }
        }
    }

    public void StopRoll()
    {
        parent.animator.SetBool("Roll", false);
        parent.hitBox.enabled = true;
        animPlaying = false;
        isRolling = false;

        Weapon weap = parent.GetWeapon();
        weap.Show();
        weap.coolDown = false;

        foreach (WeaponItem wep in parent.weapons)
        {
            wep.weapon.coolDown = false;
        }
        StartCoroutine(RollCoolDown());
    }

    private IEnumerator RollCoolDown()
    {
        yield return new WaitForSeconds(0.1f);
        parent.rollCoolDown = false;
    }

    public void Roll()
    {
        if (parent.rollCoolDown == false)
        {
            Vector2 rollTo = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (rollTo.x != 0 || rollTo.y != 0)
            {
                if(Mathf.Abs(rollTo.x) > 0 && Mathf.Abs(rollTo.y) > 0)
                {
                    if (rollTo.x > 0)
                    {
                        rollTo.x = 0.7f;
                    }
                    else
                    {
                        rollTo.x = -0.7f;
                    }

                    if (rollTo.y > 0)
                    {
                        rollTo.y = 0.7f;
                    }
                    else
                    {
                        rollTo.y = -0.7f;
                    }
                }
                else if (Mathf.Abs(rollTo.x) > 0)
                {
                    if (rollTo.x > 0)
                    {
                        rollTo.x = 1f;
                    }
                    else
                    {
                        rollTo.x = -1f;
                    }
                }
                else
                {
                    if (rollTo.y > 0)
                    {
                        rollTo.y = 1f;
                    }
                    else
                    {
                        rollTo.y = -1f;
                    }
                }

                parent.rollCoolDown = true;
                parent.hitBox.enabled = false;
                isRolling = true;

                Weapon w = parent.GetWeapon();
                w.Hide();
                w.coolDown = true;

                animPlaying = true;
                parent.animator.SetBool("Roll", true);

                StartCoroutine(moveRoll(rollTo));
            }
        }
    }

    private IEnumerator moveRoll(Vector2 moveTo)
    {
        while (animPlaying)
        {
            parent.movement.rb.AddForce(moveTo * 18 * parent.stats.speed * Time.timeScale);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
