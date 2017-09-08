﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed;
    public float waveSpan;
    public float timeTillFall;
    public float spinSpeed;
    public float spinSpeedFalloff;
    public float spinDuration;
    public Color idleColor;
    public Color activeColor1;
    public Color activeColor2;
    public Color dangerColor;
    public int level;
    public int variant;
    public GameObject deathSphere;

    enum State
    {
        descended = 0,
        descending = 1,
        elevated = 2
    }

    private State state;

    private bool elevated;
    private bool descending;
    private bool danger;
    private bool instantiatedDeath;
    private Color currentColor;
    private Color descendedColor;
    private Vector3 elevatedPos;
    private Vector3 endPos;
    private float currentTimeTillFall;
    private float descentTime;
    private float currentSpinSpeed;
    private float currentSpinTime;
    private float spinFalloffMultiplier = 1;
    private Renderer _rend;
    private bool wave;
    [HideInInspector] public bool spinning;
    private float startY;
    private float waveTime;

	void Start () {
        currentSpinSpeed = spinSpeed;
        _rend = GetComponent<Renderer>();
        if (variant == 1)
        {
            currentColor = activeColor1;
            _rend.material.color = activeColor1;
        }
        else
        {
            currentColor = activeColor2;
            _rend.material.color = activeColor2;
        }
        elevatedPos = transform.position;
        endPos = new Vector3(transform.position.x, -50, transform.position.z);
        descendedColor = Color.clear;
        elevated = true;
	}
	
	void Update () {

        if (danger)
        {
            dangerColor = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.time * 6, 1));
            _rend.material.color = dangerColor;
        }
        
        if (spinning)
        {
            transform.Rotate(Vector3.right * currentSpinSpeed * Time.deltaTime);
            currentSpinTime += Time.deltaTime;
            if (currentSpinTime > spinDuration)
            {
                transform.eulerAngles = Vector3.zero;
                spinning = false;
                currentSpinTime = 0;
            }

            #region OldSpin 
            /*
            if (currentSpinSpeed > 50)
            {
                currentSpinSpeed -= spinSpeedFalloff * Time.deltaTime * spinFalloffMultiplier;
                spinFalloffMultiplier += 0.4f;    
            }
            else
            {
                if ((transform.eulerAngles.x > -2 && transform.eulerAngles.x < 2))
                {
                    transform.eulerAngles = Vector3.zero;
                    spinning = false;
                    currentSpinSpeed = spinSpeed;
                    spinFalloffMultiplier = 1;
                }
            }
            */
            #endregion
        }

        else if (wave)
        {
            waveTime = waveTime + Time.deltaTime;
            float y = startY + Mathf.Sin(waveTime * speed) * waveSpan / 2;
            
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            
            if (transform.position.y < startY)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                wave = false;
            }
        }
       
        else if (!elevated)
        {
            
            currentTimeTillFall += Time.deltaTime;
            if (currentTimeTillFall > timeTillFall)
            {
                if (!instantiatedDeath)
                {
                    Instantiate(deathSphere, transform.position, Quaternion.identity);
                    instantiatedDeath = true;
                }
                danger = false;
                descentTime = descentTime + Time.deltaTime / 2;
                transform.position = Vector3.Lerp(elevatedPos, endPos, descentTime);
                _rend.material.color = Color.Lerp(idleColor, descendedColor, descentTime);

                if (transform.position.y <= endPos.y)
                {
                    transform.position = new Vector3(transform.position.x, endPos.y, transform.position.z);
                    state = State.descended;
                }
            }
            else
            {
                danger = true;
            }
        }
	}

    void Wave()
    {
        wave = true;
        waveTime = 0;
        startY = 0;
    }

    void Descent()
    {
        if (state == State.elevated)
        {
            state = State.descending;
        }
    }

    void Spin()
    {
        spinning = true;
    }

    public void SwitchColor()
    {
        if (elevated)
        {
            if (currentColor == activeColor1)
            {
                currentColor = activeColor2;
            }
            else
            {
                currentColor = activeColor1;
            }
            if (!danger)
                _rend.material.color = currentColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WaveTrigger")
            Wave();
        if (other.tag == "DescentTrigger")
        {
            if (elevated && other.GetComponent<ExpandingTrigger>().level == level)
                elevated = false;
        }
        if (other.tag == "Blade")
            Spin();
        if (other.tag == "DangerTrigger")
            danger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DangerTrigger")
        {
            danger = false;
        }
    }
}
