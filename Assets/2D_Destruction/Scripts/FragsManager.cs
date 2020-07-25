using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragsManager : MonoBehaviour
{
    public float timeWait;
    public bool disableRB;
    public void GetFragments(List<GameObject> frags, Vector2 velocity)
    {
        if(velocity == Vector2.zero)
        {
            velocity = Vector2.one;
        }
        ChangeVelocity(frags, velocity);
        StartCoroutine(ChangeLayer(frags));
    }

    private IEnumerator ChangeLayer(List<GameObject> frags)
    {
        yield return new WaitForSeconds(timeWait);
        foreach (GameObject frag in frags)
        {
            if (disableRB)
            {
                Destroy(frag.GetComponent<Collider2D>());
                Destroy(frag.GetComponent<Rigidbody2D>());
            }
            frag.GetComponent<Renderer>().sortingLayerName = "Floor";
        }
        StartCoroutine(ChangeColor(frags));
    }

    private void ChangeVelocity(List<GameObject> frags, Vector2 velocity)
    {
        foreach (GameObject frag in frags)
        {
            frag.GetComponent<Rigidbody2D>().velocity = velocity * Random.Range(-1f,1f);
        }
    }

    private IEnumerator ChangeColor(List<GameObject> frags)
    {
        for(int i = 0; i < 10; i++)
        {
            foreach (GameObject frag in frags)
            {
                var color = frag.GetComponent<Renderer>().material.color;
                color.a -= 0.05f;
                frag.GetComponent<Renderer>().material.color = color;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}