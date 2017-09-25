using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour {

    public int beatsToDrop = 6;
    public float dropSpeed = 15;

    private AudioManager _audioManager;
    private float y;

	void Start () {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        y = transform.position.y;
	}
	
	protected virtual void Update () {
        if (_audioManager.onBeat)
        {
            beatsToDrop--;
        }
		if (beatsToDrop <= 1)
        {
            transform.position += Vector3.down * (Time.deltaTime * (y / _audioManager.secondsToBeat));
        }
        if (transform.position.y < -50)
            Destroy(gameObject);
	}
}
