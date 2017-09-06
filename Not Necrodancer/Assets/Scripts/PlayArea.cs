using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchColors()
    {
        foreach(Transform platform in transform)
        {
            if (platform.GetComponent<Platform>())
                platform.GetComponent<Platform>().SwitchColor();
        }
    }
}
