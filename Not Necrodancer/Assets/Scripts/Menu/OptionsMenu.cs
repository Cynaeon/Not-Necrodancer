using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MenuScreen {

    public MenuScreen mainMenu;

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

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("LastLevelFinished", 0);
        PlayerPrefs.SetInt("LastLevelUnlocked", 1);
        PlayerPrefs.Save();
        GameProgress gp = FindObjectOfType<GameProgress>();
        gp.RefreshProgress();
        soundEffects.MenuValidate();
    }
}
