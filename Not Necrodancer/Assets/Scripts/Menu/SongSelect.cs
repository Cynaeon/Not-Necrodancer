using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SongSelect : MenuScreen {

    public GameObject audioManager;
    public Transform songList;
    public GameObject firstSelectedMenu;

    public GameObject menuBackground;
    public MenuScreen mainMenu;
    public GameObject songInfo;
    public GameObject songButtons;
    public GameObject loading;
    public GameObject smoke;
    public GameObject waveTrigger;
    public float waveIntervalMin;
    public float waveIntervalMax;
    public GameObject song_BTW;
    public GameObject song_CMM;
    public GameObject song_CG;
    public GameObject song_Shelter;
    public GameObject song_TMA;

    private Quaternion songListStartRot;
    private Vector3 songListStartPos;
    private GameObject songObject;
    private GameObject songToBePlayed;
    private AudioManager am;
    private Canvas canvas;

	protected override void Start () {
        base.Start();
        am = audioManager.GetComponent<AudioManager>();
	}
	
	protected override void Update () {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
            ToMainMenu();

        if (songToBePlayed)
        {
            songButtons.SetActive(false);
            songInfo.SetActive(false);
            menuBackground.SetActive(false);
            loading.SetActive(true);
            StartCoroutine(StartGame());
        }
        else
        {
            if (songObject)
                Destroy(songObject);
            songButtons.SetActive(true);
            songInfo.SetActive(true);
            menuBackground.SetActive(true);
            loading.SetActive(false);
        }

        if (am.inGame)
            loading.SetActive(false);
    }

    private void ToMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        mainMenu.enabled = true;
        gameObject.SetActive(false);
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        if (songToBePlayed)
        {
            songObject = Instantiate(songToBePlayed);
        }
        songToBePlayed = null;
        gameObject.SetActive(false);
        am.StartGame();
    }

    public void PlayAgain()
    {
        am.ResetGame();
        songToBePlayed = songObject;
        Destroy(songObject);
    }

    public void ToMenu()
    {
        am.ResetGame();
        Destroy(songObject);
        targetRot = 0;
        entered = false;
        songList.position = songListStartPos;
        songList.rotation = songListStartRot;
        
        songButtons.SetActive(true);
        songInfo.SetActive(true);
    }

    public void PlayBTW()
    { 
        songToBePlayed = song_BTW;
    }

    public void PlayCMM()
    {
        songToBePlayed = song_CMM;
    }

    public void PlayCG()
    {
        songToBePlayed = song_CG;
    }

    public void PlayShelter()
    {
        songToBePlayed = song_Shelter;
    }

    public void PlayTMA()
    {
        songToBePlayed = song_TMA;
    }
}
