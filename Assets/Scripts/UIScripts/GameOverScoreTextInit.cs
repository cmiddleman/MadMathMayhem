using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScoreTextInit : MonoBehaviour
{
    public TextMeshProUGUI highScore1;
    public TextMeshProUGUI highScore2;
    public TextMeshProUGUI highScore3;
    public TextMeshProUGUI scoreText;

    private bool newHighScore;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = Settings.Instance.currScore;
        newHighScore = Settings.Instance.IsNewHighScore();

        highScore1.text = "#1 - " + Util.AddCommas(Settings.Instance.highScores[0]);
        highScore2.text = "#2 - " + Util.AddCommas(Settings.Instance.highScores[1]);
        highScore3.text = "#3 - " + Util.AddCommas(Settings.Instance.highScores[2]);

        if (newHighScore)
        {
            scoreText.text = "New HighScore!!\n" + Util.AddCommas(score);
        }
        else
        {
            scoreText.text = "Score: " + Util.AddCommas(score);
        }
    }

}
