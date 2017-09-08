﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {

    public GameObject collectable;
    public GameObject levelUp;
    public GameObject[] enemies;
    public float collectableSpawnInterval;
    public float enemySpawnInterval;
    public float collectableSpawnHeight;
    public float enemySpawnHeight;
    public int areaX;
    public int areaY;

    public float enemyIntervalMultiplier = 1;
    private float currentCollectableInterval;
    private float currentEnemyInterval;

	void Start () {
        currentCollectableInterval = collectableSpawnInterval;
        currentEnemyInterval = enemySpawnInterval;
	}
	
	void Update () {
        currentCollectableInterval -= Time.deltaTime;
        currentEnemyInterval -= Time.deltaTime * enemyIntervalMultiplier;

        if (currentCollectableInterval < 0)
        {
            SpawnCollectable();
            currentCollectableInterval = collectableSpawnInterval;
        }

        if (currentEnemyInterval < 0)
        {
            SpawnEnemy();
            currentEnemyInterval = enemySpawnInterval;
        }
	}

    private void SpawnEnemy()
    {
        int numberOfEnemies = 1;
        int rnd = UnityEngine.Random.Range(0, 11);
        if (rnd >= 9)
        {
            numberOfEnemies = 2;
        }

        while (numberOfEnemies > 0)
        {
            int enemyNumber = UnityEngine.Random.Range(0, enemies.Length);
            int x = (UnityEngine.Random.Range(-areaX / 2, (areaX / 2) + 1)) * 2;
            int z = (UnityEngine.Random.Range(-areaY / 2, (areaY / 2) + 1)) * 2;
            if (enemies[enemyNumber].name == "Bomb")
            {
                x += 1;
                z += 1;
            }
            Vector3 pos = new Vector3(x, enemySpawnHeight, z);
            int rotY = UnityEngine.Random.Range(1, 4) * 90;
            Vector3 rot = new Vector3(0, rotY, 0);
            Instantiate(enemies[enemyNumber], pos, Quaternion.Euler(rot));
            numberOfEnemies--;
        }
    }

    public void SpawnLevelUp()
    {
        int x = (UnityEngine.Random.Range(-areaX / 2, (areaX / 2) + 1)) * 2;
        int z = (UnityEngine.Random.Range(-areaY / 2, (areaY / 2) + 1)) * 2;
        Vector3 pos = new Vector3(x, collectableSpawnHeight, z);
        Instantiate(levelUp, pos, Quaternion.identity);
    }

    void SpawnCollectable()
    {
        int x = (UnityEngine.Random.Range(-areaX / 2, (areaX / 2) + 1)) * 2;
        int z = (UnityEngine.Random.Range(-areaY / 2, (areaY / 2) + 1)) * 2;
        Vector3 pos = new Vector3(x, collectableSpawnHeight, z);
        Instantiate(collectable, pos, Quaternion.identity);
    }

    public void SwitchColors()
    {
        foreach(Transform platform in transform)
        {
            if (platform.GetComponent<Platform>())
                platform.GetComponent<Platform>().SwitchColor();
        }
    }
}
