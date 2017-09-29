using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

    public AudioClip ambient;

    private AudioClip instance;
    private AudioSource _as;
    private float clipTime;
    private AudioManager _am;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        _as = GetComponent<AudioSource>();
        //_as.PlayOneShot(ambient);
        SceneManager.LoadSceneAsync(1);
    }
	
	// Update is called once per frame
	void Update () { 
        if (!_am)
        {
            _am = (AudioManager)FindObjectOfType(typeof(AudioManager));
        }
       
        else
        {
            if (_am.inGame)
                _as.volume = 0;
            else
                _as.volume = 1;
        }


        /*
        clipTime += Time.deltaTime;
        print(clipTime);
        if (clipTime > ambient.length - 0.5f)
        {
            _as.PlayOneShot(ambient);
            clipTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.T))
    
            _as.PlayOneShot(ambient);
            */
    }

}
