using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorizer : MonoBehaviour {
    public ParticleSystem playerGlow;
    public ParticleSystem playerMoveTrail;
    public Light playerLight;
    public Light[] stageLight;
    public Material platformMaterial;
    public Material platformBaseMaterial;
    public ParticleSystem smoke;
    public ParticleSystem ambient;
    public ParticleSystem backgroundRays;

    public Color playerColor;
    public Color stageColor;
    public Color backgroundColor;

	void Update () {
        var main = playerGlow.main;
        main.startColor = playerColor;
        main = playerMoveTrail.main;
        main.startColor = playerColor;
        playerLight.color = playerColor;

        platformMaterial.SetColor("_EmissionColor", new Color(stageColor.r / 4, stageColor.g / 4, stageColor.b / 4));
        platformBaseMaterial.SetColor("_EmissionColor", new Color(stageColor.r / 8, stageColor.g / 8, stageColor.b / 8));
        foreach (Light light in stageLight) 
            light.color = stageColor;
        main = smoke.main;
        main.startColor = new Color(stageColor.r, stageColor.g, stageColor.b, 0.08f);
        main = ambient.main;
        main.startColor = stageColor;

        main = backgroundRays.main;
        main.startColor = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0.2f);
    }
}
