using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed;
    public float waveSpan;
    public float spinSpeed;
    public float spinSpeedFalloff;
    public float spinDuration;
    public Material[] materials;
    public int variant;

    enum State
    {
        descended = 0,
        descending = 1,
        elevated = 2
    }

    private State state;

    private bool elevated;
    private bool descending;
    private Vector3 elevatedPos;
    private Vector3 endPos;
    private float descentTime;
    private float currentSpinSpeed;
    private float spinFalloffMultiplier = 1;
    private Renderer _rend;
    private bool wave;
    private bool spinning;
    private float startY;
    private float waveTime;

	void Start () {
        currentSpinSpeed = spinSpeed;
        _rend = GetComponent<Renderer>();
        if (variant == 1)
        {
            _rend.material = materials[0];
        }
        else
        {
            _rend.material = materials[1];
        }
        elevatedPos = transform.position;
        endPos = new Vector3(transform.position.x, 0, transform.position.z);
        if (transform.position.y > 0)
            state = State.elevated;
        else
            state = State.descended;
	}
	
	void Update () {

        if (spinning)
        {
            transform.Rotate(Vector3.right * currentSpinSpeed * Time.deltaTime);

            if (currentSpinSpeed > 50)
            {
                currentSpinSpeed -= spinSpeedFalloff * Time.deltaTime * spinFalloffMultiplier;
                spinFalloffMultiplier += 0.4f;    
            }

            else
            {
                if ((transform.eulerAngles.x > -1 && transform.eulerAngles.x < 1))
                {
                    print("öö");
                    transform.eulerAngles = Vector3.zero;
                    spinning = false;
                    currentSpinSpeed = spinSpeed;
                    spinFalloffMultiplier = 1;
                }
            }
        }

        else if (wave && state == State.descended)
        {
            waveTime = waveTime + Time.deltaTime;
            float y = startY + Mathf.Sin(waveTime * speed) * waveSpan / 2;
            
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            
            if (transform.position.y < startY)
            {
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                wave = false;
            }
        }
       
        else if (state == State.descending)
        {
            descentTime = descentTime + Time.deltaTime;
            transform.position = Vector3.Lerp(elevatedPos, endPos, descentTime);
            
            if (transform.position.y <= 0.01f)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                state = State.descended;
            }
        }
	}

    void Wave()
    {
        wave = true;
        waveTime = 0;
        startY = transform.position.y;
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
        if (_rend.sharedMaterial == materials[0])
        {
            _rend.material = materials[1];
        }
        else
        {
            _rend.material = materials[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WaveTrigger")
            Wave();
        if (other.tag == "DescentTrigger")
        {
            if (state == State.elevated)
                state = State.descending;
        }
        if (other.tag == "Blade")
            Spin();
    }
}
