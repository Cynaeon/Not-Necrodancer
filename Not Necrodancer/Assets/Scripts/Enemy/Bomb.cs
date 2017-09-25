using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy {

    public float speed;
    public ParticleSystem explosion;
    public Transform dangerTrigger;

    private bool toBeDestroyed;
    private float smallWaitTime = 0.2f;
	
	protected override void Update () {
        base.Update();

        if (toBeDestroyed)
        {
            smallWaitTime -= Time.deltaTime;
            if (smallWaitTime < 0)
            {
                Destroy(gameObject);
            }
        }

        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);

        if (transform.position.y < 0.3f && !toBeDestroyed)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            dangerTrigger.position = new Vector3(0, -100, 0);
            toBeDestroyed = true;
        }
	}
}
