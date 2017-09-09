using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour {
    
    // Scoring system version: 1.00
    public float timeBonus;
    public Text scoreText;

    public float totalScore;
    private float timeScore;
    private int level;
    private int playerScore;
    private int beatStreak;
    private Player playerScript;
    private AudioManager audioManagerScript;

	void Start () {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        audioManagerScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();
	}
	
	void Update () {
        if (!audioManagerScript.songStopped)
        {
            level = audioManagerScript.level - 1;
            playerScore = playerScript.score;
            beatStreak = playerScript.beatStreak;
            timeScore += (Time.deltaTime * timeBonus * (level + 1)) * (beatStreak / 100 + 1);

            totalScore = (level * 10 + playerScore + timeScore) * 100;
        }
        scoreText.text = totalScore.ToString("000000");
	}
}
