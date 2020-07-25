using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects_UI : MonoBehaviour
{
    public GameObject SlimeOilEffects;
    public GameObject FlameWaterEffects;

    public SpriteRenderer slime;
    public SpriteRenderer oil;

    public SpriteRenderer flame;
    public SpriteRenderer water;

    private void Start()
    {
        SlimeOilEffects.SetActive(false);
        FlameWaterEffects.SetActive(false);
        StartCoroutine(UpdateEffects());
    }

    private IEnumerator UpdateEffects()
    {
        float slimeOil;
        float flameWater;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            slimeOil = Player.instance.stats.effects.slimeEffect;
            flameWater = Player.instance.stats.effects.flameEffect;

            if (slimeOil != 0)
            {
                SlimeOilEffects.SetActive(true);
                if (slimeOil > 0)
                {
                    slime.enabled = true;
                    slime.GetComponent<Renderer>().material.SetFloat("_Position",Mathf.Abs(slimeOil));

                    oil.enabled = false;

                }
                else
                {
                    slime.enabled = false;

                    oil.enabled = true;
                    oil.GetComponent<Renderer>().material.SetFloat("_Position", Mathf.Abs(slimeOil));
                }
            }
            else
            {
                SlimeOilEffects.SetActive(false);
            }

            if (flameWater != 0)
            {
                FlameWaterEffects.SetActive(true);
                if (flameWater > 0)
                {
                    flame.enabled = true;
                    flame.GetComponent<Renderer>().material.SetFloat("_Position", Mathf.Abs(flameWater));

                    water.enabled = false;

                }
                else
                {
                    flame.enabled = false;

                    water.enabled = true;
                    water.GetComponent<Renderer>().material.SetFloat("_Position", Mathf.Abs(flameWater));
                }
            }
            else
            {
                FlameWaterEffects.SetActive(false);
            }

        }
    }
}
