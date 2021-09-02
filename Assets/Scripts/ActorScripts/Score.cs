using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    private float scoreRate;
    public bool running { get; set; }

    private int streakMultiplier;
    private int obstacleDifficultyMultiplier;

    private const int TOP_NUMS_COUNT = 6;

    public TextMeshProUGUI scoreText;
    public float score { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        running = true;
        score = 0;
        scoreRate = Values.BASE_SCORE_RATE;
        streakMultiplier = ComputeStreakMultiplier();
        obstacleDifficultyMultiplier = Settings.Instance.obstacleDifficulty + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            score += scoreRate * obstacleDifficultyMultiplier * Time.deltaTime;
            UpdateScoreText();
        }
    }

    //Sum of highest 6 * math difficulty
    public int ComputeStreakMultiplier()
    {
        float multiplier = 0;

        bool[] validNums = Settings.Instance.validNums;
        int count = TOP_NUMS_COUNT;
        int index = validNums.Length - 1;
        while(count > 0 && index >= 0)
        {
            if (validNums[index])
            {
                multiplier += index + 1;
                count--;
            }
            index--;
        }
        multiplier *= Settings.Instance.mathDifficulty + 1;
        if (Settings.Instance.IsOpValid(Values.MUL))
            multiplier *= Values.MUL_BONUS_FACTOR;
        if (Settings.Instance.IsOpValid(Values.DIV))
            multiplier *= Values.DIV_BONUS_FACTOR;

        Debug.Log(multiplier);
        return (int) multiplier;
    }

    public void AddScore(float newScore)
    {
        score += newScore;
    }

    void UpdateScoreText()
    {
        scoreText.text = Util.AddCommas((int) score);
    }

   
}