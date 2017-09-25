using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {

    public AudioClip levelUp;
    public AudioClip beep;
    public AudioClip playerDeath;

    private AudioSource _as;

    void Start () {
        _as = GetComponent<AudioSource>();
	}

    internal void Beep()
    {
        _as.PlayOneShot(beep);
    }

    internal void LevelUp()
    {
        _as.PlayOneShot(levelUp);
    }

    internal void PlayerDeath()
    {
        _as.PlayOneShot(playerDeath);
    }
}
