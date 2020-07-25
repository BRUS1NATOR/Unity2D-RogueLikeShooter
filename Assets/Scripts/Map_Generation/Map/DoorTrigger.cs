using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool playerNearby;
    private Door parent;

    private void Awake()
    {
        parent = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerNearby = true;
        if (parent.isOpen)
        {
            parent.animator.SetBool("Open", true);
            parent.DoorCollider.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerNearby = false;
        if (parent.isOpen)
        {
            parent.animator.SetBool("Open", false);
            parent.DoorCollider.SetActive(true);
        }
    }
}
