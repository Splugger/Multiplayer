using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour {

    AudioSource source;

	// Use this for initialization
	void Awake () {
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 1.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
