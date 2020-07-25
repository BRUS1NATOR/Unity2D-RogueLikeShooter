using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Lever : MonoBehaviour
{
    Animator animator;
    public bool isUp = true;
    bool coolDown = false;

    public delegate void turnSwitch(bool toggleOnOff);
    public turnSwitch toggle;

    private SpriteRenderer spriteRenderer;
    private Material defMaterial;
    public Material lightMaterial;

    Interactable interactable;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactable = GetComponent<Interactable>();

        lightMaterial = ItemManager.instance.highLightMaterial;
        defMaterial = spriteRenderer.material;

        interactable.highLightObject = Light;
        interactable.interactWithObject = Turn;
    }

    private void Turn()
    {
        if (coolDown == false)
        {
            coolDown = true;
            isUp = !isUp;
            animator.SetBool("isUp", isUp);
            toggle(!isUp);
            StartCoroutine(WaitForCoolDown(1f));
        }
    }

    private IEnumerator WaitForCoolDown(float sec)
    {
        yield return new WaitForSeconds(sec);
        coolDown = false;
    }

    protected virtual void Light(bool light)
    {
        if (light)
        {
            spriteRenderer.material = lightMaterial;
        }
        else
        {
            spriteRenderer.material = defMaterial;
        }
    }
}
