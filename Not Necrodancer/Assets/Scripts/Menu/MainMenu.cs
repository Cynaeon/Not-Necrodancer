﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MenuScreen {

    public GameObject menuButtons;
    public MenuScreen customizeButtons;
    public MenuScreen songSelectButtons;
    public MenuScreen optionsButtons;

    protected override void Start()
    {
        base.Start();
        menuButtons.SetActive(true);
    }

    public void Play()
    {
        songSelectButtons.gameObject.SetActive(true);
        songSelectButtons.enabled = true;
        gameObject.SetActive(false);
    }

    public void Customize()
    {
        customizeButtons.gameObject.SetActive(true);
        customizeButtons.enabled = true;
        gameObject.SetActive(false);
    }

    public void Options()
    {
        optionsButtons.gameObject.SetActive(true);
        optionsButtons.enabled = true;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
