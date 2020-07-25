using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes { PassiveItem, UseableItem, WeaponItem }

/// <summary>
/// Класс от которого наследуются предметы
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// NPC которому принадлежит предмет
    /// </summary>
    public NPC npc;

    /// <summary>
    /// Имя предмета
    /// </summary>
    public string itemName;
    /// <summary>
    /// Описание предмета
    /// </summary>
    public string description;

    /// <summary>
    /// Тип предмета (должен определяетс в наследуемом классе)
    /// </summary>
    [HideInInspector]
    public ItemTypes itemType;

    /// <summary>
    /// Событие вызываемое при взятии/продаже предмета
    /// </summary>
    public event EventHandler itemSold;

    public Interactable interactable;

    /// <summary>
    /// Табличка появляющаяся при наведении игрока на предмет
    /// </summary>
    private Name_Plate plate;

    /// <summary>
    /// Источник света предмета
    /// </summary>
    [HideInInspector]
    public GameObject pointLight;

    /// <summary>
    /// Вызов itemSold
    /// </summary>
    protected virtual void RaiseItemSoldEvent()
    {
        itemSold?.Invoke(this,null);
    }
    
    /// <summary>
    /// Рендер, который должен спрятаться при взятии предмета
    /// Ренден, который долж
    /// </summary>
    public Renderer item_renderer;

    /// <summary>
    /// Иконка предмета в UI
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// Цена предмета
    /// </summary>
    public int price;

    protected virtual void Awake()
    {
        SetUpInteraction();
        plate = GetComponentInChildren<Name_Plate>();
        
        if (plate != null)
        {
            plate.SetText(itemName);
            if (price > 0)
            {
                plate.SetPrice(price);
                itemSold += Sold;
            }
            else
            {
                plate.SetPrice(0);
            }
        }
        if (item_renderer != null)
        {
            item_renderer.sortingLayerName = "Default";

            HighLight(false);
        }
    }
    
    /// <summary>
    /// Инициализация предмета 
    /// </summary>
    /// <param name="Price">Цена предмета</param>
    public virtual void SetItem(int Price)
    {
        price = Price;
        itemSold += Sold;
        plate.SetPrice(price);
    }

    public virtual void SetUpInteraction()
    {
        if (interactable == null)
        {
            interactable = GetComponentInChildren<Interactable>();
        }
        interactable.interactWithObject = Interact;
        interactable.highLightObject += HighLight;
    }
    
    /// <summary>
    /// Взаимодействие с предметом
    /// </summary>
    public void Interact()
    {
        //Отдать игроку
        Give(Player.instance);
    }

    /// <summary>
    /// Подсветить предмет
    /// </summary>
    /// <param name="light">Спрятать/Показать</param>
    protected virtual void HighLight(bool light)
    {
        if (light)
        {
            plate.Hide(true);
        }
        else
        {
            plate.Hide(false);
        }
    }

    /// <summary>
    /// Выключить/Включить источник света предмета
    /// </summary>
    /// <param name="light">Включить/Выключить</param>
    protected virtual void Light(bool light)
    {
        if (pointLight != null)
        {
            pointLight.SetActive(light);
        }
    }

    /// <summary>
    /// Дать предмет NPC
    /// </summary>
    /// <param name="parent">NPC которому нужно дать предмет</param>
    /// <returns></returns>
    public virtual bool Give(NPC parent)
    {
        if (parent.npcType == NPCType.Player)
        {
            if (parent.GetComponent<Player>().TakeItem(this))
            {
                RaiseItemSoldEvent();         
                HideItem();
                SetParent(parent, parent.transform);
                Player.instance.DisplayItemDescription(this);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Выкинуть предмет
    /// </summary>
    public virtual void Drop()
    {
        ShowItem();
    }

    public void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// Показать предмет
    /// </summary>
    public virtual void ShowItem()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        transform.parent = null;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
        interactable.CanInteract(true);
        item_renderer.enabled = true;
    }

    /// <summary>
    /// Спрятать предмет
    /// </summary>
    public virtual void HideItem()
    {
        if (item_renderer != null)
        {
            item_renderer.enabled = false;
        }
        interactable.CanInteract(false);
        Light(false);
    }

    /// <summary>
    /// Присвоить родителя
    /// </summary>
    /// <param name="parent">NPC которому будет принадлежать предмет</param>
    /// <param name="transformParent">Обьект к которому будет прикреплен предмет</param>
    protected virtual void SetParent(NPC parent, Transform transformParent)
    {
        npc = parent;
        transform.parent = transformParent;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Вызывает при первом "поднятии" предмета
    /// </summary>
    /// <param name="sender">null</param>
    /// <param name="e">null</param>
    private void Sold(object sender, EventArgs e)
    {
        plate.SetPrice(0);
        Player.instance.RefreshItem();
        price = 0;
        itemSold -= Sold;
    }
}
