using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEndMenu : MenuScreen {

    public AudioManager audioManager;
    public MenuScreen songSelect;

    public void Continue()
    {
        audioManager.ResetGame(true);
        songSelect.gameObject.SetActive(true);
        songSelect.enabled = true;
        gameObject.SetActive(false);
    }

    public void Restart()
    { 
        audioManager.ResetGame(false);
        audioManager.StartGame();
        gameObject.SetActive(false);
    }
}
