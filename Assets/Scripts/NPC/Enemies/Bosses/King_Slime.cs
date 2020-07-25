using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King_Slime : Boss
{
    public float timeToSpawnSlime;

    public GameObject slime;
    public GameObject slimeSpawnAnimator;

    public List<GameObject> slimes;
    private Coroutine slimeCor;

    public override IEnumerator Die(bool destroy)
    {
        yield return StartCoroutine(base.Die(destroy));

        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        sp.color = new Color(255, 255, 255, 0.5f);
    }

    protected override void Awake()
    {
        base.Awake();

        slimes = new List<GameObject>();
        WakeUpEvent += SpawnSlimes;
        SleepEvent += StopSpawnSlimes;

        if (timeToSpawnSlime < 5)
        {
            timeToSpawnSlime = 15;
        }
    }

    public void SpawnSlimes(System.Object sender, EventArgs e)
    {
        if (slimeCor == null)
        {
            slimeCor = StartCoroutine(SpawnSlimes());
        }
    }
    public void StopSpawnSlimes(System.Object sender, EventArgs e)
    {
        if (slimeCor != null)
        {
            StopCoroutine(slimeCor);
        }
    }

    private IEnumerator SpawnSlimes()
    {
        while (isAlive)
        {
            Debug.Log("SPAWN SLIMES!");
            if (room.enemiesAlive < 3)
            {
                Debug.Log("SPAWN SLIMES < 3");
                yield return  StartCoroutine(EnemiesManager.SpawnEnemy(room, slime, slimeSpawnAnimator, 4));
                room.EnemiesWakeUp();
            }
            Debug.Log("WAITFORSEC");
            yield return new WaitForSeconds(timeToSpawnSlime);
            Debug.Log("Waited 15");
        }
    }

    protected override IEnumerator SpellZero()
    {
        List<Vector3> startPositions = SimpleFunctions.RandomCircle(bulStartPoint.position, 0.5f, 4, 0);
        List<Vector3> endPositions = SimpleFunctions.RandomCircle(bulStartPoint.position, 1.0f, 4, 0);
        for (int j = 0; j < startPositions.Count; j++)
        {
            BulletInfo bul = new BulletInfo(startPositions[j], endPositions[j], bullet_speed, 1, 1, 1);
            Bullet bullet = Instantiate(bulletObj, startPositions[j], Quaternion.identity).GetComponent<Bullet>();
            bullet.Setup(bul, 1, transform.rotation, false);
        }

        yield return new WaitForSeconds(0.5f);

        yield return new WaitForEndOfFrame();
    }
}
