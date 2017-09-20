using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SongSelect : MonoBehaviour {

    public GameObject audioManager;
    public Transform songList;
    public float rollSpeed;
    public float maxRotation;
    public float minRotation;
    public GameObject eventSystem;
    public GameObject songInfo;
    public GameObject songButtons;
    public GameObject loading;
    public GameObject song_BTW;
    public GameObject song_CMM;
    public GameObject song_CG;

    private bool entered;
    private float step;
    private float targetRot;
    private GameObject songToBePlayed;
    private AudioManager am;
    private Canvas canvas;

	void Start () {
        eventSystem.SetActive(false);
        am = audioManager.GetComponent<AudioManager>();
	}
	
	void Update () {

        if (!entered)
        {
            Vector3 target = new Vector3(0, 0, 0);
            step += Time.deltaTime * rollSpeed;
            songList.eulerAngles = Vector3.Lerp(songList.rotation.eulerAngles, target, step);
            if (Vector3.Distance(songList.eulerAngles, target) < 0.2f)
            {
                eventSystem.SetActive(true);
                entered = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Up"))
            {
                if (targetRot > minRotation)
                {
                    targetRot -= 15;
                    step = 0;
                }
            }
            if (Input.GetButtonDown("Down"))
            {
                if (targetRot < maxRotation)
                {
                    targetRot += 15;
                    step = 0;
                }
            }

            Vector3 target = new Vector3(0, 0, targetRot);
            step += Time.deltaTime * rollSpeed * 20;
            songList.eulerAngles = Vector3.Lerp(songList.rotation.eulerAngles, target, step);

            if (songToBePlayed)
            {
                songButtons.SetActive(false);
                songInfo.SetActive(false);
                loading.SetActive(true);
                StartCoroutine(StartGame());
            }

            if (am.inGame)
                loading.SetActive(false);
        }
	}

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        if (songToBePlayed)
            Instantiate(songToBePlayed);
        songToBePlayed = null;
        am.StartGame();
    }

    public void PlayBTW()
    { 
        songToBePlayed = song_BTW;
    }

    public void PlayCMM()
    {
        songToBePlayed = song_CMM;
    }

    public void PlayCG()
    {
        songToBePlayed = song_CG;
    }
}
