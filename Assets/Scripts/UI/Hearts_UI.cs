using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI здоровья игрока
/// </summary>
public class Hearts_UI : MonoBehaviour
{
    [SerializeField]
    private Transform heartsParent;
    [SerializeField]
    private Transform armorPlatesParent;


    [SerializeField]
    private Sprite fullHeart = null;

    [SerializeField]
    private Sprite halfHeart = null;

    [SerializeField]
    private Sprite emptyHeart = null;

    [SerializeField]
    private Sprite armorPlate = null;

    public List<GameObject> hearts = new List<GameObject>();
    public List<GameObject> plates = new List<GameObject>();

    public int maxHp;

    private void ReCreate()
    {
        if (maxHp == 0)
        {
            maxHp = Player.instance.stats.maxHealth;

            for (int i = 0; i < maxHp; i += 2)
            {
                CreateHeart();
            }
        }


        if (maxHp > Player.instance.stats.maxHealth) //Если уменьшилось
        {
            for (int i = 0; i < maxHp - Player.instance.stats.maxHealth; i -= 2)
            {
                Destroy(hearts[hearts.Count - 1]);
                hearts.RemoveAt(hearts.Count - 1);
            }
        }
        else
        {
            for (int i = 0; i < Player.instance.stats.maxHealth - maxHp; i += 2)
            {
                CreateHeart();
            }
        }

        for (int i = plates.Count - 1; i >= 0; i--)
        {
            Destroy(plates[i]);
            plates.RemoveAt(i);
        }

        for (int i = 0; i < Player.instance.stats.curArmor; i++)
        {
            CreateArmor();
        }

        maxHp = Player.instance.stats.maxHealth;
    }

    public void Refresh()
    {
        ReCreate();
        int hp = Player.instance.GetHp();

        int count = 2;

        foreach (GameObject H in hearts)
        {
            if (hp >= count)
            {
                H.GetComponent<Image>().sprite = fullHeart;
            }
            else if (hp + 1 >= count)
            {
                H.GetComponent<Image>().sprite = halfHeart;
            }
            else
            {
                H.GetComponent<Image>().sprite = emptyHeart;
            }

            count += 2;
        }
    }

    private void CreateHeart()
    {
        GameObject g = Instantiate(new GameObject(), heartsParent);
        g.AddComponent<Image>().sprite = emptyHeart;
        hearts.Add(g);
    }

    private void CreateArmor()
    {
        GameObject g = Instantiate(new GameObject(), armorPlatesParent);
        g.AddComponent<Image>().sprite = armorPlate;
        plates.Add(g);
    }
}
