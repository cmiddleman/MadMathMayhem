using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int lane;
    private float jumpSpeed;
    private bool jumpingUp;
    private bool jumpingDown;
    public bool dead { get; private set; }
    private bool thisTouchMoved;
    public float recoverTime { get; private set; }
    public float timeSinceLastBlink;
    public float blinkTime;
    private SpriteRenderer sprite;

    private Vector3 targetPos;
    private float jumpHeight;
    public float touchDownY;
    private float worldHeight;
   
    public Animator animator;

    private int health;

    private int streak;
    private int streakMultiplier;
    public GameObject[] lives;
    public bool recovering { get; private set; }
    private float recoverTimeSoFar;

    public Score score;
    [SerializeField] private TextMeshProUGUI streakText;
    public GameObject pauseButton;

    public TextMeshProUGUI coinText;

    float[] laneYPos;

    // how to make BUFFER const?
    private float TOUCH_BUFFER;
    
    //lanes indexed from top to bottom
    private const int MIN_LANE = 0;
    private const int MAX_LANE = Values.NUM_LANES - 1;
    private const int START_LANE = 1;
    private const float JUMP_TIME = .2f;
    private const int MIN_STREAK = 3;
    private const int MAX_STREAK = 10;




    private void Awake()
    {
        laneYPos = new float[4];
    }

    // Start is called before the first frame update
    void Start()
    {
        health = Values.MAX_HEALTH;
        streak = 0;
        
        targetPos = transform.position;
        lane = START_LANE;
        jumpingUp = false;
        jumpingDown = false;
        dead = false;
        thisTouchMoved = false;
        recoverTime = 1f;
        recoverTimeSoFar = 0f;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        score = gameObject.GetComponent<Score>();
        streakMultiplier = score.ComputeStreakMultiplier();
        timeSinceLastBlink = 0f;
        blinkTime = 3f;
        coinText.text = "x" + Settings.Instance.coinCount.ToString();

        worldHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        jumpHeight = 2*worldHeight / 5;
        TOUCH_BUFFER = Screen.height / 12;
        jumpSpeed = jumpHeight / JUMP_TIME;
        //   animator.SetFloat("jumpRate", 1/JUMP_TIME);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            animator.SetBool("jumpingUp", jumpingUp);
            animator.SetBool("jumpingDown", jumpingDown);

            //hangle touch events
            CheckTouch();
           
            

            if (recovering)
            {
                float recoverFrac = recoverTimeSoFar / recoverTime;
                sprite.color = new Color(recoverFrac, 1, 1, 1);
                recoverTimeSoFar += Time.deltaTime;
                if (recoverTimeSoFar > recoverTime)
                {
                    sprite.color = Color.white;
                    recovering = false;
                    recoverTimeSoFar = 0f;
                }
            }

            timeSinceLastBlink += Time.deltaTime;
            if (timeSinceLastBlink > blinkTime)
            {
                Blink();
            }

            //TODO: remove later, for testing on comp.
            TEST();


            //reposition if jumping
            if (jumpingUp || jumpingDown)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, jumpSpeed * Time.deltaTime);
                if (transform.position == targetPos)
                {

                    jumpingUp = false;
                    jumpingDown = false;
                }

                Jump(Time.deltaTime);
            }
        }   
    }

    void CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //If just touches set reference y
            if (touch.phase == TouchPhase.Began)
            {
                touchDownY = touch.position.y;
            }
            else if (touch.phase == TouchPhase.Moved && !thisTouchMoved)
            {
                HandleTouchMove(touch.position.y);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                thisTouchMoved = false;
            }
        }
    }

     void HandleTouchMove(float currTouchY)
    {
        //Player cannot jump if touch is released while jumping
       
        //should player move up or down, if so call it
        if(touchDownY + TOUCH_BUFFER < currTouchY && lane > MIN_LANE)
        {
            MoveUp();
            thisTouchMoved = true;
        }
        else if(touchDownY > TOUCH_BUFFER + currTouchY && lane < MAX_LANE )
        {
            MoveDown();
            thisTouchMoved = true;
        }
    }

    void Jump(float delta)
    {
        //Impliment code to make player grow and then shrink to give appearance of jumping.
    }

    void Blink()
    {
        timeSinceLastBlink = 0f;
        blinkTime = Random.Range(3f, 10f);
        animator.ResetTrigger("blink");
        animator.SetTrigger("blink");
    
        
    }

    public void IncreaseStreak()
    {
        streak++;
        if(streak >= MIN_STREAK)
        {
            streakText.gameObject.SetActive(true);
            streakText.text = "Streak x" + streak.ToString() + "!!!";
            StartCoroutine(AnimateStreak());
        }   
    }

    IEnumerator AnimateStreak()
    {
        float animTime = streakText.gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
        yield return new WaitForSeconds(animTime);
        streakText.text = "";
        score.AddScore(streakMultiplier * Mathf.Min(MAX_STREAK, streak - MIN_STREAK + 1));
        streakText.gameObject.SetActive(false);
    }
    public void ResetStreak()
    {
        streak = 0;
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (!recovering)
        {
            if (enemy.tag == "Paper")
            {
                TakeDamage();
                recovering = true;
                AudioManager.Instance.Play("PlayerHurt");
            }
            else if (enemy.tag == "Pencil")
            {
                TakeDamage();
                recovering = true;
                AudioManager.Instance.Play("PlayerHurt");
            }
            else if (enemy.tag == "Pen")
            {
                TakeDamage();
                if(Settings.Instance.mathDifficulty != Settings.EASY)
                    TakeDamage();
                recovering = true;
                AudioManager.Instance.Play("PlayerHurt");
            }
        }
      
        
        if(enemy.tag == "Health")
        {
            Heal();
            AudioManager.Instance.Play("PowerUp");
        }
        else if(enemy.tag == "Coin")
        {
            Settings.Instance.AdjustCoinCount(1);
            coinText.text = "x" + Settings.Instance.coinCount;
            AudioManager.Instance.Play("Coin");
        }
        Destroy(enemy);
    }

    //Sets player's target location to start moving them there, also plays the jump noise.
    void Move()
    {
        targetPos.y = laneYPos[lane];
        AudioManager.Instance.Play("Jump");
    }

    public void TakeDamage()
    {
        health--;
        if(health > -1)
            lives[health].SetActive(false);
        if (health == 0)
            Die();
    }

    public void Heal()
    {
        if(health < Values.MAX_HEALTH)
        {
            lives[health].SetActive(true);
            health++;
        }
    }
    public void Die()
    {
        dead = true;
        AudioManager.Instance.StopAll();
        pauseButton.SetActive(false);
        jumpingDown = false;
        jumpingUp = false;
        score.running = false;
        Settings.Instance.UpdateHighScores((int)score.score);
        animator.SetTrigger("dead");
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.angularVelocity = 720;
        rb.velocity = new Vector2(-4, 6);

    }

    private void OnBecameInvisible()
    {
        if (dead)
        {
            Settings.Instance.Save();
            SceneManager.LoadScene("GameOver");
        }
    }

    //called on mouse or touch release
   

    public void SetLaneY(float lane1, float lane2, float lane3, float lane4)
    {
        laneYPos[0] = lane1;
        laneYPos[1] = lane2;
        laneYPos[2] = lane3;
        laneYPos[3] = lane4;
    }

    void MoveUp()
    {
        jumpingUp = true;
        lane--;
        Move();
    }
    void MoveDown()
    {
        jumpingDown = true;
        lane++;
        Move();
    }

    // TEMP code for moving with arrow keys.
    void TEST()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S)) && !(jumpingUp || jumpingDown) && lane < MAX_LANE)
        {
            MoveDown();
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.W)) && !(jumpingUp || jumpingDown) && lane > MIN_LANE)
        {
            MoveUp();
        }
    }
 

}
