using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingTrigger : MonoBehaviour {

    public float speed;
    public float duration;

	void Update () {
        transform.localScale += new Vector3(speed, speed, speed);

        duration -= Time.deltaTime;

        if (duration < 0)
            Destroy(gameObject);
    }
}
