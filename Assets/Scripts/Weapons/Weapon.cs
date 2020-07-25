using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType { fireArm, laser, melee}

public class Weapon : Weareble {

    public AudioClip shoot;
    public AudioClip reload;

    public int damage;
    public int power; 

    public WeaponItem weaponItem;

    public bool oneHanded;
    public SeveralPoints[] startAndEndPoints;

    public Animator animator;
    [HideInInspector]
    protected AudioSource audioSource;

    public Coroutine reloadCoroutine;

    public WeaponType type;

    public int ammo
    {
        get
        {
            return Ammo;
        }
        set
        {
            Ammo = value;
            if (Ammo > ammoMax)
            {
                Ammo = ammoMax;
            }
            if (Player.instance != null)
            {
                Player.instance.RefreshWeapon();
            }
        }
    }

    private int Ammo;

    public int ammoMax;

    public int magazine;
    public int magazineMax;

    public float attack_speed;
    public float reload_speed;

    public bool belongsToPlayer;

    public bool infiniteAmmo;
   
    public bool coolDown;

    public bool reloadCoolDown;


    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ammo = ammoMax;
        weaponItem = GetComponentInParent<WeaponItem>();
        animator = GetComponent<Animator>();
        facingRight = true;
        if (animator != null)
        {
            animator.SetBool("Attack", false);
        }
        coolDown = false;
    }

    public virtual void Attack(params BulletMods[] mods)
    {

    }

    public virtual IEnumerator Reload()
    {
        if (reloadCoolDown == false)
        {
            reloadCoolDown = true;
            if (ammo >= 0)
            {
                if (magazine != magazineMax)
                {
                    if (belongsToPlayer == true)
                    {
                        Player.instance.reloadText.text = "RELOADING";
                    }
                    yield return new WaitForSeconds(reload_speed);

                    audioSource.PlayOneShot(reload);

                    int difference = magazineMax - magazine;

                    if (infiniteAmmo == false)
                    {
                        ammo -= difference;

                        if (ammo < 0)
                        {
                            magazine += difference + ammo;
                            ammo = 0;
                        }
                        else
                        {
                            magazine = magazineMax;
                        }
                    }

                    else
                    {
                        magazine = magazineMax;
                    }
                }
                if (belongsToPlayer == true)
                {
                    Player.instance.reloadText.text = "";
                    Player.instance.actionBar.RefreshWeapon();
                }

            }
            reloadCoolDown = false;
        }
    }

    public virtual void CancelReload()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }
        reloadCoolDown = false;
        if (belongsToPlayer == true)
        {
            Player.instance.reloadText.text = "";
        }
    }

    protected virtual void PlaySound()
    {
        audioSource.PlayOneShot(shoot, 0.5f);
        GameManager.instance.audioManager.AddLiveSource(audioSource);
    }

    protected virtual void StopSound()
    {
        GameManager.instance.audioManager.RemoveLiveSource(audioSource);
    }

    public void PlayShootAnim()
    {
        coolDown = true;
        animator.SetBool("Attack", true);
        animator.SetFloat("Attack_Speed", attack_speed);
    }

    public void StopShootAnim()
    {
        animator.SetBool("Attack", false);
        StartCoroutine(WaitForCoolDown());
    }

    private IEnumerator WaitForCoolDown()
    {
        yield return new WaitForSeconds(0.05f);
        coolDown = false;
    }
}
