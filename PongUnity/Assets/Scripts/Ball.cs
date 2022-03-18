using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public float speed = 20;
    private bool isMoving=false;
    Vector2 currectDirection;

    void Start()
    {
        int startingDirection = Random.Range(0, 2);
        //Here it is determined randomly where does the ball is gonna go first
        if (startingDirection > 0)
        {
            currectDirection = Vector2.right * speed;
        }
        else
        {
            currectDirection = Vector2.left * speed;
        }
    }

    private void Update()
    {
        //Here are 2 cases
        //1: When the game just started and we need to move the ball to play
        //2: When we play the game after we've paused it before.
        if (GameManager.sharedInstance.gameStarted && !GameManager.sharedInstance.gamePaused && !isMoving)
        {
            //We move the ball
            this.GetComponent<Rigidbody2D>().velocity = currectDirection;
            isMoving = true;
        }
        //Then Here is just 1 case
        //We just have paused the game
        else if(GameManager.sharedInstance.gameStarted&&GameManager.sharedInstance.gamePaused&&isMoving)
        {
            //Save the direction
            currectDirection=this.GetComponent<Rigidbody2D>().velocity;
            //Freeze the ball
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            //Changr BoleanFlag
            isMoving = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag=="Player1"|| other.gameObject.tag == "Player2")
        {
            GetComponent<AudioSource>().Play();
            //We calculate the hitFactor
            float y = hitFactor(this.transform.position,other.transform.position,other.collider.bounds.size.y);
            //We calculate the last direction (X) to repeat It later
            float x = GetComponent<Rigidbody2D>().velocity.x / speed;
            Vector2 newDirection = new Vector2(x,y).normalized;
            //Then we apply this new direction to the ball with the selected speed
            this.GetComponent<Rigidbody2D>().velocity = newDirection*speed;
        }else if (other.gameObject.tag=="Goal")
        {
            //Lest see wich goal does the ball hit
            if (this.transform.position.x > 0)//right goal
            {
                //Point to the player 1
                GameManager.sharedInstance.scoreGoal("Score1");
            }
            else if(this.transform.position.x<0)//left goal
            {
                //Point to the player 2
                GameManager.sharedInstance.scoreGoal("Score2");
            }
            Destroy(gameObject);
        }
    }

    private float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        //The hitFactor will be a float Number that will be positive or negative depending on where does de ball has hit
        //if it was to low it will be negative and viceversa if it was to high, later on we will normalize this number
        //in order to give the ball a new direction (Y) that can be + / -  / 0
        return (ballPos.y-racketPos.y)/racketHeight;
    }
}
