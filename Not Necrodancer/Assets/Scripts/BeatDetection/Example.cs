﻿/*
 * Copyright (c) 2015 Allan Pichardo
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System;
using System.Collections.Generic;

public class Example : MonoBehaviour
{

    public float bpm;
    public float secondsToBeat;

    private float beatTime;
    private List<float> beats = new List<float>();

	void Start ()
	{
		//Select the instance of AudioProcessor and pass a reference
		//to this object
		AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
		processor.onBeat.AddListener (onOnbeatDetected);
		processor.onSpectrum.AddListener (onSpectrum);
	}

    private void Update()
    {
        beatTime += Time.deltaTime;
    }
    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    void onOnbeatDetected()
    {
        beats.Add(beatTime);
        beatTime = 0;
        
        float sum = 0;
        for (int i = 0; i < beats.Count; i++)
        {
            sum += beats[i];
        }
        secondsToBeat = sum / beats.Count;

        if (Camera.main.backgroundColor == Color.blue)
            Camera.main.backgroundColor = Color.cyan;
        else
            Camera.main.backgroundColor = Color.blue;
            
        //Debug.Log ("Beat!!!");
	}

	//This event will be called every frame while music is playing
	void onSpectrum (float[] spectrum)
	{
		//The spectrum is logarithmically averaged
		//to 12 bands

		for (int i = 0; i < spectrum.Length; ++i) {
			Vector3 start = new Vector3 (i, 0, 0);
			Vector3 end = new Vector3 (i, spectrum [i], 0);
			Debug.DrawLine (start, end);
		}
	}
}
