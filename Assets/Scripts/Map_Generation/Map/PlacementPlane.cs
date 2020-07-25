using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlacementPlane : MonoBehaviour
{
    public Placement place;
    public bool hiddenFromMap=true;

    public NavMeshSurface[] navMaps;

    public GameObject navPlane;
    public Renderer mapPlane;
    public GameObject miniMapIcon;

    private Material defMaterial;
    public Material lightMaterial;

    public Renderer blackOutRenderer;

    private void Start()
    {
        defMaterial = mapPlane.material;
    }

    private void CreateNavigation()
    {
        navMaps = navPlane.GetComponents<NavMeshSurface>();
        navMaps[0].BuildNavMesh();
        navMaps[1].BuildNavMesh();
    }

    public void Setup(Placement place)
    {
        this.place = place;
        transform.position = new Vector3(place.transform.position.x + place.width / 2, place.transform.position.y + place.height / 2 + 0.5f, 9);
        transform.localScale = new Vector3(place.width / 9f, 1, place.height / 9f);
        transform.parent = place.transform;
        CreateNavigation();
        blackOutRenderer.sortingLayerName = "Upper";
    }

    public void LightOut()
    {
        blackOutRenderer.material.color = new Color(0, 0, 0, 0);
    }

    public void HighLight(bool light)
    {
        if (light)
        {
            mapPlane.material = lightMaterial;
        }
        else
        {
            mapPlane.material = defMaterial;
        }
    }

    public void SetMiniMapIcon(Sprite icon, Vector2? cord)
    {
        if (miniMapIcon != null)
        {
            Destroy(miniMapIcon);
        }

        if (cord.HasValue)
        {
            Vector3 c = cord.Value;
            c.z = 10;
            miniMapIcon = Instantiate(new GameObject(), c, Quaternion.identity);
        }
        else
        {
            miniMapIcon = Instantiate(new GameObject(), place.GetCenter(), Quaternion.identity);
        }
        miniMapIcon.transform.parent = transform;
        miniMapIcon.name = "MiniMapIcon";
        miniMapIcon.layer = LayerMask.NameToLayer("MiniMap");
        SpriteRenderer s = miniMapIcon.AddComponent<SpriteRenderer>();
        s.sprite = icon;
    }

    public void HideFromMap(bool hide)
    {
        mapPlane.enabled = !hide;
        if (miniMapIcon != null)
        {
            miniMapIcon.SetActive(!hide);
            hiddenFromMap = !hide;
        }
    }
}