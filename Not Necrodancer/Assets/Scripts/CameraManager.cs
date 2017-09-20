using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraManager : MonoBehaviour {

    public float beatFlashSpeed;
    public float levelFlashSpeed;
    public Color beatFlashColor;
    public Color levelFlashColor;
    public Color deathColor;
    public float shakeSpeed;
    public float shakeSpan;

    private PostProcessingBehaviour screenBlur;
    private Camera cam;
    private bool screenShake;
    private float currentSpan;
    private float startX;
    public Vector3 startPosition;
    public Vector3 startRotation;
    private Color startColor;
    private float step;
    private bool levelUpFlashing;
    private bool deathFlashing;

	void Start () {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        cam = GetComponent<Camera>();
        startColor = cam.backgroundColor;
        startX = transform.position.x;
        currentSpan = shakeSpan;
        screenBlur = GetComponent<PostProcessingBehaviour>();
	}

    void Update()
    {
        if (screenShake)
        {
            float x = startX + Mathf.Sin(Time.time * shakeSpeed) * currentSpan / 2;
            currentSpan -= Time.deltaTime * 2;
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            if (currentSpan < 0)
            {
                screenShake = false;
                currentSpan = shakeSpan;
            } 
        }

        if (deathFlashing)
        {
            if (step > 0)
                step -= Time.deltaTime * levelFlashSpeed;
            else
                deathFlashing = false;
            cam.backgroundColor = Color.Lerp(startColor, deathColor, step);
        }

        else if (levelUpFlashing)
        {
            if (step > 0)
                step -= Time.deltaTime * levelFlashSpeed;
            else
                levelUpFlashing = false;
            cam.backgroundColor = Color.Lerp(startColor, levelFlashColor, step);
        }
        else
        {
            if (step > 0)
                step -= Time.deltaTime * beatFlashSpeed;
            cam.backgroundColor = Color.Lerp(startColor, beatFlashColor, step);
        }
    }

    public void SetToGamePosition()
    {
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

    public void ScreenShake()
    {
        screenShake = true;
    }

    public void BlurScreen()
    {
        screenBlur.enabled = true;
    }

    public void UnblurScreen()
    {
        screenBlur.enabled = false;
    }

    public void BeatFlash()
    {
        if (!levelUpFlashing && !deathFlashing)
            step = 1;
    }

    public void DeathFlash()
    {
        step = 1;
        deathFlashing = true;
    }

    public void LevelUpFlash()
    {
        step = 1;
        levelUpFlashing = true;
    }
}
