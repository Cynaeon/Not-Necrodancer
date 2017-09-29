using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEndMenu : MenuScreen {

    public AudioManager audioManager;
    public MenuScreen songSelect;

    public void Continue()
    {
        audioManager.ResetGame();
        songSelect.gameObject.SetActive(true);
        songSelect.enabled = true;
        gameObject.SetActive(false);
    }

    public void Restart()
    { 
        audioManager.ResetGame();
        audioManager.StartGame();
        gameObject.SetActive(false);
    }
}
