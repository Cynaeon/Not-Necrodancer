using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SongButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public int stageNumber;
    public SongData songData;
    public Text songInfo;
    public Text songTitle;
    public Color unlockedColor;
    public Color lockedColor;

    internal bool unlocked;
    internal bool played;
    internal bool cleared;

    private Button _button;
    private GameProgress _gp;

    public void Start()
    {
        _gp = FindObjectOfType<GameProgress>();
        _button = GetComponent<Button>();
    }

    public void Update()
    {
        unlocked = _gp.levelUnlocked[stageNumber];
        played = _gp.levelFinished[stageNumber];
        if (unlocked)
        {
            ColorBlock cb = _button.colors;
            cb.normalColor = unlockedColor;
            songTitle.color = unlockedColor;
            _button.colors = cb;
        }
        else
        {
            ColorBlock cb = _button.colors;
            cb.normalColor = lockedColor;
            songTitle.color = lockedColor;
            _button.colors = cb;
        }
        if (played)
            songTitle.text = songData.songName;
        else
            songTitle.text = "???";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplaySongInfo();
    }

    public void OnSelect(BaseEventData eventData)
    {
        DisplaySongInfo();
    }

    public void DisplaySongInfo()
    {
        if (songData)
        {
            if (unlocked)
                songInfo.text = "Tempo: " + songData.songSpeed.ToString() + "\nLength: " + songData.songLenght.ToString();
            else
                songInfo.text = "Tempo: ???\nLength: ???";
        }
        else
            songInfo.text = "Tempo: ???\nLength: ???";
    }
}
