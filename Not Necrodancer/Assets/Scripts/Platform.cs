using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed;
    public float waveSpan;

    private bool wave;
    private float startY;
    private float waveTime;

	void Start () {
        startY = transform.position.y;
	}
	
	void Update () {

        if (wave)
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
        print(waveTime);
	}

    void Wave()
    {
        wave = true;
        waveTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WaveTrigger")
            Wave();
    }
}
