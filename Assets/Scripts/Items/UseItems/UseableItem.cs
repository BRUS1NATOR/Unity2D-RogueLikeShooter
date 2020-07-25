using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предметы которые могут многократно использоваться
/// </summary>
public class UseableItem : Item
{
    /// <summary>
    /// Событие "использовать предмет"
    /// </summary>
    public event EventHandler useItem;
    /// <summary>
    /// Событие "обновить состояние предмета"
    /// </summary>
    public event EventHandler updateItem;
    /// <summary>
    /// Событие "закончить использование предмета"
    /// </summary>
    public event EventHandler unUseItem;

    /// <summary>
    /// Время использования предмета
    /// </summary>
    public int useTime;

    /// <summary>
    /// Шаг с которым убывает заряд
    /// </summary>
    public int chargeStep;

    /// <summary>
    /// Максимальный заряд предмета
    /// </summary>
    public int chargeMax;

    /// <summary>
    /// Текущий заряд предмета
    /// </summary>
    private int chargeNow;

    /// <summary>
    /// Можно ли использовать
    /// </summary>
    public bool canUse;
    /// <summary>
    /// Используется ли предмет сейчас
    /// </summary>
    public bool isActive=false;

    public int charge
    {
        get
        {
            return chargeNow;
        }
        set
        {
            if (value > chargeNow)
            {
                if (isActive == false)
                {
                    chargeNow = value;
                    if (chargeNow >= chargeMax)
                    {
                        chargeNow = chargeMax;
                        canUse = true;
                    }
                }
            }
            else
            {
                chargeNow = value;
            }
            RaiseItemUpdateEvent();
        }
    }

    /// <summary>
    /// Использовать предмет
    /// </summary>
    protected virtual void RaiseItemUseEvent()
    {
        useItem?.Invoke(this, null);
    }

    /// <summary>
    /// Обновить состояние предемета
    /// </summary>
    protected virtual void RaiseItemUpdateEvent()
    {
        updateItem?.Invoke(this, null);
    }

    /// <summary>
    /// Закончить использование предмета
    /// </summary>
    protected virtual void RaiseItemUnUseEvent()
    {
        unUseItem?.Invoke(this, null);
    }

    /// <summary>
    /// Анимация предмета в UI
    /// </summary>
    public RuntimeAnimatorController uiAnimator;

    protected override void Awake()
    {
        base.Awake();
        itemType = ItemTypes.UseableItem;
    }

    public virtual void Use()
    {
        RaiseItemUseEvent();
    }

    public virtual void UnUse()
    {
        isActive = false;
        chargeNow = 0;
        RaiseItemUnUseEvent();
    }
}
