﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour {

    public AudioClip MusicClip;

    public AudioSource MusicSource;
	// Use this for initialization
	void Start () {
        MusicSource.clip = MusicClip;
        MusicSource.time = 33;
        MusicSource.Play(0);
	}
	
	// Update is called once per frame
	void Update () {

		if (MusicSource.time > 57.5)
		{
			MusicSource.Stop();
		}
		
	}
}
