using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weareble : MonoBehaviour
{
    public bool facingRight;

    private void Awake()
    {
        facingRight = true;
    }

    public void Hide()
    {
        Renderer render = GetComponent<Renderer>();
        render.enabled = false;
    }

    public void Show()
    {
        Renderer render = GetComponent<Renderer>();
        render.enabled = true;
    }
}