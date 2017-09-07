using UnityEngine;

public class IsAlive : MonoBehaviour {

    private ParticleSystem ps;

	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
	}
}
