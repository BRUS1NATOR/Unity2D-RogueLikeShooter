using System;
using UnityEngine;

[Serializable]
public class Wall
{
    public string info;
    public Direction4D direction;
    public GameObject gameObject;
    public bool hasDecoration = false;

    public Wall(Direction4D dir)
    {
        direction = dir;
    }

    public void CreateDecoration(GameObject decortaion, Vector2 localPosition)
    {
        if (hasDecoration == false)
        {
            hasDecoration = true;
            MonoBehaviour.Instantiate(decortaion, new Vector3(gameObject.transform.position.x + localPosition.x, gameObject.transform.position.y + localPosition.y), Quaternion.identity, gameObject.transform);
        }
    }

    public void CreateDecoration(GameObject decortaion)
    {
        if (hasDecoration == false)
        {
            hasDecoration = true;
            MonoBehaviour.Instantiate(decortaion, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
        }
    }

    public void CreateWallModificator(GameObject modificator)
    {
        MonoBehaviour.Instantiate(modificator, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
    }


    public void Destroy()
    {
        info += "GAMEOBJECT DESTROYED";
       UnityEngine.Object.Destroy(gameObject);
    }
}
