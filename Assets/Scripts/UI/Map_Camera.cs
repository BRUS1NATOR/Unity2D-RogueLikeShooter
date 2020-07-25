using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class Map_Camera : MonoBehaviour
{
    Vector2 sightMousePos;

    [HideInInspector]
    public bool bigMap;

    private Camera cam;
    public Teleport teleport;

    public Canvas bigMapCanvas;
    public RawImage miniMapImage;
    public RawImage bigMapImage;

    void Start()
    {
        cam = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void LateUpdate()
    {
        if (Time.timeScale != 0)
        {
            if (Player.instance != null)
            {
                if (bigMap)
                {
                    sightMousePos = Player.instance.sightCamera.ScreenToWorldPoint(Input.mousePosition);

                    if (teleport != null)
                    {
                        Vector2 worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                        foreach (Room r in Map.roomObjs)
                        {
                            if (SimpleFunctions.Check_Superimpose(worldMousePos, r.start, r.end))
                            {
                                if (r.hereWasPlayer)
                                {
                                    r.HighLight(true);
                                    if (Input.GetButton("Fire1"))
                                    {
                                        teleport.TeleportTo(r);
                                        TurnOffBigMap(false);
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                r.HighLight(false);
                            }
                        }
                    }

                    Vector3 v = transform.position;

                    if(sightMousePos.x < Player.instance.transform.position.x-4)
                    {
                        v.x -= sightMousePos.normalized.x;
                    }
                    else if(sightMousePos.x > Player.instance.transform.position.x + 4)
                    {
                        v.x += sightMousePos.normalized.x;
                    }

                    if (sightMousePos.y < Player.instance.transform.position.y - 3)
                    {
                        v.y -= sightMousePos.normalized.y;
                    }
                    else if (sightMousePos.y > Player.instance.transform.position.y + 3)
                    {
                        v.y += sightMousePos.normalized.y;
                    }

                    v.x = Mathf.Clamp(v.x, Map.lowestVector.x, Map.highestVector.x);
                    v.y = Mathf.Clamp(v.y, Map.lowestVector.y, Map.highestVector.y);

                    transform.position = Vector3.MoveTowards(transform.position, v, 1f);
                }
            }
        }
    }

    public void TurnOnBigMap(Teleport teleport)
    {
        this.teleport = teleport;
        Player.instance.canShoot = false;
        Player.instance.movement.dontMove = true;
        bigMapCanvas.enabled = true;
        RenderTexture tex = new RenderTexture(Screen.width, Screen.height, 0);
        cam.orthographicSize = 90;
        cam.targetTexture = tex;
        bigMapImage.texture = tex;
        bigMap = true;
        Camera.main.GetComponent<Camera_Bounds>().movement = CameraMovement.Freeze;
        Camera.main.transform.position = Player.instance.transform.position;
    }

    public void TurnOffBigMap(bool unFreezePlayer)
    {
        teleport = null;
        if (unFreezePlayer)
        {
            Player.instance.canShoot = true;
            Player.instance.movement.dontMove = false;
        }
        bigMapCanvas.enabled = false;
        RenderTexture tex = new RenderTexture(300, 300, 0);
        cam.orthographicSize = 45;
        cam.targetTexture = tex;
        miniMapImage.texture = tex;
        bigMap = false;
        transform.localPosition = Vector3.zero;
        Camera.main.GetComponent<Camera_Bounds>().movement = CameraMovement.ChasePlayer;
        foreach (Room r in Map.roomObjs)
        {
            r.HighLight(false);
        }
    }
}
