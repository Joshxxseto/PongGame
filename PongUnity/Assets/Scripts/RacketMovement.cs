using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketMovement : MonoBehaviour
{
    public float speed=30f;
    public string myAxis="Vertical";
    private float direction;
    private Vector2 startPos;

    private void Start()
    {
        startPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.gameStarted && !GameManager.sharedInstance.gamePaused)
        {
            //While we can play, we will move the racket
            direction = Input.GetAxis(myAxis);
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed * direction);
        }
        else
        {
            //As long as the game hasn't started or is paused the rackets will not move
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    public void restartPos()
    {
        this.transform.position = startPos;
    }
}
