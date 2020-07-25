using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
