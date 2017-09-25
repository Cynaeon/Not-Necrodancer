using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenu : MenuScreen {

    public Color[] colors;
    public Colorizer colorizer;
    public GameObject backgroundRays;
    public MenuScreen mainMenu;
    public Button playerButton;
    public Button stageButton;
    public Button backgroundButton;

    private int player;
    private int stage;
    private int background;

    protected override void Start()
    {
        base.Start();
        SetButtonColor(playerButton, player);
        SetButtonColor(stageButton, stage);
        SetButtonColor(backgroundButton, background);
    }

    private void SetButtonColor(Button button, int colorIndex)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = colors[colorIndex] / 4;
        cb.highlightedColor = colors[colorIndex] / 2;
        cb.pressedColor = colors[colorIndex];
        button.colors = cb;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
            ToMainMenu();
    }

    private void ToMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        mainMenu.enabled = true;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        backgroundRays.SetActive(true);
    }

    private void OnDisable()
    {
        backgroundRays.SetActive(false);
    }

    public void PlayerColor()
    {
        player++;
        if (player >= colors.Length)
            player = 0;
        colorizer.playerColor = colors[player];
        SetButtonColor(playerButton, player);
    }

    public void StageColor()
    {
        stage++;
        if (stage >= colors.Length)
            stage = 0;
        colorizer.stageColor = colors[stage];
        SetButtonColor(stageButton, stage);
    }

    public void BackgroundColor()
    {
        background++;
        if (background >= colors.Length)
            background = 0;
        colorizer.backgroundColor = colors[background];
        SetButtonColor(backgroundButton, background);
    }
}
