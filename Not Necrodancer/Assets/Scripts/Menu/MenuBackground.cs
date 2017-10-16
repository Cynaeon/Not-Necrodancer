using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour {

    private Image _image;

	void Start () {
        _image = GetComponent<Image>();
	}

	void Update () {
        MenuScreen activeMenu = FindObjectOfType<MenuScreen>();
        if (activeMenu)
            _image.enabled = true;
        else
            _image.enabled = false;
	}
}
