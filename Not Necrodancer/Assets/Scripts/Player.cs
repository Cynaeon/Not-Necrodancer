using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;
    public ParticleSystem deathEffect;
    public float respawnTime;
    public float invulTime;
    public Color invulColor;
    public Color[] colorGradient = new Color[6];
    public float colorShiftSpeed;
    public bool canMove;
    public int streakToStarPower;
    public int score;
    public SoundEffects soundEffects;

    private Renderer _rend;
    private CameraManager cameraManager;
    private AudioManager _audioManager;
    private Color startColor;
    private Color currentColor;
    private GameObject fastForwardInstance;
    private int currentColorIndex;
    private float currentShiftTime;
    [HideInInspector] public int beatStreak;
    private int fastsSpawned;
    private bool alreadyMoved;
    private bool beatHit;
    private bool beatHitChecked;
    private bool dead;
    private bool invulnerable;
    internal bool starPowerActive;
    private float timeDead;
    private float timeInvulnerable;
    private Vector3 startPos;
    private Vector3 newPosition;

	void Start () {
        startPos = transform.position;
        _rend = GetComponent<Renderer>();
        startColor = _rend.material.color;
        newPosition = transform.position;
        cameraManager = Camera.main.GetComponent<CameraManager>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
	}

    void Update()
    {
        if (starPowerActive)
            ShiftColor();
        else
        {
            _rend.material.color = startColor;
        }

        if (dead)
        {
            timeDead += Time.deltaTime;
            if (timeDead > respawnTime)
            {
                Respawn();
                timeDead = 0;
            }
        }

        // v FIX THIS AAAAAAA v
        else if (transform.position == newPosition)
        {
            if (Input.GetButtonDown("Right"))
            {
                if (canMove && !alreadyMoved)
                {
                    newPosition = transform.position + new Vector3(2, 0, 0);
                    beatHit = true;
                    alreadyMoved = true;
                }
                else
                {
                    beatStreak = 0;
                    GameObject.Find("Missed").GetComponent<PopupText>().Activate();
                }
            }
            if (Input.GetButtonDown("Left"))
            {
                if (canMove && !alreadyMoved)
                {
                    newPosition = transform.position + new Vector3(-2, 0, 0);
                    beatHit = true;
                    alreadyMoved = true;
                }
                else
                {
                    beatStreak = 0;
                    GameObject.Find("Missed").GetComponent<PopupText>().Activate();
                }
            }
            if (Input.GetButtonDown("Up"))
            {
                if (canMove && !alreadyMoved)
                {
                    newPosition = transform.position + new Vector3(0, 0, 2);
                    beatHit = true;
                    alreadyMoved = true;
                }
                else
                {
                    beatStreak = 0;
                    GameObject.Find("Missed").GetComponent<PopupText>().Activate();
                }
            }
            if (Input.GetButtonDown("Down"))
            {
                if (canMove && !alreadyMoved)
                {
                    newPosition = transform.position + new Vector3(0, 0, -2);
                    beatHit = true;
                    alreadyMoved = true;
                }
                else
                {
                    beatStreak = 0;
                    GameObject.Find("Missed").GetComponent<PopupText>().Activate();
                }
            }
        }

        if (!canMove && !beatHitChecked)
        {
            alreadyMoved = false;
            if (beatHit)
                beatStreak++;
            else
            {
                if (beatStreak > 0)
                    GameObject.Find("Missed").GetComponent<PopupText>().Activate();
                beatStreak = 0;
            }
            beatHit = false;
            beatHitChecked = true;
        }

        if (canMove)
            beatHitChecked = false;

        if (!dead)
            transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);

        if (invulnerable)
        {
            currentColor = Color.Lerp(startColor, invulColor, Mathf.PingPong(Time.time * 6, 1));
            _rend.material.color = currentColor;
            timeInvulnerable += Time.deltaTime;
            if (timeInvulnerable > invulTime)
            {
                invulnerable = false;
                _rend.material.color = startColor;
                timeInvulnerable = 0;
            }
        }

        if (beatStreak >= streakToStarPower * (fastsSpawned + 1))
        {
            if (!GameObject.Find("FastForward(Clone)"))
            {
                GameObject.Find("PlayArea").GetComponent<PlayArea>().SpawnFastForward();
                fastsSpawned++;
            }
        }
    }

    private void Respawn()
    {
        dead = false;
        invulnerable = true;
        transform.position = new Vector3(0, 1, 0);
    }

    private void Die()
    {
        if (!invulnerable)
        {
            score -= _audioManager.scoreToLevelUp / 2;
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            soundEffects.PlayerDeath();
            cameraManager.ScreenShake();
            cameraManager.DeathFlash();
            transform.position = new Vector3(100, 100, 100);
            newPosition = new Vector3(0, 1, 0);
            GameObject go = GameObject.Find("LevelUp(Clone)");
            if (go)
                Destroy(go);
            dead = true;
        }
    }

    public void Reset()
    {
        dead = false;
        invulnerable = false;
        score = 0;
        currentColorIndex = 0;
        currentShiftTime = 0;
        beatStreak = 0;
        fastsSpawned = 0;
        transform.position = startPos;
        newPosition = startPos;
    }

    private void ShiftColor()
    {
        int nextColor = currentColorIndex + 1;
        if (nextColor >= colorGradient.Length)
            nextColor = 0;
        currentShiftTime += Time.deltaTime * colorShiftSpeed;
        _rend.material.color = Color.Lerp(colorGradient[currentColorIndex], colorGradient[nextColor], currentShiftTime);
        if (currentShiftTime > 1)
        {
            currentColorIndex++;
            if (currentColorIndex >= colorGradient.Length)
                currentColorIndex = 0;
            currentShiftTime = 0;
        }
    }

    internal void ToggleStarPower()
    {
        if (starPowerActive)
            starPowerActive = false;
        else
            starPowerActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            score++;
            Destroy(other.gameObject);
        }
        if (other.tag == "Platform")
        {
            if (other.GetComponent<Platform>().spinning)
                Die();
        }
        if (other.tag == "Blade" || other.tag == "Death")
            Die();
        if (other.tag == "LevelUp")
        {
            _audioManager.IncreaseLevel();
            score = 0;
            Destroy(other.gameObject);
        }
        if (other.tag == "FastForward")
        {
            _audioManager.ActivateStarPower();
            Destroy(other.gameObject);
        }
    }
}
