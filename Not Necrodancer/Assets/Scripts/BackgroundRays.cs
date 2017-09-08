using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRays : MonoBehaviour {

    public float spinningSpeed;

    private AudioManager audioManager;
    private ParticleSystem.ShapeModule ps;
    private bool rotating;

	void Start () {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ps = GetComponent<ParticleSystem>().shape;
	}
	
	void Update () {
        if (audioManager.level == 1)
            ps.arcSpread = 0.5f;
        if (audioManager.level == 2)
            ps.arcSpread = 0.25f;
        if (audioManager.level >= 3)
            transform.Rotate(Vector3.up * Time.deltaTime * spinningSpeed, Space.World);
        if (audioManager.level == 4)
            ps.arcSpread = 0.2f;
        if (audioManager.level == 5)
            ps.arcSpread = 0;
    }
}
