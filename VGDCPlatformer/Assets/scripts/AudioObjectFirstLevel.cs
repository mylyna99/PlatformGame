using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObjectFirstLevel : MonoBehaviour {

    public AudioClip MusicClipFirstLevel;

    public AudioSource MusicSourceFirstLevel;
	// Use this for initialization
	void Start () {
        MusicSourceFirstLevel.clip = MusicClipFirstLevel;
        MusicSourceFirstLevel.Play(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}