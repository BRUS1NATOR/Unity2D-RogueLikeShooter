using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Area_Emitter : MonoBehaviour
{
    private Transform parent;
    public NPC npc;

    public float areaLifeTime;
    public GameObject area;

    private int maximumObjects;
    public float waitTimeForNextObject;

    public GameObject[] pool; 
    public bool emit = false;

    private Color defColor = Color.white;

    void Start()
    {
        maximumObjects = System.Convert.ToInt32(areaLifeTime / waitTimeForNextObject + 1);
        if (npc == null)
        {
            try
            {
                npc = GetComponentInParent<NPC>();
            }
            catch { }
          }
        if (npc != null)
        {
            npc.WakeUpEvent += Begin;
            npc.SleepEvent += End;
            if (npc.room != null)
            {
                parent = npc.room.areasGrid.transform;
            }
        }
    }

    void Begin(object sender, EventArgs e)
    {
        emit = true;

        pool = new GameObject[maximumObjects];
        Renderer rend = area.GetComponent<Renderer>();
        defColor = rend.sharedMaterial.color;

        StartCoroutine(Emit());
    }

    void End(object sender, EventArgs e)
    {
        emit = false;
    }

    public IEnumerator Emit()
    {
        int count = 0;
        while (emit)
        {
            yield return new WaitForSeconds(waitTimeForNextObject);

            if (count >= maximumObjects)
            {
                count = 0;
            }

            if (pool[count] != null)
            {
                if (pool[count].gameObject.activeSelf)
                {
                    pool[count].SetActive(false);
                }

                pool[count].transform.position = transform.position;

                pool[count].gameObject.SetActive(true);
            }
            else
            {
                pool[count] = Instantiate(area, transform.position, transform.rotation);
                if (parent != null)
                {
                    pool[count].transform.parent = parent.transform;
                }
            }
            Live(pool[count],areaLifeTime);
            count++;
        }
    }

    public void Live(GameObject obj, float lifeTime)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material.color = defColor;
        rend.sortingOrder = 1;
        StartCoroutine(WaitToKill(obj, lifeTime));
    }

    private IEnumerator WaitToKill(GameObject obj,float lifeTime)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        yield return new WaitForSeconds(lifeTime / 2f);
        Color col = rend.material.color;
        for (int i = 0; i < 8; i++)
        {
            rend.sortingOrder += 1;
            col.a -= 0.1f;
            rend.material.color = col;
            yield return new WaitForSeconds((lifeTime / 2f)/8);
        }
        obj.SetActive(false);
    }
}
