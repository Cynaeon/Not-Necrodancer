using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDirection : MonoBehaviour {

    public Vector3 dir;
    public float speed;

	void Update () {
        if (transform.position.y < 20) 
            transform.position += dir * speed;
	}
}
