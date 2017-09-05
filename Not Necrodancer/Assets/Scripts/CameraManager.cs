using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float flashSpeed;
    public Color flashColor;

    private Color startColor;
    private float step;
    

	void Start () {
        startColor = GetComponent<Camera>().backgroundColor;
	}
	
	void Update () {
        if (step > 0)
            step -= Time.deltaTime * flashSpeed;
        GetComponent<Camera>().backgroundColor = Color.Lerp(startColor, flashColor, step);
	}

    public void FlashBackground()
    {
        step = 1;
    }
}
