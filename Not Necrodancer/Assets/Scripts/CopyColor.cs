using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyColor : MonoBehaviour {

    public GameObject target;

    private Renderer targetRend;
    private Light light;

	// Use this for initialization
	void Start () {
        targetRend = target.GetComponent<Renderer>();
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        light.color = targetRend.material.color;
	}
}
