﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextReady : MonoBehaviour {

    public float shrinkSpeed;
    public SoundEffects soundEffects;

    internal int cycle = 3;
    private float currentSecondsToBeat;
    private bool beeped;
    private float y;
    private Text readyText;
    private AudioManager audioManager;

	void Start () {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        currentSecondsToBeat = 0;
        readyText = GetComponent<Text>();
        transform.localScale = new Vector3(1, 0, 1);
        y = 1;
	}
	
	void Update () {
       
        if (cycle >= 0)
        {
            if (audioManager.timeTillSongStart - audioManager.songTime < audioManager.secondsToBeat * cycle && !beeped)
            {
                if (cycle > 0)
                {
                    transform.localScale = Vector3.one;
                    y = 1;
                    readyText.text = cycle.ToString();
                    soundEffects.Beep();
                }
                else
                    readyText.text = "";
                beeped = true;
                cycle--;
                cycle = Mathf.Clamp(cycle, -1, 3);
            }
            else if (beeped)
            {
                currentSecondsToBeat += Time.deltaTime;
                if (currentSecondsToBeat > audioManager.secondsToBeat)
                {
                    currentSecondsToBeat = 0;
                    beeped = false;
                }
            }
        }

        if (y > 0)
            y -= Time.deltaTime * shrinkSpeed;
        else
            y = 0;

        if (y < 0.5f)
        {
            transform.localScale = new Vector3(1, y * 2, 1);
        }
    }

    internal void Reset()
    {
        cycle = 3;
        currentSecondsToBeat = 0;
    }
}
