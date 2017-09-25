using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {
    
    public GameObject player;
    public GameObject scoringSystem;
    public GameObject pauseUI;
    public GameObject pauseButtons;
    public GameObject songEndMenu;
    public EventSystem eventSystem;
    public GameObject firstSelectedPause;
    public GameObject score;
    public GameObject hiscoretable;
    public GameObject backGroundRays;
    public CameraManager cameraManager;
    public LightController lightController;
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
    private float starPowerTime;
    private float current;
    private float currentTempoIncrease = 1;
    private int beatNumber;

	void Start () {
        soundEffects = GetComponent<SoundEffects>();
        playerScript = player.GetComponent<Player>();
        SetScripts(false);
        scoringSystem.SetActive(false);
        score.SetActive(false);
        hiscoretable.SetActive(false);
        songEndMenu.SetActive(false);
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
        lightController.ToggleOn();
        score.SetActive(true);
        scoringSystem.SetActive(true);
        backGroundRays.SetActive(true);
        SetScripts(true);
        Camera.main.GetComponent<CameraManager>().SetToGamePosition();
        Camera.main.GetComponent<RotateAround>().enabled = false;
        songTime = 0;
        _songData = GameObject.FindGameObjectWithTag("SongData").GetComponent<SongData>();
        _songData.MuteTracks();
        secondsToBeat = _songData.secondsToBeat;
        songEndTime = _songData.songEndTime;
        timeTillSongStart = _songData.timeTillSongStart;
        movementWindow = _songData.movementWindow;
        tempoOffset = _songData.tempoOffset;
        sphereStartScale = tempoSphere.localScale;
        sphereStartColor = tempoSphere.GetComponent<Renderer>().material.color;
        spherePositiveColor = Color.green;
        songStopped = true;
    }

    void Update () {

        if (!inGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GameObject.FindGameObjectWithTag("SongData"))
                    StartGame();
            }
        }
        else 
        {
            songTime += Time.deltaTime;
            if (Input.GetButtonDown("Pause"))
            {
                if (gamePaused)
                    UnpauseGame();
                else
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

            Tempo();

            if (Input.GetKeyDown(KeyCode.U))
                IncreaseLevel();

            if (songTime > _songData.timeTillSongStart && songStopped)
                StartSong();

            if (_songData.songLayer1[0].time > songEndTime && songStopped == false)
            {
                SongEnd();
            }

            if (Input.GetButtonDown("Descent"))
            {
                Vector3 pos = new Vector3(player.transform.position.x, 14, player.transform.position.z);
                Instantiate(descentTrigger, pos, Quaternion.identity);
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
    }

    private void SongEnd()
    {
        playAreaScript.StopSpawning();
        hiscoretable.SetActive(true);
        songEndMenu.SetActive(true);
        Camera.main.GetComponent<CameraManager>().BlurScreen();
        songStopped = true;
    }

    public void ResetGame()
    {
        UnpauseGame();
        DeleteGameObjects();
        playAreaScript.ResetPlatforms();
        inGame = false;
        hiscoretable.SetActive(false);
        songEndMenu.SetActive(false);
        starPower.SetActive(false);
        beatNumber = 0;
        level = 1;
        songStopped = true;
        lightController.ToggleOff();
        Camera.main.GetComponent<RotateAround>().enabled = true;
        cameraManager.Reset();
        backGroundRays.transform.eulerAngles = new Vector3(-90, 0, 0);
        backGroundRays.SetActive(false);
        scoringSystem.GetComponent<ScoringSystem>().ResetScore();
        playerScript.Reset();
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
        pauseUI.SetActive(true);
        pauseButtons.SetActive(true);
        eventSystem.SetSelectedGameObject(firstSelectedPause);
        gamePaused = true;
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        playerScript.enabled = true;
        _songData.UnpauseTracks();
        Camera.main.GetComponent<CameraManager>().UnblurScreen();
        pauseButtons.SetActive(false);
        pauseUI.SetActive(false);
        gamePaused = false;
    }

    private void StartSong()
    {
        _songData.StartSong();
        songStopped = false;
        playAreaScript.StartSpawning();
    }

    public void IncreaseLevel()
    {
        level++;
        soundEffects.LevelUp();
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
        _songData.SetTempo(currentTempoIncrease);
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
            //tempoSphere.localScale *= 1.5f;
        }
    }
}
