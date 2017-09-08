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

    private Renderer _rend;
    private CameraManager cameraManager;
    private Color startColor;
    private Color currentColor;
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
                if (canMove)
                {
                    newPosition = transform.position + new Vector3(2, 0, 0);
                    canMove = false;
                }
                else
                    print("fault");
            }
            if (Input.GetButtonDown("Left"))
            {
                if (canMove)
                {
                    newPosition = transform.position + new Vector3(-2, 0, 0);
                    canMove = false;
                }
                else
                    print("fault");
            }
            if (Input.GetButtonDown("Up"))
            {
                if (canMove)
                {
                    newPosition = transform.position + new Vector3(0, 0, 2);
                    canMove = false;
                }
                else
                    print("fault");
            }
            if (Input.GetButtonDown("Down"))
            {
                if (canMove)
                {
                    newPosition = transform.position + new Vector3(0, 0, -2);
                    canMove = false;
                }
                else
                    print("fault");
            }
        }     

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
            cameraManager.ScreenShake();
            cameraManager.DeathFlash();
            transform.position = new Vector3(0, -100, 0);
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
