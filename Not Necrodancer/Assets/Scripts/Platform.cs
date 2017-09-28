using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed;
    public float waveSpan;
    public float timeTillFall;
    public float spinSpeed;
    public Material idleMaterial;
    public Material activeMaterial1;
    public Material activeMaterial2;
    public Material dangerMaterial;
    public Material descendedMaterial;
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

    private GameObject deathSphereInst;
    private bool elevated;
    private bool descending;
    private bool danger;
    private bool instantiatedDeath;
    private Material currentActiveMaterial;
    private Vector3 elevatedPos;
    private Vector3 endPos;
    private SongData _songData;
    private float spinDuration;
    private float currentTimeTillFall;
    private float descentTime;
    private float currentSpinSpeed;
    private float currentSpinTime;
    private int dangerLevel;
    private Renderer _rend;
    private bool wave;
    [HideInInspector] public bool spinning;
    private float startY;
    private float waveTime;

	void Start () {
        currentSpinSpeed = spinSpeed;
        _rend = GetComponent<Renderer>();
        if (variant == 1)
            currentActiveMaterial = activeMaterial1;
        else
            currentActiveMaterial = activeMaterial2;
        _rend.material = currentActiveMaterial;
        elevatedPos = transform.position;
        endPos = new Vector3(transform.position.x, -50, transform.position.z);
        elevated = true;
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
            float lerp = Mathf.PingPong(Time.time * 15, 1);
            _rend.material.Lerp(idleMaterial, dangerMaterial, lerp);
        }
        
        if (spinning)
        {
            float lerp = Mathf.PingPong(Time.time * 15, 1);
            _rend.material.Lerp(idleMaterial, dangerMaterial, lerp);
            transform.Rotate(Vector3.right * currentSpinSpeed * Time.deltaTime);
            currentSpinTime += Time.deltaTime;
            if (currentSpinTime > spinDuration)
            {
                transform.eulerAngles = Vector3.zero;
                spinning = false;
                currentSpinTime = 0;
                _rend.material = idleMaterial;
            }
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
                    deathSphereInst = Instantiate(deathSphere, transform.position, Quaternion.identity);
                    instantiatedDeath = true;
                }
                danger = false;
                descentTime = descentTime + Time.deltaTime / 2;
                transform.position = Vector3.Lerp(elevatedPos, endPos, descentTime);
                platformBase.SetActive(false);
                _rend.material.Lerp(idleMaterial, descendedMaterial, descentTime);

                if (transform.position.y <= endPos.y)
                {
                    transform.position = new Vector3(transform.position.x, endPos.y, transform.position.z);
                    _rend.enabled = false;
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

    public void SwitchColor(int max, int current)
    {
        if (elevated)
        {
            if (currentActiveMaterial == activeMaterial1)
                currentActiveMaterial = activeMaterial2;
            else
                currentActiveMaterial = activeMaterial1;
            if (!danger)
            {
                _rend.material = currentActiveMaterial;
                if (current > max)
                    current = max;
                _rend.material.SetColor("_EmissionColor", (_rend.material.GetColor("_EmissionColor") * (1 + 0.1f * current)));
            }
        }
    }

    public void SetToEndColor()
    {
        if (elevated)
        {
            _rend.material = activeMaterial1;
        }
    }

    public void SetToIdleColor()
    {
        _rend.material = currentActiveMaterial;
    }

    public void ResetPlatform()
    {
        platformBase.SetActive(true);
        instantiatedDeath = false;
        Destroy(deathSphereInst);
        elevated = true;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        _rend.enabled = true;
        _rend.material = idleMaterial;
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
            SwitchColor(40, 40);
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
            float lerp = Mathf.PingPong(Time.time * dangerLevel, 1);
            _rend.material.Lerp(idleMaterial, dangerMaterial, lerp);
            /*
            dangerMaterial = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.time * dangerLevel, 1));
            _rend.material.color = dangerMaterial;
            */
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
