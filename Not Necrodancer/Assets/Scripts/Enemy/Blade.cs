using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float maxSpeed;
    public float destroyAtY;
    public GameObject ps;

    private Transform[] children;

    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        speed *= 1.02f;

        if (transform.position.y < destroyAtY)
        {
            if (ps)
                ps.transform.parent = null;
            Destroy(gameObject);
        }

    }
}
