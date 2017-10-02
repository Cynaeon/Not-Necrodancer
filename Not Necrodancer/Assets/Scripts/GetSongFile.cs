using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetSongFile : MonoBehaviour {

    DirectoryInfo musicFolder;
    WWW myClip;
    string myPath;

    AudioSource _as;

	// Use this for initialization
	void Start () {
        myPath = "C:/Users/c5jtasal/Music/Custom music";
        musicFolder = new DirectoryInfo(myPath);
        myClip = new WWW("file:///" + musicFolder.GetFiles()[0].FullName);
        _as.clip = myClip.GetAudioClip(false, false);

    }
	
	// Update is called once per frame
	void Update () {
		if (!_as.isPlaying)
        {
            
        }
	}
}


