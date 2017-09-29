using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {

    public SoundEffects _sf;

	// Use this for initialization
	void Start () {
        _sf = (SoundEffects) FindObjectOfType(typeof(SoundEffects));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect(BaseEventData eventData)
    {
        _sf.MenuSelect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sf.MenuSelect();
    }
}
