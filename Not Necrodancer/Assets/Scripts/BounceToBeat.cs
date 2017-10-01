using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BounceToBeat : MonoBehaviour {

    public float shrinkSpeed = 10;
    public float beatIntensity = 1.5f;

    private Vector3 startScale;
    private UnityAction bounceToBeat;

    void Start () {
        startScale = transform.localScale;
	}
	
	void Update () {
        if (transform.localScale.x > startScale.x)
        {
            float value = shrinkSpeed * Time.deltaTime;
            transform.localScale -= new Vector3(value, value, value);
        }

        if (transform.localScale.x < startScale.x)
            transform.localScale = startScale;
    }

    void Awake()
    {
        bounceToBeat = new UnityAction(OnBeat);
    }

    void OnEnable()
    {
        EventManager.StartListening("Bounce", bounceToBeat);
    }

    void OnDisable()
    {
        EventManager.StopListening("Bounce", bounceToBeat);
    }

    public void OnBeat()
    {
        transform.localScale *= beatIntensity;
    }
}
