using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    public Color onColor;
    public Color offColor;
    public float transitionSpeed;

    private float step;
    private bool lightsOn;
    private Light _light;

	void Start () {
        _light = GetComponent<Light>();
	}

	void Update () {
        if (lightsOn)
        {
            if (step < 1)
                step += Time.deltaTime * transitionSpeed;
            else
                step = 1;
        }
        else
        {
            if (step < 0)
                step -= Time.deltaTime * transitionSpeed;
            else
                step = 0;
        }

        if (lightsOn)
            _light.color = Color.Lerp(offColor, onColor, step);
	}

    public void ToggleLights()
    {
        if (lightsOn)
            lightsOn = false;
        else
            lightsOn = true;   
    }

    public void ToggleOn()
    {
        lightsOn = true;
    }

    public void ToggleOff()
    {
        lightsOn = false;
    }
}
