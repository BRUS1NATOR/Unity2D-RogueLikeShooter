using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public EffectType areaEffect;

    [Header("4 times per second, recommend 0.05f - 0.2f")]
    public float value;

    private void Start()
    {
        switch (areaEffect)
        {
            case EffectType.Flame:
                {
                    Renderer[] renderers = GetComponentsInChildren<Renderer>();
                    foreach (Renderer ren in renderers)
                    {
                        Material mat = ren.material;
                        if (mat.shader.name == "Custom/Fire")
                        {
                            mat.SetFloat("_Offset", Random.Range(0f, 1f));
                            int w = mat.GetInt("_GradientWidthHeight");
                            mat.SetInt("_PositionX", Random.Range(0,w));
                            mat.SetInt("_PositionY", Random.Range(0, w));
                        }
                    }
                    break;
                }

                   
        }
    }
}
