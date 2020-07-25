using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail_Damage : MonoBehaviour
{
    public EffectType areaEffect;

    [Header("4 times per second, recommend 0.05f - 0.2f")]
    public float value; 

    public TrailRenderer trail;

    const int MAX_POSITIONS = 250;
    Vector3[] TrailRecorded = new Vector3[MAX_POSITIONS];
    int layerMask;

    void Start()
    {
        layerMask = /*1 << LayerMask.NameToLayer("Enemy") |*/ 1 << LayerMask.NameToLayer("Area");
        StartCoroutine(GetPoints());
    }

    private IEnumerator GetPoints()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            int numberOfPositions = GetComponent<TrailRenderer>().GetPositions(TrailRecorded);  //Записываем в массив, и получаем количество записанных(переписанных) данных в массиве

            for (int i = 0; i < numberOfPositions - 1; i++)
            {

                RaycastHit2D hit = Physics2D.Linecast(TrailRecorded[i], TrailRecorded[i + 1], layerMask);

                if (hit.collider != null)
                {
                    NPC npc = hit.collider.gameObject.GetComponentInParent<NPC>();
                    npc.stats.AddEffect(areaEffect, value);
                }

                Debug.DrawLine(TrailRecorded[i], TrailRecorded[i + 1], Color.green, 0.25f);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

