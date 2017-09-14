using UnityEngine;

public class IsAlive : MonoBehaviour {

    public float lifetime;

    private ParticleSystem ps;

	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if (ps)
        {
            if (transform.parent == null)
            {
                lifetime -= Time.deltaTime;
                if (lifetime < 0)
                    Destroy(gameObject);
            }
                if (!ps.IsAlive())
                Destroy(gameObject);
        }
	}
}
