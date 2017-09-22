using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed;
    public float waveSpan;
    public float timeTillFall;
    public float spinSpeed;
    public Color activeColor1;
    public Color activeColor2;
    public Color dangerColor;
    public int level;
    public int variant;
    public GameObject deathSphere;
    public GameObject platformBase;

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
    private Color idleColor;
    private Color currentActiveColor;
    private Color descendedColor;
    private Vector3 elevatedPos;
    private Vector3 endPos;
    private SongData _songData;
    private float spinDuration;
    private float currentTimeTillFall;
    private float descentTime;
    private float currentSpinSpeed;
    private float currentSpinTime;
    private int dangerLevel;
    //private float spinFalloffMultiplier = 1;
    private Renderer _rend;
    private bool wave;
    [HideInInspector] public bool spinning;
    private float startY;
    private float waveTime;

	void Start () {
        currentSpinSpeed = spinSpeed;
        _rend = GetComponent<Renderer>();
        if (variant == 1)
            currentActiveColor = activeColor1;
        else
            currentActiveColor = activeColor2;
        idleColor = _rend.material.color;
        elevatedPos = transform.position;
        endPos = new Vector3(transform.position.x, -50, transform.position.z);
        descendedColor = Color.clear;
        elevated = true;
        
        //spinDuration = GameObject.FindGameObjectWithTag("SongData").GetComponent<SongData>().secondsToBeat * 2;
	}

	
	void Update () {

        if (!_songData)
        {
            if (GameObject.FindGameObjectWithTag("SongData"))
                _songData = GameObject.FindGameObjectWithTag("SongData").GetComponent<SongData>();
        }
            
        else
            spinDuration = _songData.secondsToBeat * 2;

        if (danger)
        {
            dangerColor = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.time * 6, 1));
            _rend.material.color = dangerColor;
        }
        
        if (spinning)
        {
            dangerColor = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.time * 6, 1));
            _rend.material.color = dangerColor;
            transform.Rotate(Vector3.right * currentSpinSpeed * Time.deltaTime);
            currentSpinTime += Time.deltaTime;
            if (currentSpinTime > spinDuration)
            {
                transform.eulerAngles = Vector3.zero;
                spinning = false;
                currentSpinTime = 0;
                _rend.material.color = idleColor;
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
                platformBase.SetActive(false);
                _rend.material.color = Color.Lerp(idleColor, descendedColor, descentTime);

                if (transform.position.y <= endPos.y)
                {
                    transform.position = new Vector3(transform.position.x, endPos.y, transform.position.z);
                    gameObject.SetActive(false);
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
            if (currentActiveColor == activeColor1)
                currentActiveColor = activeColor2;
            else
                currentActiveColor = activeColor1;
            if (!danger)
                _rend.material.color = currentActiveColor;
        }
    }

    public void SetToEndColor()
    {
        if (elevated)
        {
            _rend.material.color = activeColor1;
        }
    }

    public void SetToIdleColor()
    {
        _rend.material.color = idleColor;
    }

    public void ResetPlatform()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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
        if (other.tag == "SongEndTrigger")
            SwitchColor();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "DangerTrigger")
        {
            int beatsToDrop = other.transform.parent.GetComponent<Enemy>().beatsToDrop;
            int newDangerLevel;
            if (beatsToDrop <= 2)
                newDangerLevel = 15;
            else if (beatsToDrop < 4)
                newDangerLevel = 7;
            else
                newDangerLevel = 1;

            if (newDangerLevel != dangerLevel)
                dangerLevel = newDangerLevel;
            dangerColor = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.time * dangerLevel, 1));
            _rend.material.color = dangerColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DangerTrigger")
        {
            danger = false;
        }
    }
}
