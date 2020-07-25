using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats_UI : MonoBehaviour
{
    [SerializeField]
    public GameObject coin = null;

    [SerializeField]
    private GameObject coinText = null;

    private Text text;

    private void Awake()
    {
        text = coinText.GetComponent<Text>();
    }

    public void Refresh()
    {
        text.text = Player.instance.coins.ToString();
    }
}
