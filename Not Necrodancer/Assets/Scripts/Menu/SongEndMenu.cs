using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEndMenu : MenuScreen {

    public AudioManager audioManager;

    public void Continue()
    {

    }

    public void Restart()
    { 
        audioManager.ResetGame();
        audioManager.StartGame();
        gameObject.SetActive(false);
    }
}
