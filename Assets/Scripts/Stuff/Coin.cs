using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().coins += 1;
            collision.GetComponent<Player>().actionBar.RefreshStats();
            Destroy(this.gameObject);
        }
    }
}