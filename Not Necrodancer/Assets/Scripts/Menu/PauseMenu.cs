using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MenuScreen
{
    public AudioManager audioManager;
    public MenuScreen songSelect;

    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Pause"))
            Resume();
    }

    public void Resume()
    {
        audioManager.UnpauseGame();
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        audioManager.ResetGame();
        audioManager.StartGame();
        gameObject.SetActive(false);
    }

    public void ToMenu()
    {
        audioManager.ResetGame();
        songSelect.gameObject.SetActive(true);
        songSelect.enabled = true;
        gameObject.SetActive(false);
    }
}