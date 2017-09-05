using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;

    private Vector3 newPosition;

	// Use this for initialization
	void Start () {
        newPosition = transform.position;
	}

    void Update()
    {
        if (transform.position == newPosition)
        {
            if (Input.GetButtonDown("Right"))
                newPosition = transform.position + new Vector3(2, 0, 0);
            if (Input.GetButtonDown("Left"))
                newPosition = transform.position + new Vector3(-2, 0, 0);
            if (Input.GetButtonDown("Up"))
                newPosition = transform.position + new Vector3(0, 0, 2);
            if (Input.GetButtonDown("Down"))
                newPosition = transform.position + new Vector3(0, 0, -2);
        }
        transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);

        /*
        if (Input.GetButtonDown("Right"))
            newPosition = newPosition + new Vector3(2, transform.position.y, transform.position.z);
        else if (Input.GetButtonDown("Left"))
            newPosition = newPosition + new Vector3(-2, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);
        */
    }
}
