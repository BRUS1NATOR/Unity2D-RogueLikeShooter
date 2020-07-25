using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownFix : MonoBehaviour
{
    public bool isActive;
    public string sortingLayerDefault = "Default"; //Or what ever layer you want it to be
    public string sortingLayer = "UI"; //Or what ever layer you want it to be

    public void OnDropDownClick()
    {
        isActive = !isActive;
        Transform droplist = transform.Find("Dropdown List");

        if (droplist != null)
        {
            if (isActive)
            {
                droplist.GetComponent<Canvas>().sortingLayerName = sortingLayer;
            }
            else
            {
                droplist.GetComponent<Canvas>().sortingLayerName = sortingLayerDefault;
            }
        }
    }

}
