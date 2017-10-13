using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoringSystem : MonoBehaviour {
    
    // Scoring system version: 1.02
    // Latest change: Star power earns double points
    public float timeBonus;
    public int beatStreakMax;
    public Text scoreText;
    public Text hiscoreText;
    public float grade;
    public Image[] stars;

    public float totalScore;
    private float timeScore;
    private int level;
    private int playerScore;
    private int beatStreak;
    private float pointsForStar;
    public string hiscorePrefix;
    private bool scoreSaved;
    private Player playerScript;
    private AudioManager _am;

	void Start () {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        _am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
	}

    private void OnEnable()
    {
        GameObject songData = GameObject.FindGameObjectWithTag("SongData");
        hiscorePrefix = "";
        hiscorePrefix += songData.name + "_hiscore_";
        hiscoreText.text = "";
    }

    void Update () {
        if (!_am.songStopped)
        {
            level = _am.level;
            playerScore = playerScript.score;
            beatStreak = playerScript.beatStreak;
            beatStreak = Mathf.Clamp(beatStreak, 0, beatStreakMax);
            int starPowerMultiplier = 1;
            pointsForStar = _am.scoreToFiveStars / 5;
            if (playerScript.starPowerActive)
                starPowerMultiplier = 2;
            timeScore += (Time.deltaTime * timeBonus * (level * 1.4f)) * (beatStreak / 100 + 1) * starPowerMultiplier;
            totalScore = (level * 10 + playerScore + timeScore) * 100;
        }
        else if (!scoreSaved && totalScore > 0)
        {
            CheckHiscores();
            //DisplayHiscores();
        }
        grade = totalScore / pointsForStar;
        for (int i = 1; i < 5; i++)
        {
            if (grade >= i)
                stars[i - 1].enabled = true;
            else
                stars[i - 1].enabled = false;
        }
        scoreText.text = totalScore.ToString("000000");
	}

    private void CheckHiscores()
    {
        for (int i = 1; i < 10; i++)
        {
            int hiscore = PlayerPrefs.GetInt(hiscorePrefix + i.ToString());
            if (totalScore > hiscore)
            {
                for (int j = 10; j > i; j--) 
                {
                    PlayerPrefs.SetInt(hiscorePrefix + j.ToString(), PlayerPrefs.GetInt(hiscorePrefix + (j-1).ToString()));
                }
                PlayerPrefs.SetInt(hiscorePrefix + i.ToString(), (int)totalScore);
                break;
            }
        }
        PlayerPrefs.Save();
        scoreSaved = true;
    }

    public void ResetScore()
    {
        totalScore = 0;
        timeScore = 0;
        level = 0;
        playerScore = 0;
        beatStreak = 0;
        scoreSaved = false;
        hiscoreText.text = "";
    }

    private void DisplayHiscores()
    {
        for (int i = 1; i < 10; i++)
        {
            int score = PlayerPrefs.GetInt(hiscorePrefix + i.ToString());
            hiscoreText.text += i + ". " + score.ToString("000000") + "\n";
        }
    }
}
