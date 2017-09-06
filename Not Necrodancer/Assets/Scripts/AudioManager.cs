﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource song;
    public AudioSource[] songLayer1;
    public AudioSource[] songLayer2;
    public AudioSource[] songLayer3;
    public AudioSource[] songLayer4;
    public GameObject player;
    public CameraManager cameraManager;
    public GameObject waveTrigger;
    public GameObject descentTrigger;
    public Transform tempoSphere;
    public float movementWindow;
    public float tempoOffset;
    public float shrinkSpeed;

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
        sphereStartScale = tempoSphere.localScale;
        sphereStartColor = tempoSphere.GetComponent<Renderer>().material.color;
        spherePositiveColor = Color.green;
	}
	
	void Update () {

        if (Input.GetButtonDown("Wave"))
            WaveBlast();

        if (Input.GetButtonDown("Descent"))
        {
            Vector3 pos = new Vector3(player.transform.position.x, 14, player.transform.position.z);
            Instantiate(descentTrigger, pos, Quaternion.identity);
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

    private void WaveBlast()
    {
        Vector3 pos = player.transform.position;
        Instantiate(waveTrigger, pos, Quaternion.identity);
    }

    private void OnBeat()
    {
        GameObject.Find("PlayArea").GetComponent<PlayArea>().SwitchColors();
        cameraManager.FlashBackground();
        tempoSphere.localScale *= 1.5f;
    }
}
