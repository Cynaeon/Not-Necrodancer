using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

    public float speed;
    public float acceleration;
    public float maxSpeed;
	
	void Update () {
        print(transform.eulerAngles.x);
        if (transform.eulerAngles.x > 15)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * speed);
            if (speed < maxSpeed)
            {
                speed *= acceleration;
            }
        }

        else 
        {
            speed = 1;
            speed *= acceleration;
            transform.position -= new Vector3(0, 0.1f + speed, 0);
        }

        if (transform.position.y < -150)
            Destroy(gameObject);
    }
}
