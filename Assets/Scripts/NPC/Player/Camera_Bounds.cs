using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CameraMovement { ChasePlayer, MoveAround, Idle, Freeze}

public class Camera_Bounds : MonoBehaviour
{
    public static Camera_Bounds instance;
    public Vector3 mousePos;
    private Vector3 sightMousePos;
    public CameraMovement movement;
    public Vector3 destination = Vector3.zero;

    public Image fadeImage;
    public GameObject extraLight;

    [SerializeField]
    private bool initialized = false;

    public Room roomNow;
    public Room roomNext;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame

    void LateUpdate()
    {
        switch (movement)
        {
            case CameraMovement.ChasePlayer:
                {
                    extraLight.SetActive(false);
                    if (Time.timeScale != 0)
                    {
                        if (Player.instance != null)
                        {
                            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            sightMousePos = Player.instance.sightCamera.ScreenToWorldPoint(Input.mousePosition);
                            float posX = Mathf.Clamp(sightMousePos.x, Player.instance.transform.position.x - 3, Player.instance.transform.position.x + 3);
                            float posY = Mathf.Clamp(sightMousePos.y, Player.instance.transform.position.y - 2, Player.instance.transform.position.y + 2);

                            transform.position = Vector3.MoveTowards(transform.position, new Vector3(posX, posY, 0), 0.25f);
                        }
                    }
                    break;
                }
            case CameraMovement.MoveAround:
                {
                    extraLight.SetActive(true);
                    if (Map.mapIsGenerated == true)
                    {
                        if (initialized == false)
                        {
                            if (Map.roomObjs != null)
                            {
                                if (roomNow == null)
                                {
                                    roomNow = Map.roomObjs[Random.Range(0, Map.roomObjs.Count)];
                                    transform.position = roomNow.transform.position;
                                }
                                else
                                {
                                    roomNow = roomNext;
                                }
                                roomNext = roomNow.corridors[Random.Range(0, roomNow.corridors.Count)].corridor.Rooms[Random.Range(0, 2)];
                                destination = new Vector2((roomNext.start.x + roomNext.width / 2), (roomNext.start.y + roomNext.height / 2));

                                initialized = true;

                                StartCoroutine(Move(4));
                            }
                        }
                    }
                    else
                    {
                        initialized = false;
                    }
                    break;
                }
            case CameraMovement.Idle:
                {
                    //
                    break;
                }
            case CameraMovement.Freeze:
                {

                    break;
                }
        }

    }

    public IEnumerator FadeScreen(bool fade, float time)
    {
        if (time < 1)
        {
            if (fade)
            {
                fadeImage.color = new Color(0, 0, 0, 1);
            }
            else
            {
                fadeImage.color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            time /= 50;
            if (fade)
            {
                if (fadeImage.color.a != 1f)
                {
                    for (int i = 0; i <= 50; i++)
                    {
                        fadeImage.color = new Color(0, 0, 0, (float)(0.02f * System.Convert.ToDouble(i)));
                        yield return new WaitForSecondsRealtime(time);
                    }
                }
            }
            else
            {
                for (int i = 50; i >= 0; i--)
                {
                    fadeImage.color = new Color(0, 0, 0, (float)(0.02f * System.Convert.ToDouble(i)));
                    yield return new WaitForSecondsRealtime(time);
                }
            }
        }
        yield return 0;
    }

    public IEnumerator FadeScreen(bool fade, float time, Color color)
    {
        if (time < 1)
        {
            if (fade)
            {
                fadeImage.color = new Color(color.r, color.g, color.b, 1);
            }
            else
            {
                fadeImage.color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            time /= 50;
            if (fade)
            {
                if (fadeImage.color.a != 1f)
                {
                    for (int i = 0; i <= 50; i++)
                    {
                        fadeImage.color = new Color(color.r, color.g, color.b, (float)(0.02f * System.Convert.ToDouble(i)));
                        yield return new WaitForSecondsRealtime(time);
                    }
                }
            }
            else
            {
                for (int i = 50; i >= 0; i--)
                {
                    fadeImage.color = new Color(color.r, color.g, color.b, (float)(0.02f * System.Convert.ToDouble(i)));
                    yield return new WaitForSecondsRealtime(time);
                }
            }
        }
        yield return 0;
    }

    float d = 1f / 32;


    public IEnumerator Move(int speed)
    {
        float step = (float)speed / 100;
        while ((transform.position.x != destination.x || transform.position.y != destination.y) && movement == CameraMovement.MoveAround)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, step);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        initialized = false;
    }
}
