using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEndTrigger : MonoBehaviour {

    public float cycleTime;
    public float speed;

    private enum CycleType
    {
        None = 0,
        ExpandCenter = 1,
        ExpandCorner = 2,
    }

    private CycleType cycleType;
    private float currentCycleTime;
    private bool cycleReady;
	
	void Update () {

        if (cycleType == CycleType.None)
            RollANewCycle();
        else if (cycleType == CycleType.ExpandCenter)
            ExpandFromCenter();
        else if (cycleType == CycleType.ExpandCorner)
            ExpandFromCorner();
    }

    private void ExpandFromCorner()
    {
        if (cycleReady)
        {
            transform.localScale += Vector3.one * 0.5f;
            currentCycleTime += Time.deltaTime;
            if (currentCycleTime > cycleTime)
                cycleType = CycleType.None;
        }
        else
        {
            int rnd = UnityEngine.Random.Range(0, 2);
            if (rnd == 0) 
                transform.position = new Vector3(7, 0, 7);
            else
                transform.position = new Vector3(-7, 0, -7);
            transform.localScale = Vector3.one;
            currentCycleTime = 0;
            cycleReady = true;
        }
    }

    private void RollANewCycle()
    {
        int max = Enum.GetValues(typeof(CycleType)).Length;
        int rnd = UnityEngine.Random.Range(1, max);
        cycleType = (CycleType)rnd;
        cycleReady = false;
    }

    void ExpandFromCenter()
    {
        if (cycleReady)
        {
            transform.localScale += Vector3.one * 0.5f;
            currentCycleTime += Time.deltaTime;
            if (currentCycleTime > cycleTime) 
                cycleType = CycleType.None;
        }
        else
        {
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
            currentCycleTime = 0;
            cycleReady = true;
        }
    }
}
