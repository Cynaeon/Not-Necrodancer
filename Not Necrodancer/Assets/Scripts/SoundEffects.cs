using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {

    public AudioClip levelUp;
    public AudioClip beep;
    public AudioClip playerDeath;
    public AudioClip menuSelect;
    public AudioClip menuBack;
    public AudioClip menuValidate;

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

    internal void MenuSelect()
    {
        _as.PlayOneShot(menuSelect);
    }

    internal void MenuBack()
    {
        _as.PlayOneShot(menuBack);
    }

    internal void MenuValidate()
    {
        _as.PlayOneShot(menuValidate);
    }
}
