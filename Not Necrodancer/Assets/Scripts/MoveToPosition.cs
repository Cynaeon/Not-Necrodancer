using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour {

    public Vector3 end;
    public float speed;

    private Vector3 start;
    private float step;

	void Start () {
        start = transform.position;
        end += start;
	}
	
	void Update () {
        step += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(start, end, step);
	}
}
