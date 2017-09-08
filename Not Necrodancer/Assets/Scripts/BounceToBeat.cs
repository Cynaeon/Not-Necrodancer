using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceToBeat : MonoBehaviour {

    public float shrinkSpeed;

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
    }

    public void OnBeat()
    {
        transform.localScale *= 1.5f;
    }
}
