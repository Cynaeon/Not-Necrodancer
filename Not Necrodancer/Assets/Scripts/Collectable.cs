using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public float speed;
    public float endY;
    public float lifeTime;

	void Start () {
		
	}
	
	void Update () {
        if (transform.position.y > endY)
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        else
            transform.position = new Vector3(transform.position.x, endY, transform.position.z);

        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);
	}
}
