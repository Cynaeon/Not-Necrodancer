using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour {

    public GameObject trigger;
    public float minInterval;
    public float maxInterval;

    private float timeTillSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timeTillSpawn <= 0)
        {
            SpawnTrigger();
            timeTillSpawn = UnityEngine.Random.Range(minInterval, maxInterval);
        }
        else
        {
            timeTillSpawn -= Time.deltaTime;
        }
	}

    private void SpawnTrigger()
    {
        int rnd = UnityEngine.Random.Range(1, 5);
        Vector3 spawnPos = Vector3.zero;
        if (rnd == 2)
            spawnPos = new Vector3(8, 0, 8);
        else if (rnd == 3)
            spawnPos = new Vector3(-8, 0, 8);
        else if (rnd == 4)
            spawnPos = new Vector3(-8, 0, -8);
        else if (rnd == 5)
            spawnPos = new Vector3(8, 0, -8);
        Instantiate(trigger, spawnPos, Quaternion.identity);
    }
}
