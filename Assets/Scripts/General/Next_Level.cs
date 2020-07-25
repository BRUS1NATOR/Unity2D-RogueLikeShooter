using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_Level : MonoBehaviour
{
    public Room room;
    public Interactable interact;

    private bool pressed = false;
    
    void Start()
    {
        if (room == null)
        {
            room = GetComponentInParent<Room>();
        }
        interact.interactWithObject = nextLevel;
    }

    private void nextLevel()
    {
        if (room.Enemies.Count == 0)
        {
            if (Player.instance.isAlive)
            {
                if (pressed == false)
                {
                    GameManager.instance.loadManager.NextLevel();
                }
            }
        }
    }

}
