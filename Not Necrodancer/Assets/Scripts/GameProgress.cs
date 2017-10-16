using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour {

    public bool[] levelUnlocked = new bool[9];
    public bool[] levelFinished = new bool[9];

    internal string[] reqs = new string[9];

    void Start () {
        RefreshProgress();
        reqs[0] = "";
        reqs[1] = "";
        reqs[2] = "completing Born This Way";
        reqs[3] = "completing Call Me Maybe";
	}

	void Update () {
		
	}

    internal void LevelFinished(int levelNumber)
    {
        levelFinished[levelNumber] = true;
        int lastLevel = SetLastLevel(levelFinished);
        PlayerPrefs.SetInt("LastLevelFinished", lastLevel);
        PlayerPrefs.Save();
    }

    internal void LevelUnlocked(int levelNumber)
    {
        levelUnlocked[levelNumber] = true;
        int lastLevel = SetLastLevel(levelUnlocked);
        PlayerPrefs.SetInt("LastLevelUnlocked", lastLevel);
        PlayerPrefs.Save();
    }

    private int SetLastLevel(bool[] array)
    {
        int lastLevel = 0;
        for (int i = 0; i < levelFinished.Length; i++)
        {
            if (array[i] == true)
                lastLevel = i;
        }
        return lastLevel;
    }

    private void GetLastLevel(int lastLevel, bool[] array)
    {
        for (int i = 0; i <= lastLevel; i++)
        {
            array[i] = true;
        }
    }

    internal void RefreshProgress()
    {
        GetLastLevel(PlayerPrefs.GetInt("LastLevelFinished"), levelFinished);
        GetLastLevel(PlayerPrefs.GetInt("LastLevelUnlocked"), levelUnlocked);
    }
}
