using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObstacleController : MonoBehaviour
{
    //as percents
    private int healthChance;
    private int coinChance;

    private float ballSpawnTime;
    private float pencilSpawnTime;
    private float mathSpawnTime;
    private float mathThinkTime;

    public GameObject[] paperBalls;
    public GameObject Pencil;
    public GameObject Pen;
    public GameObject Health;
    public GameObject Coin;
    public PlayerController player;


    public GameObject lane1Spawn;
    public GameObject lane2Spawn;
    public GameObject lane3Spawn;
    public GameObject lane4Spawn;

    public Text mathProbemText;
    public Text[] mathLaneTexts;
    private int correctLane;

    public float startX;

    public bool mathing;
    private bool spawning;
    public float timeSinceLastBall;
    public float timeSinceLastPencil;
    public float timeSinceLastMath;
    public float mathTime;
    private float worldHeight;
    private float laneHeight;
    private int lastPencilLane;
    private int lastBallLane;

    public bool addEnabled;
    public bool subEnabled;
    public bool mulEnabled;
    public bool divEnabled;

    // Start is called before the first frame update
    void Start()
    {
        healthChance = Values.Instance.HealthChance();
        coinChance = Values.COIN_CHANCE;
        ballSpawnTime = Values.Instance.BallSpawnTime();
        pencilSpawnTime = Values.Instance.PencilSpawnTime();
        mathSpawnTime = Values.MATH_SPAWN_TIME;
        mathThinkTime = Values.Instance.MathThinkTime();

        lastPencilLane = -1;
        lastBallLane = -1;

        timeSinceLastBall = 0f;
        timeSinceLastPencil = 0f;
        timeSinceLastMath = 0f;
        mathTime = 0f;
        mathing = false;
        spawning = true;
        worldHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        laneHeight = 2 * worldHeight / 5;

        player.SetLaneY(lane1Spawn.transform.position.y, lane2Spawn.transform.position.y, lane3Spawn.transform.position.y, lane4Spawn.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.dead)
            return;
        if (spawning)
        {
            timeSinceLastBall += Time.deltaTime;
            timeSinceLastPencil += Time.deltaTime;
            timeSinceLastMath += Time.deltaTime;

            

            if(timeSinceLastBall > ballSpawnTime)
            {
                //spawn either ball, bonus score, or health then reset timer.
                timeSinceLastBall = 0;
                int roll = Random.Range(0, 100);
                if (roll < healthChance)
                    SpawnHealth();
                else if (roll < healthChance + coinChance)
                    SpawnCoin();
                else
                    SpawnBall();
            }
               
            
            if(timeSinceLastPencil > pencilSpawnTime) 
                SpawnPencil();

            if (timeSinceLastMath > mathSpawnTime)
                spawning = false;
        }
        else
        {
            if (!mathing && transform.childCount == 0)
                StartMath();
            else if(mathing)
            {
                mathTime += Time.deltaTime;
                if (mathTime > mathThinkTime)
                    EndMath();
            }
            
        }
    }

    void SpawnBall()
    {

        int ballNum = Random.Range(0, paperBalls.Length);

        int laneNum = Random.Range(1, Values.NUM_LANES+1);
        while(laneNum == lastBallLane)
            laneNum = Random.Range(1, Values.NUM_LANES + 1);
        
        lastBallLane = laneNum;

        GameObject ball = Instantiate(paperBalls[ballNum], transform);
        SetLanePos(ball, laneNum);
    }

    void SpawnHealth()
    {
        int laneNum = Random.Range(1, Values.NUM_LANES + 1);

        GameObject health = Instantiate(Health, transform);
        SetLanePos(health, laneNum);
    }

    void SpawnCoin()
    {
        int laneNum = Random.Range(1, Values.NUM_LANES + 1);

        GameObject coin = Instantiate(Coin, transform);
        SetLanePos(coin, laneNum);
    }

    void SpawnPencil()
    {
        timeSinceLastPencil = 0;

        int laneNum = Random.Range(1, Values.NUM_LANES+1);
        while(laneNum == lastPencilLane)
            laneNum = Random.Range(1, Values.NUM_LANES + 1);

        lastPencilLane = laneNum;

        GameObject pencil = Instantiate(Pencil, transform);
        SetLanePos(pencil, laneNum);
    }

    //integer parameter is the lane that is to be *not* hit by the pens
    void SpawnPens(int lane)
    {
        for(int i = 1; i<= Values.NUM_LANES; i++)
        {
            if(i != lane)
            {
                GameObject pen = Instantiate(Pen, transform);
                SetLanePos(pen, i);
            }
        }
        AudioManager.Instance.Stop("TickTock");
        AudioManager.Instance.Play("PenWoosh");
    }


    void ClearText()
    {
        StartCoroutine(ClearMathText());
        for(int i = 1; i <= mathLaneTexts.Length; i++)
        {
            if(i == correctLane)
            {
                mathLaneTexts[i-1].gameObject.GetComponent<LaneTextController>().StartAnim();
            }
            else
            {
                mathLaneTexts[i - 1].text = "";
            }
        }
    }

    IEnumerator ClearMathText()
    {
        yield return new WaitForSeconds(Values.MATH_TEXT_ANIM_TIME);
        mathProbemText.text = "";
    }

    void StartMath()
    {
        AudioManager.Instance.Play("TickTock");

        mathing = true;
        timeSinceLastMath = 0;

        //Determine the two one digit number to be operated, if operation is subtraction or division, num1 and correctAns will be swapped
        int num1 = GetRandomValidNum();
        int num2 = GetRandomValidNum();

        //Determine the operation to be done
        int op = DetermineOperation();

        //Generate answer as well as fake answers, probablly should be done in seperate method but oh vel...
        int correctAns = -3;
        int wrongAns1 = -3;
        int wrongAns2 = -3;
        int wrongAns3 = -3;
        if (op == Values.ADD)
        {
            correctAns = num1 + num2;
          
            wrongAns1 = correctAns + 1;
            wrongAns2 = correctAns - 1;
            // the final incorrect answer will be randomly +- 2 from correct
            wrongAns3 = correctAns + 2 - 4 * Random.Range(0, 2);
        }
        else if(op == Values.SUB)
        {
            correctAns = num1;
            num1 = correctAns + num2;

            wrongAns1 = correctAns + 1;
            wrongAns2 = correctAns - 1;
            wrongAns3 = correctAns + 2 - 4 * Random.Range(0, 2);
        }
        else if (op == Values.MUL)
        {
            correctAns = num1 * num2;

            wrongAns1 = (num1 + 1 - 2 * Random.Range(0, 2)) * num2;
            wrongAns2 = num1 * (num2 + 1 - 2 * Random.Range(0, 2));

            // The issue is this number could be the original number if they are one apart, e.g. 4*5 can become 5*4 which is not good!
            wrongAns3 = correctAns;
            while(wrongAns3 == correctAns)
                wrongAns3 = (num1 + 1 - 2 * Random.Range(0, 2)) * (num2 + 1 - 2 * Random.Range(0, 2));
        }
        else if (op == Values.DIV)
        {
            correctAns = num1;
            num1 = correctAns * num2;

            wrongAns1 = correctAns + 1;
            wrongAns2 = correctAns - 1;
            wrongAns3 = correctAns + 2 - 4 * Random.Range(0, 2);
        }

        mathProbemText.text = num1.ToString() + OpToString(op) + num2.ToString();

        List<int> lanes = new List<int> { 1, 2, 3, 4 };
 
        int laneIndex = Random.Range(0, lanes.Count);
        int currLane = lanes[laneIndex];
        SetLaneText(currLane, correctAns.ToString());
        lanes.RemoveAt(laneIndex);
        correctLane = currLane;

        laneIndex = Random.Range(0, lanes.Count);
        currLane = lanes[laneIndex];
        SetLaneText(currLane, wrongAns1.ToString());
        lanes.RemoveAt(laneIndex);

        laneIndex = Random.Range(0, lanes.Count);
        currLane = lanes[laneIndex];
        SetLaneText(currLane, wrongAns2.ToString());
        lanes.RemoveAt(laneIndex);

        laneIndex = Random.Range(0, lanes.Count);
        currLane = lanes[laneIndex];
        SetLaneText(currLane, wrongAns3.ToString());
        lanes.RemoveAt(laneIndex);
    }

    void EndMath()
    {
        mathTime = 0;
        mathing = false;
        spawning = true;
        SpawnPens(correctLane);
        ClearText();
        StartCoroutine(CheckStreak());
    }

    IEnumerator CheckStreak()
    {
        yield return new WaitForSeconds(.5f);
        if (player.recovering)
            player.ResetStreak();
        else
            player.IncreaseStreak();
    }

    int GetRandomValidNum()
    {
        int num = -1;
        while (true)
        {
            num = Random.Range(1, Settings.MAX_NUM + 1);
            if (Settings.Instance.IsNumValid(num))
                break;
        }
        return num;
    }

    //Randomly picks an operation until it finds a valid operation
    int DetermineOperation()
    {
        int op = -1;
        while (true)
        {
            op = Random.Range(0, Values.NUM_OPS);
            if (IsValidOperation(op))
                break;
        } 

        return op;
    }

    bool IsValidOperation(int op)
    {
        if (op == Values.ADD)
            return Settings.Instance.IsOpValid(Values.ADD);
        else if (op == Values.SUB)
            return Settings.Instance.IsOpValid(Values.SUB);
        else if (op == Values.MUL)
            return Settings.Instance.IsOpValid(Values.MUL);
        else if (op == Values.DIV)
            return Settings.Instance.IsOpValid(Values.DIV);
        else
            throw new System.Exception("Operation number invalid");
    }

    string OpToString(int op)
    {
        string opText = "";
        if (op == Values.ADD)
            opText = "+";
        else if (op == Values.SUB)
            opText = "-";
        else if (op == Values.MUL)
            opText = "x";
        else if (op == Values.DIV)
            opText = "\u00F7";
        else
            throw new System.Exception("Operation number invalid");

        return opText;
    }

    void SetLaneText(int lane, string text)
    {
        mathLaneTexts[lane - 1].text = text;
    }

    void SetLanePos(GameObject go, int lane)
    {
        switch (lane)
        {
            case 1:
                go.transform.position = lane1Spawn.transform.position;
                break;
            case 2:
                go.transform.position = lane2Spawn.transform.position;
                break;
            case 3:
                go.transform.position = lane3Spawn.transform.position;
                break;
            case 4:
                go.transform.position = lane4Spawn.transform.position;
                break;
            default:
                throw new System.Exception("Invalid lane number");
        }
    }

}
