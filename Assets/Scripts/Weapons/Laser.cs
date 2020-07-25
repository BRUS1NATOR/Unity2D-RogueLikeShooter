using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool pointsAreDirection;
    public bool visible = false;
    public bool rayCast;

    private int mask;

    public GameObject endOfLine;
    public LineRenderer lineRenderer;
    public Material lineMaterial;

    public float length;
    public float lineWidth;

    private Coroutine scaleDownCor;
    // Start is called before the first frame update
    private void Awake()
    {
        mask = 1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("HitBox"); //| 1 << LayerMask.NameToLayer("Environment");

        endOfLine = Instantiate(endOfLine, this.transform);

        length = 5;
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        SetScale();
        lineRenderer.enabled = false;

        endOfLine.SetActive(false);
    }

    private void SetScale()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    public void Hide()
    {
        endOfLine.SetActive(false);
        scaleDownCor = StartCoroutine(ScaleDown());
    }

    private IEnumerator ScaleDown()
    {
        for(int i = 0; i < 4; i++)
        {
            lineRenderer.endWidth /= 1.5f;
            lineRenderer.startWidth /= 1.5f;
            yield return new WaitForSeconds(0.1f);
        }

        lineRenderer.enabled = false;
        SetScale();
    }

    private void prepareToShow(Vector2 start, Vector2 end)
    {
        if (scaleDownCor != null)
        {
            StopCoroutine(scaleDownCor);
            SetScale();
        }
        lineRenderer.enabled = true;

        endOfLine.SetActive(true);
        if (start.x > end.x)
        {
            endOfLine.transform.right = (start - end).normalized;
        }
        else
        {
            endOfLine.transform.right = -(start - end).normalized;
        }

        if (length <= 0)
        {
            length = Vector3.Distance(end, start);
        }
    }

    public RaycastHit2D[] Show(Vector2 start, Vector2 end)
    {
        prepareToShow(start, end);

        if (rayCast)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(start, (end - start).normalized, length, mask);

            if (hits.Length > 0)
            {
                lineRenderer.SetPositions(new Vector3[] { start, hits[0].point });
                endOfLine.transform.position = lineRenderer.GetPosition(1);
                return hits;
            }
        }
        endOfLine.transform.position = lineRenderer.GetPosition(1);
        lineRenderer.SetPositions(new Vector3[] { start, end + (end - start).normalized * length });
        return null;
    }

    public RaycastHit2D[] Show(Vector2 start, Vector2 end, bool belongsToPlayer)
    {
        prepareToShow(start, end);

        if (rayCast)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(start, (end - start).normalized, length, mask);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length && i < 2; i++)
                {
                    if (!belongsToPlayer == (hits[i].collider.name == "PlayerHitBox"))
                    {
                        lineRenderer.SetPositions(new Vector3[] { start, hits[0].point });
                        endOfLine.transform.position = lineRenderer.GetPosition(1);
                        return hits;
                    }
                }
            }
        }
        endOfLine.transform.position = lineRenderer.GetPosition(1);
        
        lineRenderer.SetPositions(new Vector3[] { start, end + (end - start).normalized * length });
        return null;
    }
}
