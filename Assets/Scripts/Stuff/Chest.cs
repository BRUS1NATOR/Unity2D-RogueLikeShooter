using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chest : Destructible
{
    public GameObject itemInside;
    public RandomItems randItems;

    public bool needKey;

    private Interactable interactable;

    protected override void Awake()
    {
        base.Awake();

        interactable = GetComponentInChildren<Interactable>();
        interactable.interactWithObject = Open;
    }

    public void Open()
    {
        if (interactable.canInteract == false)
        {
            if (needKey == true)
            {
                //Key-=1;
                interactable.canInteract = true;
                Debug.Log("NEED KEY");
                return;
            }

            if (itemInside == null)
            {
                itemInside = randItems.GetItem(false);
            }

            if (itemInside.GetComponent<Item>() != null)
            {
                itemInside.GetComponent<Item>().interactable.CanInteract(false);
            }

            itemInside.GetComponent<Item>().interactable.CanInteract(false);

            itemInside.transform.position = transform.position;
            itemInside.transform.parent = transform;
            itemInside.transform.localPosition = new Vector3(0, -0.4f, 0f);

            animator.SetBool("Open", true);

            StartCoroutine(playAnimation(itemInside));
        }
    }

    private IEnumerator playAnimation(GameObject item)
    {
        yield return StartCoroutine(moveUp(item));
        yield return new WaitForSeconds(1);
        if (itemInside.GetComponent<Item>() != null)
        {
            item.GetComponent<Item>().interactable.CanInteract(true);
        }
        yield return 0;
    }

    private IEnumerator moveUp(GameObject item)
    {
        for(int i = 0; i < 10; i++)
        {
            item.transform.position = new Vector2(item.transform.position.x, item.transform.position.y + 0.05f);

            yield return new WaitForSeconds(0.1f);
        }
        yield return 0;
    }
}
