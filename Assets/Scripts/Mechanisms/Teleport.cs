using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Door
{
    public Lever lever;
    public Color fadeColor;
    public GameObject lamp;

    private void Start()
    {
        Lock();
        lever.toggle = OpenMap;
    }

    public override void Lock()
    {
        base.Lock();
        lamp.SetActive(false);
    }

    public override void UnLock()
    {
        base.UnLock();
        lamp.SetActive(true);
    }

    public void OpenMap(bool toggle)
    {
        if (toggle == true)
        {
            Player.instance.mapCamera.TurnOnBigMap(this);
        }
        else
        {
            DoorTrigger.enabled = true;
            UnLock();
        }
    }

    public void TeleportTo(Placement place)
    {
        Player.instance.canShoot = false;
        Player.instance.movement.dontMove = true;
        DoorTrigger.enabled = false;
        StartCoroutine(TeleportCoroutine(place));
    }


    private IEnumerator TeleportCoroutine(Placement room)
    {
        animator.SetBool("Open", false);
        yield return SimpleFunctions.WaitForEndOfAnimation(animator);
        animator.SetBool("isTeleporting", true);
        yield return SimpleFunctions.WaitForEndOfAnimation(animator);

        yield return StartCoroutine(Camera.main.GetComponent<Camera_Bounds>().FadeScreen(true, 2f,fadeColor));
        GameManager.instance.TeleportPlayer(room);

        StartCoroutine(Camera.main.GetComponent<Camera_Bounds>().FadeScreen(false, 2f, fadeColor));
        Player.instance.canShoot = true;
        Player.instance.movement.dontMove = false;
        animator.SetBool("isTeleporting", false);
    }
}
