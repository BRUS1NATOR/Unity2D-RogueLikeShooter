using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Body : Body
{
    // Update is called once per frame
    public AudioSource audioSource;
    private bool playing=false;
    private void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            if (!animPlaying && parent.isAlive && parent.movement.dontMove == false)
            {
                moveHorizontal = Input.GetAxisRaw("Horizontal");
                moveVertical = Input.GetAxisRaw("Vertical");

                parent.animator.SetFloat("RunVertical", Mathf.Abs(moveVertical));
                parent.animator.SetFloat("RunHorizontal", Mathf.Abs(moveHorizontal));

                if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
                {
                    if (playing == false)
                    {
                        playing = true;
                        audioSource.Play();
                    }
                }

                else
                {
                    playing = false;
                    audioSource.Stop();
                }

                Vector2 move = new Vector2(moveHorizontal, moveVertical);
                parent.movement.rb.AddForce(move * 10 * parent.stats.speed * Time.timeScale);
            }
        }
    }
}
