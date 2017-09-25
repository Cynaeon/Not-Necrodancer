using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SongSelect : MonoBehaviour {

    public GameObject audioManager;
    public Transform songList;
    public float rollSpeed;
    public float maxRotation;
    public float minRotation;
    public EventSystem eventSystem;
    public GameObject firstSelectedMenu;

    public GameObject songInfo;
    public GameObject songButtons;
    public GameObject loading;
    public GameObject smoke;
    public GameObject waveTrigger;
    public float waveIntervalMin;
    public float waveIntervalMax;
    public GameObject song_BTW;
    public GameObject song_CMM;
    public GameObject song_CG;

    private Quaternion songListStartRot;
    private Vector3 songListStartPos;
    private bool entered;
    private float step;
    private float targetRot;
    private GameObject songObject;
    private GameObject songToBePlayed;
    private AudioManager am;
    private Canvas canvas;

	void Start () {
        songListStartPos = songList.position;
        songListStartRot = songList.rotation;
        smoke.SetActive(true);
        eventSystem.enabled = false;
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
                eventSystem.enabled = true;
                eventSystem.SetSelectedGameObject(firstSelectedMenu);
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
                smoke.SetActive(false);
                StartCoroutine(StartGame());
            }

            if (am.inGame)
                loading.SetActive(false);
        }

        if (eventSystem.currentSelectedGameObject == null)
        {
            targetRot = 0;
            eventSystem.SetSelectedGameObject(firstSelectedMenu);
        }
	}

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        if (songToBePlayed)
        {
            songObject = Instantiate(songToBePlayed);
        }
        songToBePlayed = null;
        am.StartGame();
    }

    public void PlayAgain()
    {
        am.ResetGame();
        songToBePlayed = songObject;
        Destroy(songObject);
    }

    public void ToMenu()
    {
        am.ResetGame();
        Destroy(songObject);
        targetRot = 0;
        entered = false;
        songList.position = songListStartPos;
        songList.rotation = songListStartRot;
        
        songButtons.SetActive(true);
        songInfo.SetActive(true);
        smoke.SetActive(true);
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
