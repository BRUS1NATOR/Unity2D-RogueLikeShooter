using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour {

    public Stats_UI statsUI;
    public Hearts_UI healthUI;

    public int weaponNow;
    public int itemNow;

    public Image weaponSlot;
    public Image itemSlot;

    public Text itemText;
    public Text weaponText;

    public Slider itemSlider;
    public Text ammoText;

    public Text infoText;
    private Coroutine displayCoroutine;

    public Animator itemAnimator;

    private KeyCode action1, action2, action3,action4, action5, action6;

    // Use this for initialization
    void Start () {
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
        action4 = KeyCode.Alpha4;
        action5 = KeyCode.Alpha5;
        action6 = KeyCode.Alpha6;
    }
    private void Update()
    {
        if (Player.instance.isAlive)
        {
            if (Input.GetKeyDown(action1))
            {
                ChooseWeapon(0);
            }
            if (Input.GetKeyDown(action2))
            {
                ChooseWeapon(1);
            }
            if (Input.GetKeyDown(action3))
            {
                ChooseWeapon(2);
            }
            if (Input.GetKeyDown(action4))
            {
                ChooseWeapon(3);
            }
            if (Input.GetKeyDown(action5))
            {
                ChooseWeapon(4);
            }
            if (Input.GetKeyDown(action6))
            {
                ChooseWeapon(5);
            }
        }
    }
    
    public void GetItemUpdates(UseableItem item)
    {
        item.updateItem += RefreshItemSlider;
        item.unUseItem += UnUseItem;
        itemAnimator.runtimeAnimatorController = item.uiAnimator;
    }

    public void ChooseWeapon(int index)
    {
        weaponNow = index;

        Player.instance.ChangeWeapon(index);

        RefreshWeapon();
    }

    public void RefteshItem()
    {
        UseableItem item = Player.instance.GetItem();
        if (item != null)
        {
            itemText.text = item.itemName;
            itemSlot.sprite = null;

            if (item.isActive == true)
            {
                itemAnimator.runtimeAnimatorController = item.uiAnimator;
            }
            else
            {
                itemAnimator.runtimeAnimatorController = null;
            }

            if (item.icon!=null)
            { 
                itemSlot.sprite = item.icon;
            }
            SetImage(itemSlot);
        }
        else
        {
            itemAnimator.runtimeAnimatorController = null;
            itemSlot.sprite = null;
            itemText.text = "";
            SetImage(itemSlot);
        }

        RefreshItemSlider(null,null);
    }

    public void UnUseItem(object sender, EventArgs e)
    {
        UseableItem item = (UseableItem)sender;
        if (item != null)
        {
            RefteshItem();
            item.unUseItem -= UnUseItem;
            item.updateItem -= RefreshItemSlider;
        }
        else
        {
            itemAnimator.runtimeAnimatorController = null;
            itemSlot.sprite = null;

            SetImage(itemSlot);
        }
    }

    public void RefreshItemSlider(object sender, EventArgs e)
    {
        UseableItem item = Player.instance.GetItem();

        if (item != null)
        {
            itemSlider.maxValue = item.chargeMax;
            itemSlider.value = item.charge;
        }
        else
        {
            itemSlider.maxValue = 1;
            itemSlider.value = 0;
        }
    }

    private void SetImage(Image img)
    {
        if (img.sprite == null)
        {
            img.color = new Color(0, 0, 0, 0);
        }
        else
        {
            img.color = new Color(255, 255, 255, 1);
        }
    }

    public void RefreshWeapon()
    {
        Weapon weapon = Player.instance.GetWeapon();

        if (weapon == null)
        {
            weaponSlot.sprite = null;
            SetImage(weaponSlot);
            weaponText.text = "";
            ammoText.text = "";
        }

        else
        {
            weaponText.text = Player.instance.GetWeapon().GetComponentInParent<WeaponItem>().itemName;
            if (weapon.infiniteAmmo == true)
            {
                ammoText.text = weapon.magazine + "/" + "INF";
            }
            else
            {
                ammoText.text = weapon.magazine + "/" + weapon.ammo;
            }

            Sprite s = weapon.GetComponentInParent<Item>().icon;

            //if (s !=null)
            //{
            //    s = weapon.GetComponentInParent<WeaponItem>().item_sprite.sprite;
            //}

            weaponSlot.sprite = s;
            SetImage(weaponSlot);
        }
    }

    public void RefreshStats()
    {
        statsUI.Refresh();
        healthUI.Refresh();
    }

    public void DisplayInfoText(string s)
    {
        if(displayCoroutine!= null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayInfo(s));
    }

    private IEnumerator DisplayInfo(string s)
    {
        infoText.text = s;
        yield return new WaitForSeconds(4f);
        infoText.text = "";
        yield return 0;
    }
}
