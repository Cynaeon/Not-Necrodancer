using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour {

    public float lifetime;

    private Text _text;
    private float currentLifetime;
    private float startY;

	void Start () {
        startY = transform.position.y;
        _text = GetComponent<Text>();
        _text.enabled = false;
	}
	
	void Update () {
        currentLifetime += Time.deltaTime;
        if (currentLifetime > lifetime)
        {
            _text.enabled = false;
            currentLifetime = 0;
        }
	}

    public void Activate()
    {
        currentLifetime = 0;
        _text.enabled = true;
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
}
