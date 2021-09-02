using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Settings : MonoBehaviour
{
    public const int EASY = 0;
    public const int NORMAL = 1;
    public const int HARD = 2;
    public const int MAX_NUM = 12;
    public const int NUM_OPS = 4;
    public const int NUM_HIGH_SCORES = 3;
    private const int DEFAULT_MAX_NUM = 10;

    public string numsSetName;
    public bool[] validNums { get; private set; }

    public string opsSetName;
    public bool[] validOps { get; private set; }

    public int[] highScores { get; private set; }
    public void SetHighScores(int[] highScores)
    {
        this.highScores = highScores;
    }

    private bool newHighScore;
    public int currScore { get; private set; }
    public int obstacleDifficulty { get; private set; }
    public void SetObstacleDifficulty(int diff)
    {
        obstacleDifficulty = diff;
        PlayerPrefs.SetInt("obsDiff", diff);
    }
    public int mathDifficulty { get; private set; }
    public void SetMathDifficulty(int diff)
    {
        mathDifficulty = diff;
        PlayerPrefs.SetInt("mathDiff", diff);
    }

    //num is between 1 and 12
    public void SetNum(int num, bool isValid)
    {
        validNums[num - 1] = isValid;
        PlayerPrefsSetBool("num" + num.ToString(), isValid);
    }

    //input is between 1 and 12
    public bool IsNumValid(int num)
    {
        return validNums[num - 1];
    }

    //returns the total number of valid numbers to select from, must be at least 3 (according to the settings...)
    public int ValidNumsCount()
    {
        int count = 0;
        for (int i = 0; i < validNums.Length; i++)
            if (validNums[i])
                count++;

        return count;
    }

    public void SetOp(int op, bool isValid)
    {
        validOps[op] = isValid;
        PlayerPrefsSetBool("op" + op.ToString(), isValid);
    }
    public bool IsOpValid(int op)
    {
        return validOps[op];
    }
    public int ValidOpCount()
    {
        int count = 0;
        for (int i = 0; i < validOps.Length; i++)
            if (validOps[i])
                count++;

        return count;
    }
    //Volumes are on a scale of .0001 (-80db) to 1 (0db)
    public float musicVolume { get; private set; }
    public void SetMusicVolume(float musicVolume)
    {
        this.musicVolume = musicVolume;
        PlayerPrefs.SetFloat("musicVol", musicVolume);
    }
    public float soundFXVolume { get; private set; }
    public void SetFXVolume(float FXVolume)
    {
        this.soundFXVolume = FXVolume;
        PlayerPrefs.SetFloat("fxVol", FXVolume);
    }
    public bool muted { get; private set; }
    public void SetMute(bool muted)
    {
        this.muted = muted;
        PlayerPrefsSetBool("muted", muted);

    }
    public int coinCount { get; private set; }
    public void AdjustCoinCount(int value)
    {
        coinCount += value;
        PlayerPrefs.SetInt("coins", coinCount);
    }

    public static Settings Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadPrefs();
    }

    private void Start()
    {
        newHighScore = false;
    }

    void LoadPrefs()
    {
        bool[] defaultNums = new bool[MAX_NUM];
        for (int i = 0; i < DEFAULT_MAX_NUM - 1; i++)
            defaultNums[i] = PlayerPrefsGetBool("num" + (i+1).ToString(), true);
        for(int i = DEFAULT_MAX_NUM - 1; i < MAX_NUM; i++)
            defaultNums[i] = PlayerPrefsGetBool("num" + (i + 1).ToString(), false);

        bool[] defaultOps = new bool[NUM_OPS];
        for (int i = 0; i < NUM_OPS; i++) {
            if (i != Values.MUL)
                defaultOps[i] = (PlayerPrefs.GetInt("op" + i.ToString(), 0) == 1);
            else
                defaultOps[i] = (PlayerPrefs.GetInt("op" + i.ToString(), 1) == 1);
        }


        validNums = defaultNums;
        validOps = defaultOps;

        obstacleDifficulty = PlayerPrefs.GetInt("obsDiff", NORMAL);
        mathDifficulty = PlayerPrefs.GetInt("mathDiff", NORMAL);

        musicVolume = PlayerPrefs.GetFloat("musicVol", 1);
        soundFXVolume = PlayerPrefs.GetFloat("fxVol", 1);
        muted = PlayerPrefs.GetInt("muted", 0) == 1;

        highScores = new int[NUM_HIGH_SCORES];
        for (int i = 0; i < highScores.Length; i++)
            highScores[i] = PlayerPrefs.GetInt("hs" + (i + 1).ToString(), 0);

        coinCount = PlayerPrefs.GetInt("coins", 0);
       
    }

    void PlayerPrefsSetBool(string name, bool value)
    {
        if (value)
            PlayerPrefs.SetInt(name, 1);
        else
            PlayerPrefs.SetInt(name, 0);
    }
    bool PlayerPrefsGetBool(string name, bool defaultValue)
    {
        int def = 0;
        if (defaultValue)
            def = 1;

        return (PlayerPrefs.GetInt(name, def) == 1);
    }

    public void Save()
    {
        PlayerPrefs.Save();
        Debug.Log("saved");
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        LoadPrefs();
    }

    public void UpdateHighScores(int newScore)
    {
        currScore = newScore;
        int[] newScores = new int[highScores.Length];
        int j = 0;
        for(int i = 0; i < highScores.Length; i++)
        {
            if(!newHighScore && newScore > highScores[i])
            {
                newScores[i] = newScore;
                newHighScore = true;
            }
            else
            {
                newScores[i] = highScores[j];
                j++;
            }
        }

        highScores = newScores;

        if(newHighScore)
            for (int i = 0; i < highScores.Length; i++)
            {
            
            PlayerPrefs.SetInt("hs" + (i + 1).ToString(), highScores[i]);
            }
        Save();
    }

    //Checks if new score is high score and returns it after resetting the bool in Settings.
    public bool IsNewHighScore()
    {
        bool newHighScore = this.newHighScore;
        this.newHighScore = false;
        return newHighScore;
    }

    
    //Needs to be redone to be cleaner and handle ToggleSets better.
    public void UpdateToggleSetValues(ToggleSet togSet)
    {
        bool[] values = togSet.GetValues();
        string setName = togSet.setName;

        if (Util.CaseInsensitiveStringCompare(setName, numsSetName)) 
        {
            if (validNums.Length != values.Length)
                Debug.LogWarning("wrong size of numsToggles values array with setName " + setName);
            validNums = values;
            for(int i = 0; i < validNums.Length; i++)
            {
                SetNum(i+1, validNums[i]);
            }
            
        }
        else if (Util.CaseInsensitiveStringCompare(setName, opsSetName))
        {
            if (validOps.Length != values.Length)
                Debug.LogWarning("wrong size of opsToggles values array with setName " + setName);
            validOps = values;
            for (int i = 0; i < validOps.Length; i++)
            {
                SetOp(i, validOps[i]);
            }
        }
        else
        {
            Debug.LogWarning("setName " + setName + " does not exist in settings");
        }
    }

    public void LoadToggleSetValues(ToggleSet togSet)
    {
        string setName = togSet.setName;
        if (Util.CaseInsensitiveStringCompare(setName, numsSetName))
        {
            togSet.LoadValues(validNums);
        }
        else if (Util.CaseInsensitiveStringCompare(setName, opsSetName))
        {
            togSet.LoadValues(validOps);
        }
        else
        {
            Debug.LogWarning("setName " + setName + " does not exist in settings");
        }
    }
}
