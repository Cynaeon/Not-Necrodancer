using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource song;
    public CameraManager cameraManager;

    private float bpm = 0.6f;

    private float current;
    private int beatNumber;

	void Start () {
		
	}
	
	void Update () {
        //print(song.time / bpm);

        if (song.time / bpm > beatNumber)
        {
            cameraManager.FlashBackground();
            beatNumber++;
        }

        /*
        current -= Time.deltaTime;
		if (current <= 0)
        {
            print("BEAT");
            current = bpm;
        }
        */
	}
}
