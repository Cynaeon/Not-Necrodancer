using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public float speed;
    public float endY;
    public float lifeTime;

    private Renderer _rend;
    private Color startColor;
    private Color invisible;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        startColor = _rend.material.color;
        invisible = Color.clear;
    }

    void Update () {
        if (transform.position.y > endY)
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        else
            transform.position = new Vector3(transform.position.x, endY, transform.position.z);

        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);
        else if (lifeTime < 2)
        {
            _rend.material.color = Color.Lerp(startColor, invisible, Mathf.PingPong(Time.time * 6, 1));
        }
	}
}
