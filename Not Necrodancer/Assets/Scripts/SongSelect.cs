using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelect : MonoBehaviour {

    public GameObject audioManager;
    public GameObject songButtons;
    public GameObject loading;
    public GameObject song_BTW;
    public GameObject song_CMM;
    public GameObject song_CG;

    private GameObject songToBePlayed;
    private AudioManager am;
    private Canvas canvas;

	void Start () {
        am = audioManager.GetComponent<AudioManager>();
	}
	
	void Update () {
        if (am.inGame)
            loading.SetActive(false);
	}

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(song_BTW);
        am.StartGame();
    }

    public void PlayBTW()
    {
        songButtons.SetActive(false);
        loading.SetActive(true);
        songToBePlayed = song_BTW;
        StartCoroutine(StartGame());
    }

    public void PlayCMM()
    {
        songButtons.SetActive(false);
        loading.SetActive(true);
        Instantiate(song_CMM);
        am.StartGame();
    }

    public void PlayCG()
    {
        songButtons.SetActive(false);
        loading.SetActive(true);
        Instantiate(song_CG);
        am.StartGame();
    }
}
