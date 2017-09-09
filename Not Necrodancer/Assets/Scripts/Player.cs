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
    public bool canMove;
    public int score;
    public AudioClip sound_death;

    private Renderer _rend;
    private CameraManager cameraManager;
    private Color startColor;
    private Color currentColor;
    [HideInInspector] public int beatStreak;
    private bool alreadyMoved;
    private bool beatHit;
    private bool beatHitChecked;
    private bool dead;
    private bool invulnerable;
    private float timeDead;
    private float timeInvulnerable;
    private Vector3 newPosition;

	void Start () {
        _rend = GetComponent<Renderer>();
        startColor = _rend.material.color;
        newPosition = transform.position;
        cameraManager = Camera.main.GetComponent<CameraManager>();
	}

    void Update()
    {
        if (dead)
        {
            timeDead += Time.deltaTime;
            if (timeDead > respawnTime)
            {
                Respawn();
                timeDead = 0;
            }
        }

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
                    beatStreak = 0;
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
                    beatStreak = 0;
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
                    beatStreak = 0;
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
                    beatStreak = 0;
            }
        }

        if (!canMove && !beatHitChecked)
        {
            alreadyMoved = false;
            if (beatHit)
                beatStreak++;
            else
                beatStreak = 0;
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
            score -= 5;
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            GetComponent<AudioSource>().PlayOneShot(sound_death, 0.3f);
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
            GameObject.Find("AudioManager").GetComponent<AudioManager>().IncreaseLevel();
            score = 0;
            Destroy(other.gameObject);
        }
    }
}
