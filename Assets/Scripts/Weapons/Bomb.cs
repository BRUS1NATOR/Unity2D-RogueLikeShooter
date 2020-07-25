using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Бомба, которая взрывается
/// </summary>
public class Bomb : MonoBehaviour
{
    /// <summary>
    /// Время до взрыва
    /// </summary>
    public float time;
    /// <summary>
    /// Радиус взрыва
    /// </summary>
    public float radius;
    /// <summary>
    /// Сила взрыва
    /// </summary>
    public float power;
    /// <summary>
    /// Урон наносимый взрывом
    /// </summary>
    public int damage;
    /// <summary>
    /// Корутин - таймер
    /// </summary>
    private Coroutine waitForExplodeCor;
    void Start()
    {
        waitForExplodeCor=StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(time);
        DoDamage();
    }

    private void DoDamage()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        ExplosionManager.Explode(transform.position, radius, damage, power, ExplosionManager.ExplosionType.bomb);
        Destroy();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void ExplodeNow()
    {
        if (waitForExplodeCor != null)
        {
            StopCoroutine(waitForExplodeCor);
        }
        DoDamage();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
