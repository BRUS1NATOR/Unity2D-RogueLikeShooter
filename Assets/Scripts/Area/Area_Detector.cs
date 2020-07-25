using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Detector : MonoBehaviour
{
    public NPC npc;
    int layerMask;

    private RaycastHit2D hit;
       // Start is called before the first frame update
    void Awake()
    {
        layerMask = 1 << LayerMask.NameToLayer("Area");

        if (npc == null)
        {
            npc = GetComponentInParent<Movement_Agent>().child;
        }

        npc.WakeUpEvent += WakeUp;
    }

    public void WakeUp(object sender, EventArgs e)
    {
        StartCoroutine(CastRay());
    }

    private IEnumerator CastRay()
    {
        while (npc.isAlive)
        {
            yield return new WaitForSeconds(0.25f);

            Area area = GetArea();
            if (area != null)
            {
                npc.stats.AddEffect(area.areaEffect, area.value);
            }
        }
    }

    public Area GetArea()
    {
        hit = Physics2D.Raycast(transform.position, Vector3.forward, 25, layerMask);
        if (hit.collider != null)
        {
            return hit.collider.gameObject.GetComponent<Area>();
        }
        return null;
    }
}
