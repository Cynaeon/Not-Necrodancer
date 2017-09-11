using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceToBeat : MonoBehaviour {

    public float shrinkSpeed;
    public float beatIntensity;

    private Vector3 startScale;

	void Start () {
        startScale = transform.localScale;
	}
	
	void Update () {
        if (transform.localScale.x > startScale.x)
        {
            float value = shrinkSpeed * Time.deltaTime;
            transform.localScale -= new Vector3(value, value, value);
        }

        if (transform.localScale.x < startScale.x)
            transform.localScale = startScale;
    }

    public void OnBeat()
    {
        transform.localScale *= beatIntensity;
    }
}
