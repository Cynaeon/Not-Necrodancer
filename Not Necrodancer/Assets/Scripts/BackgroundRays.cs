using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRays : MonoBehaviour {

    public float spinningSpeed;
    public float colorShiftSpeed;
    public Color[] colorGradient = new Color[6];
    public float beatIntensityLimiter;

    private bool limiterSet;
    private int currentColor;
    private float currentShiftTime;
    private AudioManager audioManager;
    private ParticleSystem.ShapeModule ps_shape;
    private ParticleSystem ps;
    private bool rotating;

	void Start () {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = colorGradient[0];
        ps_shape = GetComponent<ParticleSystem>().shape;
	}
	
	void Update () {
        //ShiftColor();
        if (audioManager.level == 1)
            ps_shape.arcSpread = 0.5f;
        if (audioManager.level == 2)
            ps_shape.arcSpread = 0.25f;
        if (audioManager.level >= 3)
            transform.Rotate(Vector3.up * Time.deltaTime * spinningSpeed, Space.World);
        if (audioManager.level == 4)
        {
            if (limiterSet)
            {
                GetComponent<BounceToBeat>().beatIntensity += beatIntensityLimiter;
                limiterSet = false;
            }
            ps_shape.arcSpread = 0.2f;
        }
        if (audioManager.level == 5)
        {
            if (!limiterSet)
            {
                GetComponent<BounceToBeat>().beatIntensity -= beatIntensityLimiter;
                limiterSet = true;
            }
            ps_shape.arcSpread = 0;
        }
    }

    private void ShiftColor()
    {
        int nextColor = currentColor + 1;
        if (nextColor >= colorGradient.Length)
            nextColor = 0;
        currentShiftTime += Time.deltaTime * colorShiftSpeed;
        var main = ps.main;
        main.startColor = Color.Lerp(colorGradient[currentColor], colorGradient[nextColor], currentShiftTime);
        if (currentShiftTime > 1)
        {
            currentColor++;
            if (currentColor >= colorGradient.Length )
                currentColor = 0;
            currentShiftTime = 0;
        }
    }
}
