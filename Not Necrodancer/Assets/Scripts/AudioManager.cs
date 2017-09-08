using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource song;
    public AudioSource[] songLayer1;
    public AudioSource[] songLayer2;
    public AudioSource[] songLayer3;
    public AudioSource[] songLayer4;
    public AudioSource[] songLayer5;
    public AudioSource soundEffects;
    public AudioClip sound_LevelUp;
    public GameObject player;
    
    public CameraManager cameraManager;
    public GameObject waveTrigger;
    public GameObject descentTrigger;
    public Transform tempoSphere;
    public ParticleSystem levelUpEffect;
    public float movementWindow;
    public float tempoOffset;
    public float shrinkSpeed;

    private Player playerScript;
    private PlayArea playAreaScript;
    private int level = 1;
    private GameObject levelUpSphere;
    private Vector3 sphereStartScale;
    private Color sphereStartColor;
    private Color spherePositiveColor;
    private float secondsToBeat = 0.4839f;
    private float current;
    private int beatNumber;

	void Start () {
        foreach (AudioSource track in songLayer1)
            track.mute = false;
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
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
            DescentPlatforms();

        if (Input.GetKeyDown(KeyCode.T))
            IncreaseLevel();

        if (Input.GetButtonDown("Wave"))
            WaveBlast();

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

    public void IncreaseLevel()
    {
        level++;
        soundEffects.PlayOneShot(sound_LevelUp);
        WaveBlast();
        DescentPlatforms();

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

    private void WaveBlast()
    {
        Vector3 pos = player.transform.position;
        Instantiate(waveTrigger, pos, Quaternion.identity);
    }

    private void DescentPlatforms()
    {
        GameObject trigger = Instantiate(descentTrigger, player.transform.position, Quaternion.identity);
        trigger.GetComponent<ExpandingTrigger>().level = level;
    }
         
    private void OnBeat()
    {
        GameObject.Find("PlayArea").GetComponent<PlayArea>().SwitchColors();
        GameObject go = GameObject.Find("LevelUp(Clone)");
        if (go)
            go.GetComponent<BounceToBeat>().OnBeat();
        cameraManager.BeatFlash();
        tempoSphere.localScale *= 1.5f;
    }
}
