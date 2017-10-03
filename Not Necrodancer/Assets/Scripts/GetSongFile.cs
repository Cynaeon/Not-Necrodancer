using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetSongFile : MonoBehaviour {

    public DirectoryInfo musicFolder;
    public WWW myClip;
    public string myPath;
    public AudioProcessor _ap;

    AudioSource _as;

	// Use this for initialization
	void Start () {
        _as = GetComponent<AudioSource>();
        myPath = "C:/Users/c5jtasal/Music/Custom music/ogg";
        musicFolder = new DirectoryInfo(myPath);
        myClip = new WWW("file:///" + musicFolder.GetFiles()[0].FullName);
        _as.clip = myClip.GetAudioClip(false, false);

    }
	
	// Update is called once per frame
	void Update () {
		if (!_as.isPlaying && _as.clip.loadState == AudioDataLoadState.Loaded)
        {
            _as.Play();
            _ap.enabled = true;
        }
	}
}


