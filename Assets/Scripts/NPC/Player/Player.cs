using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Статистика игры
/// </summary>
public struct GameStatistic
{
    public int EnemiesDefeatedCount;
    public int RoomsCount;
    public int ShotsBeenFired;
    public float timePlayed;
}

/// <summary>
/// Игрок
/// </summary>
public class Player : NPC {

    public static Player instance = null;

    public ActionBar actionBar;
    public Camera sightCamera;
    public Map_Camera mapCamera;

    public GameStatistic gameStatistic;

    public Corridor corridor;

    SpriteRenderer[] spriteRenderers;

    public bool allowDamage = true;
    public bool invicible = false;

    public Text reloadText;
    public int weaponIndex;

    public int maxItems;
    public List<GameObject> items;
    public int itemIndex;
    public int keys;

    public int coins;
    // Use this for initialization
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        items = new List<GameObject>(new GameObject[maxItems]);

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        gameStatistic.EnemiesDefeatedCount = 0;
        gameStatistic.RoomsCount = 0;
        gameStatistic.ShotsBeenFired = 0;
        gameStatistic.timePlayed = 0;
        canShoot = true;
    }

    private void FixedUpdate()
    {
        lookAt = Camera_Bounds.instance.mousePos - transform.position;
    }

    protected override void Start()
    {
        base.Start();
        WakeUp();
        RefreshUI();
        WakeUp();
    }

    /// <summary>
    /// Обновление интерфейса
    /// </summary>
    public void RefreshUI()
    {
        actionBar.RefreshStats();
        actionBar.RefteshItem();
    }

    /// <summary>
    /// Обновление предмета в интерфейсе
    /// </summary>
    public void RefreshItem()
    {
        actionBar.RefteshItem();
    }

    /// <summary>
    /// Обновление здоровья и монет в интерфейсы
    /// </summary>
    public void RefreshStats()
    {
        actionBar.RefreshStats();
    }

    /// <summary>
    /// Обновление интерфейса оружия
    /// </summary>
    public void RefreshWeapon()
    {
        actionBar.RefreshWeapon();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            gameStatistic.timePlayed += Time.deltaTime;
            if (weapon != null)
            {
                if (canShoot)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        Attack(weapon);
                    }
                    if (Input.GetButtonDown("Reload"))
                    {
                        Reload();
                    }
                }
                if (!movement.dontMove)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        movement.body.Roll();
                    }
                    if (Input.GetKey(KeyCode.G))
                    {
                        weapons[weaponIndex].GetComponent<WeaponItem>().Drop();
                        weapon = null;
                        weapons[weaponIndex] = null;
                        actionBar.RefreshWeapon();
                    }
                }
            }
            if (!movement.dontMove)
            {
                if (Input.GetButtonDown("DropItem"))
                {
                    DropItem(itemIndex);
                }
                if (Input.GetButtonDown("ChangeItem"))
                {
                    ChangeItem();
                }
                if (Input.GetButtonDown("UseItem"))
                {
                    UseItem();
                }
            }
        }
    }

    /// <summary>
    /// "Заморозить" игрока
    /// </summary>
    public void PausePlayer()
    {
        canShoot = false;
        movement.dontMove = true;
    }

    public void UnPausePlayer()
    {
        canShoot = true;
        movement.dontMove = false;
    }

    public void DisplayItemDescription(Item item)
    {
        actionBar.DisplayInfoText(item.description);
    }

    public void UseItem()
    {
        if (items[itemIndex] != null)
        {
            items[itemIndex].GetComponent<UseableItem>().Use();

            UseableItem item = GetItem();
            if (item != null)
            {
                if (item.isActive)
                {
                    actionBar.GetItemUpdates(item);
                }
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (allowDamage == true)
        {
            if (invicible == false)
            {
                base.TakeDamage(1);
                allowDamage = false;

                if (stats.curHealth > 0)
                {
                    StartCoroutine(Flash());
                }
            }

            RefreshStats();
        }
    }

    public IEnumerator Flash()
    {
        int flashes = 0;
        while (flashes < 4)     // .25 * 2 * 3 == 1,5 секунды      /*(3<4)*/
        {
            Visible(false);
            yield return new WaitForSeconds(0.25f);
            Visible(true);
            yield return new WaitForSeconds(0.25f);
            flashes++;
        }
        allowDamage = true;
    }

    public override bool Heal(int hp)
    {
        bool b = base.Heal(hp);
        actionBar.RefreshStats();
        return b;
    }

    override protected bool TakeWeapon(WeaponItem item)
    {
        if (coins >= item.price)
        {
            int i = 0;
            int emptySlot = weapons.Count;

            foreach (WeaponItem player_weapon in weapons)
            {
                if (player_weapon != null)
                {
                    if (player_weapon.itemName == item.itemName)   //Если такое оружие уже есть у игрока
                    {
                        player_weapon.GetComponentInChildren<Weapon>().ammo += item.GetComponentInChildren<Weapon>().ammo;
                        Destroy(item.gameObject);
                        return false;
                    }
                }
                else
                {
                    emptySlot = i;
                    break;
                }
                i++;
            }

            if(emptySlot == weapons.Count)
            {
                weapons.Add(item);
            }
            else
            {
                weapons[emptySlot] = item;
            }

            if (emptySlot == weaponIndex)
            {
                item.GetComponentInChildren<Weapon>().Show();
                weapon = weapons[weaponIndex].GetComponent<WeaponItem>().weapon;
            }
            else
            {
                item.GetComponentInChildren<Weapon>().Hide();
            }

            coins -= item.price;

            if (FacingRight == false)
            {
                item.Flip();
            }
            actionBar.RefreshWeapon();
            actionBar.DisplayInfoText(item.description);
            return true;

        }
        actionBar.RefreshWeapon();
        return false;
    }

    public void Attack(Weapon weapon)
    {
        if (weapon.coolDown == false)
        {
            if (weapon.type == WeaponType.fireArm)
            {
                weapon.Attack(bulletModificators);

                actionBar.RefreshWeapon();
            }
            else if (weapon.type == WeaponType.laser)
            {
                weapon.Attack(bulletModificators);

                actionBar.RefreshWeapon();
            }
        }
    }

    public void Reload()
    {
        Weapon weap = GetWeapon();
        if (weap != null)
        {
            weap.reloadCoroutine = StartCoroutine(weap.Reload());
        }
    }


    public void ChangeWeapon(int index)
    {
        foreach (WeaponItem w in weapons)
        {
            if (w != null)
            {
                w.weapon.Hide();
                w.weapon.CancelReload();
                reloadText.text = "";
            }
        }
        weaponIndex = index;
        if (weaponIndex < weapons.Count)
        {
            if (weapons[weaponIndex] != null)
            {
                weapon = weapons[weaponIndex].GetComponent<WeaponItem>().weapon;

                if (movement.body.isRolling == false)
                {
                    weapon.Show();
                }
                else
                {
                    weapon.coolDown = true;
                }
            }
            else
            {
                weapon = null;
            }
        }
        else
        {
            weapon = null;
        }
    }


    public override bool TakeItem(Item item)
    {
        if (item.itemType == ItemTypes.UseableItem)
        {
            int i = 0;
            int emptySlot = -1;
            foreach (GameObject it in items)
            {
                if (it != null)
                {
                    Item player_item = it.GetComponent<Item>();
                    if (player_item != null)
                    {
                        if (player_item.itemName == item.GetComponent<Item>().itemName)   //Если такой предмет уже есть у игрока
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    emptySlot = i;
                }
                i++;
            }

            if (emptySlot != -1)
            {
                if (item.price > coins)
                {
                    Debug.Log("NOT ENOUGH MONEY");
                    return false;
                }
                else
                {
                    coins -= item.price;
                    items[emptySlot] = item.gameObject;
                    itemIndex = emptySlot;
                    return true;
                }
            }
            else
            {
                DropItem(itemIndex);
                coins -= item.price;
                items[itemIndex] = item.gameObject;
                return true;
            }
        }
        else if(item.itemType == ItemTypes.PassiveItem)
        {
            if (item.price > coins)
            {
                Debug.Log("NOT ENOUGH MONEY");
                return false;
            }
            else
            {
                coins -= item.price;
                return true;
            }
        }
        else if(item.itemType == ItemTypes.WeaponItem)
        {
            return TakeWeapon(item.GetComponent<WeaponItem>());
        }
        return false;
    }

    private void DropItem(int index)
    {
        if (items[itemIndex] != null)
        {
            items[itemIndex].GetComponent<UseableItem>().UnUse();

            items[index].GetComponent<Item>().Drop();
            if (FacingRight == false)
            {
                SimpleFunctions.Flip(items[index]);
            }
            items[index] = null;
        }
        actionBar.RefteshItem();
    }

    public UseableItem GetItem()
    {
        if (items[itemIndex] != null)
        {
            return items[itemIndex].GetComponent<UseableItem>();
        }
        else
        {
            return null;
        }
    }

    public void AddChargeToItem(int charge)
    {
        UseableItem item = GetItem();
        if (item != null)
        {
            item.charge += charge;
        }
        RefreshItem();
    }

    public UseableItem ChangeItem()
    {
        if (itemIndex + 1 == maxItems)
        {
            itemIndex = 0;
        }
        else
        {
            itemIndex++;
        }
        if (items[itemIndex] != null)
        {
            actionBar.RefteshItem();
            return items[itemIndex].GetComponent<UseableItem>();
        }
        else
        {
            actionBar.RefteshItem();
            return null;
        }

    }

    private void Visible(bool visible)
    {
        if (visible)
        {
            foreach (SpriteRenderer render in spriteRenderers)
            {
                render.enabled = true;
            }
            if (weapon != null)
            {
                weapon.Show();
            }
        }
        else
        {
            foreach (SpriteRenderer render in spriteRenderers)
            {
                render.enabled = false;
            }
            if (weapon != null)
            {
                weapon.Hide();
            }
        }
    }

    public override void IncreaseMaxHealth(int hp)
    {
        base.IncreaseMaxHealth(hp);
        RefreshStats();
    }

    public override IEnumerator Die(bool destroy)
    {
        weapon.Hide();
        weapon = null;
        weapons[weaponIndex] = null;
        room.EnemiesSleep();

        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        movement.rb.velocity = Vector2.zero;

        yield return PlayDeathAnimation();
        GameManager.instance.RaiseEndGame();

    }

}
