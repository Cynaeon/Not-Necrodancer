using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoringSystem : MonoBehaviour {
    
    // Scoring system version: 1.01
    // Latest change: Limited beatstreak bonus to 40
    public float timeBonus;
    public int beatStreakMax;
    public Text scoreText;
    public Text hiscoreText;

    public float totalScore;
    private float timeScore;
    private int level;
    private int playerScore;
    private int beatStreak;
    public string hiscorePrefix;
    private bool scoreSaved;
    private Player playerScript;
    private AudioManager audioManagerScript;

	void Start () {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        audioManagerScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        hiscorePrefix = "";
        hiscorePrefix += SceneManager.GetActiveScene().name + "_hiscore_";
        hiscoreText.text = "";
	}
	
	void Update () {
        if (!audioManagerScript.songStopped)
        {
            level = audioManagerScript.level - 1;
            playerScore = playerScript.score;
            beatStreak = playerScript.beatStreak;
            beatStreak = Mathf.Clamp(beatStreak, 0, beatStreakMax);
            int starPowerMultiplier = 1;
            if (playerScript.starPowerActive)
                starPowerMultiplier = 2;
            timeScore += (Time.deltaTime * timeBonus * (level + 1)) * (beatStreak / 100 + 1) * starPowerMultiplier;
            totalScore = (level * 10 + playerScore + timeScore) * 100;
        }
        else if (!scoreSaved && totalScore > 0)
        {
            CheckHiscores();
            DisplayHiscores();
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

    private void DisplayHiscores()
    {
        for (int i = 1; i < 10; i++)
        {
            int score = PlayerPrefs.GetInt(hiscorePrefix + i);
            hiscoreText.text += score.ToString("000000") + "\n";
        }
    }
}
