using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{

    public float speed;
    public float acceleration;
    public float maxSpeed;

    private Transform[] children;

    private void Awake()
    {
        
    }

    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        speed *= 1.02f;

        if (transform.position.y < -150)
            Destroy(gameObject);

        /*
        if (transform.position.y > 0)
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        
        else
        {
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
        */
    }
}
