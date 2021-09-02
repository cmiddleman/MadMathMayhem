using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//single
public class Values : MonoBehaviour
{
    // lanes indexed starting at 0 so there are LANE_NUMBER + 1 total lanes.
    public const int NUM_LANES = 4;

    public const int ADD = 0;
    public const int SUB = 1;
    public const int MUL = 2;
    public const int DIV = 3;
    public const float HIGHLIGHT_TIME = 1.5f;
    public const int NUM_OPS = 4;
    public const int MAX_HEALTH = 3;
    public const float MATH_SPAWN_TIME = 10f;
    public const int COIN_CHANCE = 10;
    public const float PEN_SPEED = 150;
    public const float BASE_SCORE_RATE = 10;
    public const float MUL_BONUS_FACTOR = 1.5f;
    public const float DIV_BONUS_FACTOR = 1.5f;

    public const float MATH_TEXT_ANIM_TIME = 2f;


    public static Values Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private int HEALTH_CHANCE_EASY;
    [SerializeField]
    private int HEALTH_CHANCE_NORMAL;
    [SerializeField]
    private int HEALTH_CHANCE_HARD;

    public int HealthChance()
    {
        switch (Settings.Instance.obstacleDifficulty)
        {
            case Settings.EASY:
                return HEALTH_CHANCE_EASY;
            case Settings.NORMAL:
                return HEALTH_CHANCE_NORMAL;
            case Settings.HARD:
                return HEALTH_CHANCE_HARD;
            default:
                Debug.LogError("invalid difficulty");
                return 0;
        }
    }

    [SerializeField]
    private float BALL_SPAWN_TIME_EASY;
    [SerializeField]
    private float BALL_SPAWN_TIME_NORMAL;
    [SerializeField]
    private float BALL_SPAWN_TIME_HARD;

    public float BallSpawnTime()
    {
        switch (Settings.Instance.obstacleDifficulty)
        {
            case Settings.EASY:
                return BALL_SPAWN_TIME_EASY;
            case Settings.NORMAL:
                return BALL_SPAWN_TIME_NORMAL;
            case Settings.HARD:
                return BALL_SPAWN_TIME_HARD;
            default:
                Debug.LogError("invalid difficulty");
                return 0;
        }
    }

    [SerializeField]
    private float BALL_SPEED_EASY;
    [SerializeField]
    private float BALL_SPEED_NORMAL;
    [SerializeField]
    private float BALL_SPEED_HARD;

    public float BallSpeed()
    {
        switch (Settings.Instance.obstacleDifficulty)
        {
            case Settings.EASY:
                return BALL_SPEED_EASY;
            case Settings.NORMAL:
                return BALL_SPEED_NORMAL;
            case Settings.HARD:
                return BALL_SPEED_HARD;
            default:
                Debug.LogError("invalid difficulty");
                return 0;
        }
    }

    [SerializeField]
    private float PENCIL_SPAWN_TIME_EASY;
    [SerializeField]
    private float PENCIL_SPAWN_TIME_NORMAL;
    [SerializeField]
    private float PENCIL_SPAWN_TIME_HARD;

    public float PencilSpawnTime()
    {
        switch (Settings.Instance.obstacleDifficulty)
        {
            case Settings.EASY:
                return PENCIL_SPAWN_TIME_EASY;
            case Settings.NORMAL:
                return PENCIL_SPAWN_TIME_NORMAL;
            case Settings.HARD:
                return PENCIL_SPAWN_TIME_HARD;
            default:
                Debug.LogError("invalid difficulty");
                return 0;
        }
    }

    [SerializeField]
    private float MATH_THINK_TIME_EASY;
    [SerializeField]
    private float MATH_THINK_TIME_NORMAL;
    [SerializeField]
    private float MATH_THINK_TIME_HARD;

    public float MathThinkTime()
    {
        switch (Settings.Instance.mathDifficulty)
        {
            case Settings.EASY:
                return MATH_THINK_TIME_EASY;
            case Settings.NORMAL:
                return MATH_THINK_TIME_NORMAL;
            case Settings.HARD:
                return MATH_THINK_TIME_HARD;
            default:
                Debug.LogError("invalid difficulty");
                return 0;
        }
    }



}
