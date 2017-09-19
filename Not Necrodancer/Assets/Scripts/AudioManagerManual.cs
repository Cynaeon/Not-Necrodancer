using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManagerManual : MonoBehaviour {

    public AudioSource song;
    public float movementWindow;
    private List<float> charts = new List<float>() { 22.2f, 0.8f, 0.6f, 0.8f, 0.6f, 0.8f, 0.6f, 0.8f, 0.4f, 0.2f, 0.8f, 0.6f, 0.8f, 0.6f,
    0.8f, 0.6f, 0.8f, 0.6f, 0.8f, 0.6f, 0.8f, };

    private GameObject tempoSphere;
    private Player playerScript;
    private bool songStarted = true;
    private float closestBeat;
    private float nextBeat;
    private int beatNumber;

	void Start () {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        tempoSphere = GameObject.Find("Tempo");
        nextBeat += charts[beatNumber];
        closestBeat += charts[beatNumber];
        song.time = 20;
	}

    void Update() {

        print(Math.Abs(song.time - closestBeat));

        if (Math.Abs(song.time - closestBeat) < movementWindow)
        {
            playerScript.canMove = true;
            tempoSphere.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            tempoSphere.GetComponent<Renderer>().material.color = Color.red;
            playerScript.canMove = false;
            if (closestBeat + movementWindow < song.time)
                closestBeat = nextBeat;
        }

        if (song.time >= nextBeat)
        {
            OnBeat();
            beatNumber++;
            nextBeat += charts[beatNumber];
        }
        

        /*
        float beatTime = (song.time / secondsToBeat) - tempoOffset;

        if (beatTime > beatNumber)
        {
            beatNumber++;
            OnBeat();
        }

        float closestBeat = Mathf.Round(beatTime);

        if (Math.Abs(closestBeat - beatTime) < movementWindow)
        {
            playerScript.canMove = true;
        }
        else
        {
            playerScript.canMove = false;
        }
        */
    }

    private void OnBeat()
    {
        GameObject.Find("PlayArea").GetComponent<PlayArea>().SwitchColors();
        GameObject go = GameObject.Find("LevelUp(Clone)");
        GameObject go2 = GameObject.Find("BackgroundRays");
        if (go)
            go.GetComponent<BounceToBeat>().OnBeat();
        if (go2)
            go2.GetComponent<BounceToBeat>().OnBeat();
        
    }
}
