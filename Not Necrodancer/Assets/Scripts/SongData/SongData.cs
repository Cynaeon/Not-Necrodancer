using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongData : MonoBehaviour {

    public AudioSource[] songLayer1;
    public AudioSource[] songLayer2;
    public AudioSource[] songLayer3;
    public AudioSource[] songLayer4;
    public AudioSource[] songLayer5;

    public float bpm;
    public float timeTillSongStart;
    public float secondsToBeat;
    public float songEndTime;
    public float movementWindow;
    public float tempoOffset;

    private void Start()
    {
        /*
        foreach (AudioSource track in songLayer1)
            track.gameObject.SetActive(false);
        foreach (AudioSource track in songLayer2)
            track.gameObject.SetActive(false);
        foreach (AudioSource track in songLayer3)
            track.gameObject.SetActive(false);
        foreach (AudioSource track in songLayer4)
            track.gameObject.SetActive(false);
        foreach (AudioSource track in songLayer5)
            track.gameObject.SetActive(false);
            */
    }

    public void MuteTracks()
    {
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
    }

    public void PauseTracks()
    {
        foreach (AudioSource track in songLayer1)
            track.Pause();
        foreach (AudioSource track in songLayer2)
            track.Pause();
        foreach (AudioSource track in songLayer3)
            track.Pause();
        foreach (AudioSource track in songLayer4)
            track.Pause();
        foreach (AudioSource track in songLayer5)
            track.Pause();
    }

    public void UnpauseTracks()
    {
        foreach (AudioSource track in songLayer1)
            track.Play();
        foreach (AudioSource track in songLayer2)
            track.Play();
        foreach (AudioSource track in songLayer3)
            track.Play();
        foreach (AudioSource track in songLayer4)
            track.Play();
        foreach (AudioSource track in songLayer5)
            track.Play();
    }

    public void StartSong()
    {
        foreach (AudioSource track in songLayer1)
        {
            track.mute = false;
            track.time = 0;
            track.Play();
        }
        foreach (AudioSource track in songLayer2)
        {
            track.time = 0;
            track.Play();
        }
        foreach (AudioSource track in songLayer3)
        {
            track.time = 0;
            track.Play();
        }
        foreach (AudioSource track in songLayer4)
        {
            track.time = 0;
            track.Play();
        }
        foreach (AudioSource track in songLayer5)
        {
            track.time = 0;
            track.Play();
        }
    }

    public void IncreaseLevel(int level)
    {
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

    public void DecreaseLevel(int level)
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
    }

    public void SetTime(float time)
    {
        foreach (AudioSource track in songLayer1)
            track.time = time;
        foreach (AudioSource track in songLayer2)
            track.time = time;
        foreach (AudioSource track in songLayer3)
            track.time = time;
        foreach (AudioSource track in songLayer4)
            track.time = time;
        foreach (AudioSource track in songLayer5)
            track.time = time;
    }

    public void SetTempo(float tempo)
    {
        foreach (AudioSource track in songLayer1)
            track.pitch = tempo;
        foreach (AudioSource track in songLayer2)
            track.pitch = tempo;
        foreach (AudioSource track in songLayer3)
            track.pitch = tempo;
        foreach (AudioSource track in songLayer4)
            track.pitch = tempo;
        foreach (AudioSource track in songLayer5)
            track.pitch = tempo;
    }
}
