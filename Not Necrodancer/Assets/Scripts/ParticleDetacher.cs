using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDetacher : MonoBehaviour {

    private ParticleSystem ps;

	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        if (ps.transform.parent == null)
        {
            if (!ps.IsAlive())
                Destroy(gameObject);
        }
	}

    public void DetachParticles()
    {
        ps.transform.parent = null;
        var em = ps.emission;
        em.rateOverDistance = 0;
    }
}
