using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Name_Plate : MonoBehaviour
{
    public GameObject plate;

    public Text nameText;
    public Text priceText;

    public void SetText(string s)
    {
        nameText.text = s;
    }

    public void SetPrice(int price)
    {
        if (price > 0)
        {
            priceText.text = price + "coins";
        }
        else
        {
            priceText.text = "";
        }
    }

    public void Hide(bool hide)
    {
        plate.SetActive(hide);
    }
}
