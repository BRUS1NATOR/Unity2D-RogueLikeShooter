using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum HighLightColor { White}


[RequireComponent(typeof(Collider2D))]
public sealed class Interactable : MonoBehaviour
{
    HighLightColor color;

    public float minDistance;
    public Renderer rend;
    private Material defMat;

    private Material lightMat;

    public delegate void lightOn(bool light);
    public lightOn highLightObject;

    public delegate void interact();
    public interact interactWithObject;

    public bool canInteract;
    private Collider2D interactCollider;

    public void Awake()
    {
        interactCollider = GetComponent<Collider2D>();
        if (minDistance < 0.5f)
        {
            minDistance = 2.5f;
        }
        defMat = rend.material;
        highLightObject += HighLightObject;
    }
    public void Start()
    {
        lightMat = ItemManager.instance.highLightMaterial;
    }

    private void OnMouseEnter()
    {
        if(Player.instance!= null) { 
        float dist = Vector2.Distance(Player.instance.transform.position, this.transform.position);
            if (dist < minDistance)
            {
                highLightObject(true);
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.E))
        {
            float dist = Vector2.Distance(Player.instance.transform.position, this.transform.position);
            if (dist < minDistance)
            {
                interactWithObject();
            }
        }
    }

    private void OnMouseExit()
    {
        highLightObject(false);
    }

    public void HighLightObject(bool light)
    {
        if (light)
        {
            rend.material = lightMat;
        }
        else
        {
            rend.material = defMat;
        }
    }

    public void CanInteract(bool canTake)
    {
        if (canTake == true)
        {
            interactCollider.enabled = true;
        }
        else
        {
            interactCollider.enabled = false;
        }
    }
}
