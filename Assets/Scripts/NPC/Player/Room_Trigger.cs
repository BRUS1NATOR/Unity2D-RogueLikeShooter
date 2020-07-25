using System.Collections.Generic;
using UnityEngine;

public class Room_Trigger : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            player.room = collision.GetComponent<Room>();
            player.room.HideFromMap(false);
            CheckEnter();
            if (player.corridor != null)
            {
                player.corridor.HideFromMap(false);
            }
        }

        if (collision.CompareTag("Corridor"))
        {
            player.corridor = collision.GetComponent<Corridor>();
            CheckEnter();
            if (player.corridor != null)
            {
                player.corridor.HideFromMap(false);
            }
        }
        if (collision.CompareTag("SecretCorridor"))
        {
            Debug.Log("SECRET !!!!");
            player.corridor = collision.GetComponent<Corridor>();
            CheckEnter();
        }

        if (collision.CompareTag("Shop"))
        {
            player.room = collision.GetComponent<Room>();
            player.room.HideFromMap(false);
            CheckEnter();

            GameManager.instance.audioManager.PlayMusic(MusicType.shop);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Room") || collision.CompareTag("Shop"))
        {
            player.room = collision.GetComponent<Room>();
            player.room.OpenDoors();
            player.room = null;
        }
        if (collision.CompareTag("Corridor"))
        {
            player.corridor = null;
            CheckEnter();
        }
    }

    private void CheckEnter()
    {
        if (player.room != null && player.corridor == null)
        {
            if (player.room.Enemies.Count > 0)
            {
                player.room.CloseDoors();
            }
            if (player.room.hereWasPlayer == false)
            {
                player.room.hereWasPlayer = true;
                player.gameStatistic.RoomsCount += 1;
            }

        }
    }

}
