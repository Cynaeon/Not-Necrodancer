using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {
    
    public GameObject player;
    public ScoringSystem scoringSystem;
    public MenuScreen pauseMenu;
    public GameObject pauseUI;
    public MenuScreen songEndMenu;
    public EventSystem eventSystem;
    public GameObject firstSelectedPause;
    public GameObject score;
    public GameObject hiscoretable;
    public GameObject backGroundRays;
    public CameraManager cameraManager;
    public GameObject waveTrigger;
    public GameObject descentTrigger;
    public Transform tempoSphere;
    public ParticleSystem levelUpEffect;
    public GameObject starPower;

    public int scoreToLevelUp;
    public float starPowerDuration;
    public float tempoIncrease;
    public float tempoIncreaseSpeed;
    public float shrinkSpeed;

    internal int level = 1;
    internal int maxLevel = 5;
    internal bool inGame;
    internal bool songStopped;
    internal bool onBeat;

    internal float songTime;
    internal float secondsToBeat;
    internal float songEndTime;
    internal float timeTillSongStart;
    internal float scoreToFiveStars;
    internal float movementWindow;
    internal float tempoOffset;

    private SoundEffects soundEffects;
    private SongData _songData;
    private bool gamePaused;
    private bool highTempo;
    private Player playerScript;
    private PlayArea playAreaScript;
    private GameObject levelUpSphere;
    private Vector3 sphereStartScale;
    private Color sphereStartColor;
    private Color spherePositiveColor;
    private float waveInterval = 10;
    private float starPowerTime;
    private float current;
    private float currentTempoIncrease = 1;
    private int beatNumber;

	void Start () {
        Cursor.visible = false;
        soundEffects = GetComponent<SoundEffects>();
        playerScript = player.GetComponent<Player>();
        SetScripts(false);
        scoringSystem.enabled = false;
        score.SetActive(false);
        hiscoretable.SetActive(false);
        songEndMenu.gameObject.SetActive(false);
        backGroundRays.SetActive(false);
    }

    private void SetScripts(bool state)
    {
        Camera.main.GetComponent<CameraManager>().enabled = state;
        playAreaScript = GameObject.Find("PlayArea").GetComponent<PlayArea>();
        //playAreaScript.SetPlatformScripts(state);
        playAreaScript.enabled = state;
        playerScript.enabled = state;

    }

    public void StartGame()
    {
        inGame = true;
        score.SetActive(true);
        scoringSystem.enabled = true;
        backGroundRays.SetActive(true);
        SetScripts(true);
        playerScript.enabled = false;
        Camera.main.GetComponent<CameraManager>().SetToGamePosition();
        Camera.main.GetComponent<RotateAround>().enabled = false;
        songTime = 0;
        _songData = GameObject.FindGameObjectWithTag("SongData").GetComponent<SongData>();
        _songData.MuteTracks();
        secondsToBeat = _songData.secondsToBeat;
        songEndTime = _songData.songEndTime;
        timeTillSongStart = _songData.timeTillSongStart;
        scoreToFiveStars = _songData.scoreToFiveStars;
        movementWindow = _songData.movementWindow;
        tempoOffset = _songData.tempoOffset;
        scoreToLevelUp = (int) _songData.songEndTime / 7 / 3;
        sphereStartScale = tempoSphere.localScale;
        sphereStartColor = tempoSphere.GetComponent<Renderer>().material.color;
        spherePositiveColor = Color.green;
        songStopped = true;
    }

    void Update () {

        Tempo();

        if (inGame)
        {
            songTime += Time.deltaTime;
            if (Input.GetButtonDown("Pause"))
            {
                if (!gamePaused)
                    PauseGame();
            }
            /*
            if (Input.GetKeyDown(KeyCode.Space) && playerScript.starPower >= starPowerSlider.maxValue || Input.GetKeyDown(KeyCode.P))
            {
                playerScript.ToggleStarPower();
                highTempo = true;
            }

            starPowerSlider.value = playerScript.starPower;
            */
            /*
            if (Input.GetKeyDown(KeyCode.U))
                IncreaseLevel();

            */
            if (songTime > _songData.timeTillSongStart && songStopped && songTime < songEndTime)
                StartSong();

            if (_songData.songLayer1[0].time > songEndTime && songStopped == false)
            {
                SongEnd();
            }

            if (playerScript.score >= scoreToLevelUp)
            {
                if (!GameObject.Find("LevelUp(Clone)") && level < maxLevel)
                {
                    playAreaScript.SpawnLevelUp();
                }
            }

            if (playerScript.score < 0)
            {
                if (level > 1)
                {
                    DecreaseLevel();
                    playerScript.score += 10;
                }
                else
                    playerScript.score = 0;
            }

            if (tempoSphere.localScale.x > sphereStartScale.x)
            {
                float value = shrinkSpeed * Time.deltaTime;
                tempoSphere.localScale -= new Vector3(value, value, value);
            }

            if (!songStopped)
            {
                float beatTime = (_songData.songLayer1[0].time / secondsToBeat) - tempoOffset;
                if (beatTime > beatNumber)
                {
                    beatNumber++;
                    OnBeat();
                    onBeat = true;
                }
                else
                    onBeat = false;

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
        else
            SpawnWaves();
    }

    private void SpawnWaves()
    {
        waveInterval -= Time.deltaTime;
        if (waveInterval < 0)
        {
            Vector3 pos;
            int rnd = UnityEngine.Random.Range(1, 6);
            if (rnd == 1)
                pos = new Vector3(7, 0, 7);
            else if (rnd == 2)
                pos = new Vector3(-7, 0, 7);
            else if (rnd == 3)
                pos = new Vector3(7, 0, -7);
            else if (rnd == 4)
                pos = new Vector3(-7, 0, -7);
            else
                pos = Vector3.zero;
            WaveBlast(pos);
            waveInterval = 10;
        }
    }

    private void SongEnd()
    {
        DeleteGameObjects();
        playAreaScript.StopSpawning();
        playerScript.enabled = false;
        hiscoretable.SetActive(true);
        starPower.SetActive(false);
        highTempo = false;
        currentTempoIncrease = 0;
        songEndMenu.gameObject.SetActive(true);
        songEndMenu.enabled = true;
        songStopped = true;
        inGame = false;
    }

    public void ResetGame(bool unloadSong)
    {
        UnpauseGame();
        DeleteGameObjects();
        playAreaScript.ResetPlatforms();
        inGame = false;
        hiscoretable.SetActive(false);
        highTempo = false;
        currentTempoIncrease = 0;
        starPower.SetActive(false);
        starPowerTime = 0;
        songTime = 0;
        beatNumber = 0;
        level = 1;
        songStopped = true;
        GameObject.Find("Ready").GetComponent<TextReady>().Reset();
        cameraManager.Reset();
        Camera.main.GetComponent<RotateAround>().enabled = true;
        backGroundRays.transform.eulerAngles = new Vector3(-90, 0, 0);
        backGroundRays.SetActive(false);
        scoringSystem.GetComponent<ScoringSystem>().ResetScore();
        playerScript.Reset();
        playerScript.StarPowerOff();
        if (unloadSong)
            _songData.UnloadTracks();
        _songData = null;
        SetScripts(false);
    }

    private void DeleteGameObjects()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Blade");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (GameObject pickup in pickups)
            Destroy(pickup);
        GameObject[] levelups = GameObject.FindGameObjectsWithTag("LevelUp");
        foreach (GameObject levelup in levelups)
            Destroy(levelup);
        GameObject[] fastforwards = GameObject.FindGameObjectsWithTag("FastForward");
        foreach (GameObject fastforward in fastforwards)
            Destroy(fastforward);
    }

    public void ActivateStarPower()
    {
        highTempo = true;
        playerScript.ToggleStarPower();
    }

    private void PauseGame()
    {
        playerScript.enabled = false;
        _songData.PauseTracks();
        Camera.main.GetComponent<CameraManager>().BlurScreen();
        pauseMenu.gameObject.SetActive(true);
        pauseMenu.enabled = true;
        //pauseUI.SetActive(true);
        //pauseButtons.SetActive(true);
        eventSystem.SetSelectedGameObject(firstSelectedPause);
        gamePaused = true;
        Time.timeScale = 0;
    }

    internal void UnpauseGame()
    {
        Time.timeScale = 1;
        playerScript.enabled = true;
        _songData.UnpauseTracks();
        Camera.main.GetComponent<CameraManager>().UnblurScreen();
        pauseMenu.gameObject.SetActive(false);
        pauseMenu.enabled = false;
        //pauseButtons.SetActive(false);
        //pauseUI.SetActive(false);
        gamePaused = false;
    }

    private void StartSong()
    {
        _songData.StartSong();
        songStopped = false;
        playerScript.enabled = true;
        playAreaScript.StartSpawning();
    }

    public void IncreaseLevel()
    {
        level++;
        soundEffects.LevelUp();
        Vector3 pos = player.transform.position;
        WaveBlast(pos);
        DescentPlatforms();
        playAreaScript.enemyIntervalMultiplier += 0.5f;

        // LE HARD CODE FACE xD
        if (level == 3)
            playAreaScript.areaY -= 2;
        if (level == 5)
            playAreaScript.areaX -= 2;

        cameraManager.GetComponent<CameraManager>().LevelUpFlash();
        Instantiate(levelUpEffect, player.transform.position, Quaternion.identity);
        _songData.IncreaseLevel(level);
    }

    public void DecreaseLevel()
    {
        if (level > 1)
        {
            playAreaScript.enemyIntervalMultiplier -= 0.5f;
            _songData.DecreaseLevel(level);
            level--;
        }
    }

    private void Tempo()
    {
        if (highTempo)
        {
            starPower.SetActive(true);
            if (currentTempoIncrease < tempoIncrease)
                currentTempoIncrease += Time.deltaTime * tempoIncreaseSpeed;
            else
                currentTempoIncrease = tempoIncrease;
            starPowerTime += Time.deltaTime;
            if (starPowerTime >= starPowerDuration)
            {
                highTempo = false;
                playerScript.ToggleStarPower();
                starPowerTime = 0;
            }
        }
        else
        {
            starPower.SetActive(false);
            if (currentTempoIncrease > 1)
                currentTempoIncrease -= Time.deltaTime * tempoIncreaseSpeed;
            else
                currentTempoIncrease = 1;
        }
        if (_songData) 
            _songData.SetTempo(currentTempoIncrease);
    }

    private void WaveBlast(Vector3 pos)
    {
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
            EventManager.TriggerEvent("Bounce");
            //tempoSphere.localScale *= 1.5f;
        }
    }
}
