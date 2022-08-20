using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
    public Transform playerTransform;
    public Object currentScene = null;
    public GameObject redLine;
    public GameObject lastWall;
    private List<GameObject> balls;
    private GameObject score;
    private GameObject levelGameObject;
    private TextMeshPro levelTxt;
    private List<GameObject> USmall;
    private int destroyCounter = 0;
    public int level = 0;
    private bool passedLevel, showRetryOnce = false;
    public GameObject playButton, retryButton;

    void Start()
    {
        GameAnalytics.Initialize();

        levelGameObject = GameObject.FindGameObjectWithTag("Level");
        //Debug.Log("amooooooo");
        //Debug.Log(levelGameObject.transform.GetComponent<TextMeshPro>().text);

        PlayerData data = SaveSystem.LoadState();
        if (data == null)
        {
            FindObjectOfType<SceneManager>().SpawnScene(0);
            FindObjectOfType<SceneManager>().SpawnScene(1);
            FindObjectOfType<SceneManager>().SpawnScene(2);
            level = 1;
        }
        else
        {
            FindObjectOfType<SceneManager>().SpawnScene(data.scene0);
            FindObjectOfType<SceneManager>().SpawnScene(data.scene1);
            FindObjectOfType<SceneManager>().SpawnScene(data.scene2);
            level = data.level;
        }
        Pause();
    }

    private void Pause()
    {
        retryButton.SetActive(false);
        playButton.SetActive(true);
        FindObjectOfType<PlayerController>().stopFlag = true;
    }


    //play button onclick
    public void Play()
    {
        //score = 0;
        //scoreText.text = "Score: " + score.ToString();

        playButton.SetActive(false);
        FindObjectOfType<PlayerController>().stopFlag = false;
    }
    
    public void ShowRetry()
    {
        Debug.Log("Show Retry");
        retryButton.SetActive(true);
        FindObjectOfType<PlayerController>().stopFlag = true;
    }

    //retry button onclick
    public void Retry()
    {
        Debug.Log("Retry");
        retryButton.SetActive(false);
        showRetryOnce = false;
        FindObjectOfType<PlayerController>().ResetPlayer();
        FindObjectOfType<SceneManager>().RestartLevel();
    }

    void Update()
    {
        //scene changed
        if(currentScene == null || 
            (playerTransform.position.z > lastWall.transform.position.z))
        {
            
            currentScene = FindObjectOfType<SceneManager>().GetCurrentScene();
            Transform parent = ((GameObject)currentScene).transform;
            for (int i=0; i<parent.childCount; i++)
            {
                if (parent.GetChild(i).gameObject.CompareTag("FinishLine"))
                {
                    redLine = parent.GetChild(i).gameObject;
                }
                if (parent.GetChild(i).gameObject.CompareTag("LastWall"))
                {
                    lastWall = parent.GetChild(i).gameObject;
                }
                if(parent.GetChild(i).gameObject.CompareTag("Score"))
                {
                    score = parent.GetChild(i).gameObject;
                }
            }

            if (currentScene != null)
            {

                //color U
                balls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ball"));
                for (int i = 0; i < balls.Count; i++)
                {
                    if (balls[i].transform.position.z < lastWall.transform.position.z &&
                        balls[i].transform.position.z > lastWall.transform.position.z - 50)
                    {
                        USmall = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerChild"));
                        Color UColor = balls[i].GetComponent<Renderer>().material.GetColor("_Color");
                        // give color to U
                        USmall[0].GetComponent<Renderer>().material.color = UColor;
                        USmall[1].GetComponent<Renderer>().material.color = UColor;
                        USmall[2].GetComponent<Renderer>().material.color = UColor;
                        break;
                    }
                }
                
            }
            //save game state
            Queue<int> sceneInd = FindObjectOfType<SceneManager>().shownScenesIndex;
            int[] sceneIndArr = (int[])sceneInd.ToArray();
            SaveSystem.SaveState(level, sceneIndArr);
        }

        //just passed red line
        if (FindObjectOfType<PlayerController>().stopFlag == false && 
            (playerTransform.position.z - 1.5) >= redLine.transform.position.z &&
            lastWall.transform.position.y == 2)
        {
            //player won't move
            FindObjectOfType<PlayerController>().stopFlag = true;

            //count all balls with the same color
            balls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ball"));
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].transform.position.z > lastWall.transform.position.z)
                {
                    balls.Remove(balls[i]);
                    i--;
                }
            }
        }
        //player stopped between red line and last wall
        if(FindObjectOfType<PlayerController>().stopFlag == true &&
            (playerTransform.position.z - 1.5) >= redLine.transform.position.z)
        { 
            //count enemy balls (between red line and last wall)
            List<GameObject> enemyBalls = new(GameObject.FindGameObjectsWithTag("EnemyBall"));
            List<GameObject> ballsInside = new(GameObject.FindGameObjectsWithTag("Ball"));
            for (int i = 0; i < enemyBalls.Count; i++)
            {
                if (!((enemyBalls[i].transform.position.z > redLine.transform.position.z) && 
                    (enemyBalls[i].transform.position.z < lastWall.transform.position.z)))
                {
                    enemyBalls.Remove(enemyBalls[i]);
                    i--;
                }
            }
            for (int i = 0; i < ballsInside.Count; i++)
            {
                if (!((ballsInside[i].transform.position.z > redLine.transform.position.z) &&
                    (ballsInside[i].transform.position.z < lastWall.transform.position.z)))
                {
                    ballsInside.Remove(ballsInside[i]);
                    i--;
                }
            }

            
            ///popping balls + calc score
            TextMeshPro scoreTxt = score.transform.GetComponent<TextMeshPro>();
            if (enemyBalls.Count > 0 && ballsInside.Count > 0)
            {

                if (destroyCounter == 0)
                {
                    Destroy(enemyBalls[0]);
                    Destroy(ballsInside[0]);
                    destroyCounter = 20;

                    scoreTxt.text = (ballsInside.Count - 1).ToString() + "/" + (balls.Count / 2).ToString();
                    
                }
                else
                    destroyCounter--;
            }
            else
            {
                scoreTxt.text = ballsInside.Count.ToString() + "/" + (balls.Count / 2).ToString();
                if (ballsInside.Count - enemyBalls.Count >= balls.Count / 2)
                    passedLevel = true;

                if (passedLevel)
                {
                    //red line goes up
                    if (redLine.transform.position.y < 0)
                        redLine.transform.position += Vector3.up * Time.deltaTime;

                    //last wall goes down
                    if (lastWall.transform.position.y > -2.49)
                        lastWall.transform.position += Vector3.down * Time.deltaTime;
                    else
                    {
                        Debug.Log("hiiii destroy");
                        FindObjectOfType<PlayerController>().stopFlag = false;
                        passedLevel = false;
                        Destroy(score);
                        for (int i = 0; i < enemyBalls.Count; i++)
                        {
                            Destroy(enemyBalls[i]);
                        }
                        for (int i = 0; i < ballsInside.Count; i++)
                        {
                            Destroy(ballsInside[i]);
                        }
                        //update level
                        level++;
                        levelTxt = levelGameObject.transform.GetComponent<TextMeshPro>();
                        //###################################################################
                        levelTxt.text = "hi";
                        Debug.Log(levelTxt.text);
                        

                    }

                }
                else if(!showRetryOnce)
                {
                    ShowRetry();
                    showRetryOnce = true;
                }
            }
            
            //paint the last wall

        }

    }

}
