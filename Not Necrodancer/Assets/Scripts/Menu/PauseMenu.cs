using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MenuScreen
{
    public AudioManager audioManager;

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
        gameObject.SetActive(false);
    }
}