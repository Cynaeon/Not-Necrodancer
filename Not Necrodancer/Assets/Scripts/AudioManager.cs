using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public AudioSource song;
    public AudioSource[] songLayer1;
    public AudioSource[] songLayer2;
    public AudioSource[] songLayer3;
    public AudioSource[] songLayer4;
    public AudioSource[] songLayer5;
    public AudioSource soundEffects;
    public AudioClip sound_LevelUp;
    public float timeTillSongStart;
    public GameObject player;
    
    public CameraManager cameraManager;
    public GameObject waveTrigger;
    public GameObject descentTrigger;
    public Transform tempoSphere;
    public ParticleSystem levelUpEffect;
    public float secondsToBeat;
    public float songEndTime;
    public float movementWindow;
    public float tempoOffset;
    public float shrinkSpeed;

    [HideInInspector] public bool songStopped;
    private bool songStarted;
    private Player playerScript;
    private PlayArea playAreaScript;
    [HideInInspector] public int level = 1;
    private GameObject levelUpSphere;
    private Vector3 sphereStartScale;
    private Color sphereStartColor;
    private Color spherePositiveColor;
    private float current;
    private int beatNumber;

	void Start () {
        foreach (AudioSource track in songLayer1)
            track.mute = true;
        foreach (AudioSource track in songLayer2)
            track.mute = true;
        foreach (AudioSource track in songLayer3)
            track.mute = true;
        foreach (AudioSource track in songLayer4)
            track.mute = true;
        foreach (AudioSource track in songLayer5)
            track.mute = true;
        soundEffects = GetComponent<AudioSource>();
        sphereStartScale = tempoSphere.localScale;
        sphereStartColor = tempoSphere.GetComponent<Renderer>().material.color;
        spherePositiveColor = Color.green;
        playerScript = player.GetComponent<Player>();
        playAreaScript = GameObject.Find("PlayArea").GetComponent<PlayArea>();
        songStopped = true;
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.T))
            IncreaseLevel();

        if (Time.time > timeTillSongStart && songStopped && !songStarted)
            StartSong();
        
        if (songLayer1[0].time > songEndTime && songStopped == false)
        {
            playAreaScript.StopSpawning();
            songStopped = true;
        }

        if (Input.GetButtonDown("Descent"))
        {
            Vector3 pos = new Vector3(player.transform.position.x, 14, player.transform.position.z);
            Instantiate(descentTrigger, pos, Quaternion.identity);
        }

        if (playerScript.score >= 10)
        {
            if (!GameObject.Find("LevelUp(Clone)"))
            {
                playAreaScript.SpawnLevelUp();
            }
        }

        if (playerScript.score < 0)
        {
            DecreaseLevel();
            playerScript.score += 10;
        }

        if (tempoSphere.localScale.x > sphereStartScale.x)
        {
            float value = shrinkSpeed * Time.deltaTime;
            tempoSphere.localScale -= new Vector3(value, value, value);
        }

        if (songStarted)
        {
            float beatTime = (songLayer1[0].time / secondsToBeat) - tempoOffset;

            if (beatTime > beatNumber)
            {
                beatNumber++;
                OnBeat();
            }

            float closestBeat = Mathf.Round(beatTime);

            if (Math.Abs(closestBeat - beatTime) < movementWindow)
            {
                player.GetComponent<Player>().canMove = true;
                tempoSphere.GetComponent<Renderer>().material.color = spherePositiveColor;
            }
            else
            {
                tempoSphere.GetComponent<Renderer>().material.color = sphereStartColor;
                player.GetComponent<Player>().canMove = false;
            }
        }
    }

    private void StartSong()
    {
        foreach (AudioSource track in songLayer1)
        {
            track.mute = false;
            track.time = 0;
        }
        foreach (AudioSource track in songLayer2)
            track.time = 0;
        foreach (AudioSource track in songLayer3)
            track.time = 0;
        foreach (AudioSource track in songLayer4)
            track.time = 0;
        foreach (AudioSource track in songLayer5)
            track.time = 0;
        songStopped = false;
        songStarted = true;
        playAreaScript.StartSpawning();
    }

    public void IncreaseLevel()
    {
        level++;
        soundEffects.PlayOneShot(sound_LevelUp);
        WaveBlast();
        DescentPlatforms();
        playAreaScript.enemyIntervalMultiplier += 0.5f;

        // LE HARD CODE FACE xD
        if (level == 3)
            playAreaScript.areaY -= 2;
        if (level == 5)
            playAreaScript.areaX -= 2;

        cameraManager.GetComponent<CameraManager>().LevelUpFlash();
        Instantiate(levelUpEffect, player.transform.position, Quaternion.identity);
        if (level == 2)
        {
            foreach (AudioSource track in songLayer2)
                track.mute = false;
        }
        else if (level == 3)
        {
            foreach (AudioSource track in songLayer3)
                track.mute = false;
        }
        else if (level == 4)
        {
            foreach (AudioSource track in songLayer4)
                track.mute = false;
        }
        else if (level == 5)
        {
            foreach (AudioSource track in songLayer5)
                track.mute = false;
        }
    }

    public void DecreaseLevel()
    {
        if (level > 1)
        {
            playAreaScript.enemyIntervalMultiplier -= 0.5f;
            if (level == 2)
            {
                foreach (AudioSource track in songLayer2)
                    track.mute = true;
            }
            else if (level == 3)
            {
                foreach (AudioSource track in songLayer3)
                    track.mute = true;
            }
            else if (level == 4)
            {
                foreach (AudioSource track in songLayer4)
                    track.mute = true;
            }
            else if (level == 5)
            {
                foreach (AudioSource track in songLayer5)
                    track.mute = true;
            }
            level--;
        }
    }

    private void WaveBlast()
    {
        Vector3 pos = player.transform.position;
        Instantiate(waveTrigger, pos, Quaternion.identity);
    }

    private void DescentPlatforms()
    {
        Vector3 pos = player.transform.position;
        pos.y = 0;
        GameObject trigger = Instantiate(descentTrigger, pos, Quaternion.identity);
        trigger.GetComponent<ExpandingTrigger>().level = level;
    }

    private void OnBeat()
    {
        if (!songStopped)
        {
            GameObject.Find("PlayArea").GetComponent<PlayArea>().SwitchColors();
            GameObject go = GameObject.Find("LevelUp(Clone)");
            GameObject go2 = GameObject.Find("BackgroundRays");
            if (go)
                go.GetComponent<BounceToBeat>().OnBeat();
            if (go2)
                go2.GetComponent<BounceToBeat>().OnBeat();
            cameraManager.BeatFlash();
            tempoSphere.localScale *= 1.5f;
        }
    }
}
