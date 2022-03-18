using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isTwoPlayer = false;
    [SerializeField] GameObject ballPrefab;
    public static GameManager sharedInstance = null;
    public bool gameStarted=false, gamePaused=false;

    [SerializeField] AudioClip countClip, endClip;

    void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
        StartCoroutine("StartGame",4);
        if (isTwoPlayer)
        {
            //FindEachPlayer
            GameObject playerOne = GameObject.FindGameObjectWithTag("Player1");
            GameObject playerTwo = GameObject.FindGameObjectWithTag("Player2");
            //Give Them their respective axis
            playerOne.GetComponent<RacketMovement>().myAxis = "Vertical1";
            playerTwo.GetComponent<RacketMovement>().myAxis = "Vertical2";
        }
        else
        {
            //FindEachPlayer
            GameObject playerOne = GameObject.FindGameObjectWithTag("Player1");
            GameObject playerTwo = GameObject.FindGameObjectWithTag("Player2");
            //Set the game mode to SinglePlayer
            playerOne.GetComponent<RacketMovement>().myAxis = "Vertical";
            playerTwo.GetComponent<RacketMovement>().enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&gameStarted)
        {
            //We pause the game if any player pressed the spacebar
            gamePaused=!gamePaused;
            Debug.Log("Pause");
        }
    }

    private void StartCountDown(int i)
    {
        if (i<0)
        {
            GameObject.FindGameObjectWithTag("CountDown").GetComponent<Text>().text = "";
        }
        else if (i == 0)
        {
            GameObject.FindGameObjectWithTag("CountDown").GetComponent<Text>().text = "GO";
            StartCoroutine("CountDown", i - 1);
        }
        else
        {
            GameObject.FindGameObjectWithTag("CountDown").GetComponent<Text>().text = i.ToString();
            StartCoroutine("CountDown", i - 1);
        }
    }

    IEnumerator CountDown(int i)
    {
        GetComponent<AudioSource>().clip = countClip;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(1);
        StartCountDown(i);
    }

    IEnumerator StartGame(int i)
    {
        Instantiate(ballPrefab, transform.position, Quaternion.identity);
        StartCountDown(i-1);
        yield return new WaitForSecondsRealtime(i);
        Debug.Log("Start");
        GameObject.FindGameObjectWithTag("Player1").GetComponent<RacketMovement>().restartPos();
        GameObject.FindGameObjectWithTag("Player2").GetComponent<RacketMovement>().restartPos();
        gameStarted = true;
    }


    public void scoreGoal(string sideName)
    {
        //Score the point
        int currentScore = int.Parse(GameObject.Find(sideName).GetComponent<Text>().text) + 1;
        GameObject.Find(sideName).GetComponent<Text>().text = currentScore.ToString();
        //Then instantiate a new ball on the center of the field
        gameStarted = false;
        StartCoroutine("StartGame",2);
    }
}
