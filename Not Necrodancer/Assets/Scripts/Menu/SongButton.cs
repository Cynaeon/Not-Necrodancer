using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SongButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public SongData songData;

    public Text songInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (songData)
            songInfo.text = "Tempo: " + songData.songSpeed.ToString() + "\nLength: " + songData.songLenght.ToString();
        else
            songInfo.text = "Tempo: ???\nLength: ???";
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (songData)
            songInfo.text = "Tempo: " + songData.songSpeed.ToString() + "\nLength: " + songData.songLenght.ToString();
        else
            songInfo.text = "Tempo: ???\nLength: ???";
    }
}
