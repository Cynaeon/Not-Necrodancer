using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour {

    public float rollSpeed;
    public float colorShiftSpeed;
    public Color[] colorGradient;

    private int currentColorIndex;
    private float currentShiftTime;
    private float currentRoll;
    private Renderer _rend;

	void Start () {
        _rend = GetComponent<Renderer>();
	}
	
	void Update () {
        currentRoll -= rollSpeed * Time.deltaTime;
        if (currentRoll <= -1)
            currentRoll = 0;
        _rend.material.SetTextureOffset("_MainTex", new Vector2(currentRoll, 0));

        int nextColor = currentColorIndex + 1;
        if (nextColor >= colorGradient.Length)
            nextColor = 0;
        currentShiftTime += Time.deltaTime * colorShiftSpeed;
        Color color = Color.Lerp(colorGradient[currentColorIndex], colorGradient[nextColor], currentShiftTime);
        _rend.material.SetColor("_EmissionColor", color);
        if (currentShiftTime > 1)
        {
            currentColorIndex++;
            if (currentColorIndex >= colorGradient.Length)
                currentColorIndex = 0;
            currentShiftTime = 0;
        }
    }
}
