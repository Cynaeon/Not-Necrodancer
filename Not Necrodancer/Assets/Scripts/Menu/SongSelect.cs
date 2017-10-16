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
    public GameObject song_Lights;

    private Quaternion songListStartRot;
    private Vector3 songListStartPos;
    private GameObject songObject;
    private GameObject songToBePlayed;
    private AudioManager am;
    private Canvas canvas;
    private GameProgress _gp;

	protected override void Start () {
        base.Start();
        am = audioManager.GetComponent<AudioManager>();
        _gp = FindObjectOfType<GameProgress>();
	}
	
	protected override void Update () {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
            ToMainMenu();

        if (songToBePlayed)
        {
            songButtons.SetActive(false);
            songInfo.SetActive(false);
            loading.SetActive(true);
            StartCoroutine(StartGame());
        }
        else
        {
            if (songObject)
                Destroy(songObject);
            songButtons.SetActive(true);
            songInfo.SetActive(true);
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
        am.ResetGame(false);
        songToBePlayed = songObject;
        Destroy(songObject);
    }

    public void ToMenu()
    {
        am.ResetGame(true);
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
        if (_gp.levelUnlocked[1])
            songToBePlayed = song_BTW;
    }

    public void PlayCMM()
    {
        if (_gp.levelUnlocked[2])
            songToBePlayed = song_CMM;
    }

    public void PlayCG()
    {
        if (_gp.levelUnlocked[3])
            songToBePlayed = song_CG;
    }

    public void PlayShelter()
    {
        if (_gp.levelUnlocked[4]) 
            songToBePlayed = song_Shelter;
    }

    public void PlayTMA()
    {
        if (_gp.levelUnlocked[6])
            songToBePlayed = song_TMA;
    }

    public void PlayLights()
    {
        if (_gp.levelUnlocked[5])
            songToBePlayed = song_Lights;
    }
}
