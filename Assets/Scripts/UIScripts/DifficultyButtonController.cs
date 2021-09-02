using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyButtonController : MonoBehaviour
{
    public TextMeshProUGUI enemyDifficulty;
    public TextMeshProUGUI mathDifficulty;

    public Toggle mathToggleEasy;
    public Toggle mathToggleNormal;
    public Toggle mathToggleHard;
    public Toggle enemyToggleEasy;
    public Toggle enemyToggleNormal;
    public Toggle enemyToggleHard;

    private void Start()
    {
        int mathDiff = Settings.Instance.mathDifficulty;
        int obsDiff = Settings.Instance.obstacleDifficulty;

        switch (mathDiff)
        {
            case Settings.EASY:
                mathToggleEasy.isOn = true;
                break;
            case Settings.NORMAL:
                mathToggleNormal.isOn = true;
                break;
            case Settings.HARD:
                mathToggleHard.isOn = true;
                break;
        }

        switch (obsDiff)
        {
            case Settings.EASY:
                enemyToggleEasy.isOn = true;
                break;
            case Settings.NORMAL:
                enemyToggleNormal.isOn = true;
                break;
            case Settings.HARD:
                enemyToggleHard.isOn = true;
                break;
            default:
                Debug.Log("Invalid obstacle difficulty ERROR");
                break;
        }

    }
    public void SetMathDifficultyEasy(bool isOn)
    {
        if (isOn)
        {
            mathDifficulty.text = "Easy";
            Settings.Instance.SetMathDifficulty(Settings.EASY);
        }
    }

    public void SetMathDifficultyNormal(bool isOn)
    {
        if (isOn)
        {
            mathDifficulty.text = "Normal";
            Settings.Instance.SetMathDifficulty(Settings.NORMAL);
        }
    }

    public void SetMathDifficultyHard(bool isOn)
    {
        if (isOn)
        {
            mathDifficulty.text = "Hard";
            Settings.Instance.SetMathDifficulty(Settings.HARD);
        }
    }

    public void SetEnemyDifficultyEasy(bool isOn)
    {
        if (isOn)
        {
            enemyDifficulty.text = "Easy";
            Settings.Instance.SetObstacleDifficulty(Settings.EASY);
        }
    }

    public void SetEnemyDifficultyNormal(bool isOn)
    {
        if (isOn)
        {
            enemyDifficulty.text = "Normal";
            Settings.Instance.SetObstacleDifficulty(Settings.NORMAL);
        }
    }

    public void SetEnemyDifficultyHard(bool isOn)
    {
        if (isOn)
        {
            enemyDifficulty.text = "Hard";
            Settings.Instance.SetObstacleDifficulty(Settings.HARD);
        }
    }
}
