using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

    public bool isOpen;
    public Direction4D direction;

    public GameObject[] room;

    public GameObject DoorCollider;
    public DoorTrigger DoorTrigger;

    // Use this for initialization
    void Awake()
    {
        DoorTrigger = GetComponentInChildren<DoorTrigger>();

        isOpen = true;
    }

    public virtual void Lock()
    {
        isOpen = false;
        DoorCollider.SetActive(true);

        animator.SetBool("Open", false);
    }

    public virtual void UnLock()
    {
        isOpen = true;
        DoorCollider.SetActive(false);

        if (DoorTrigger.playerNearby)
        {
            animator.SetBool("Open", true);
        }
    }
}