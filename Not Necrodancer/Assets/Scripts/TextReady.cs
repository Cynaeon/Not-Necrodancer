using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextReady : MonoBehaviour {

    public float scaleSpeed;

    private float y;
    private Text readyText;
    private AudioManager audioManager;

	void Start () {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        transform.localScale = new Vector3(1, 0, 1);
	}
	
	void Update () {
        if (audioManager.timeTillSongStart - Time.time < 0.5f)
        {
            y -= Time.deltaTime * scaleSpeed;
            if (y > 0)
            {
                transform.localScale = new Vector3(1, y, 1);
            }
            else
                gameObject.SetActive(false);
        }
        else if (audioManager.timeTillSongStart - Time.time < 2.5f)
        {
            y += Time.deltaTime * scaleSpeed;
            if (y < 1)
            {
                transform.localScale = new Vector3(1, y, 1);
            }
            else
            {
                y = 1;
                transform.localScale = new Vector3(1, y, 1);
            }
        }
	}
}
