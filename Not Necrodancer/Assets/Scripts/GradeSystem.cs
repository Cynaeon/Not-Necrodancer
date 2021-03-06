﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradeSystem : MonoBehaviour {

    public AudioManager audioManager;

    public float pointsForStar;
    public float score;
    public float grade;

    private float maxScore;

	void Start () {
		
	}
	
	void Update () {
        if (!audioManager.songStopped)
        {
            pointsForStar = audioManager.scoreToFiveStars / 5;
            score += Time.deltaTime / 4;
            grade = Mathf.Floor(score / pointsForStar);
        }
	}
}
