using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public static Game control;

    public GameObject[] playerObjs;

	// Use this for initialization
	void Awake () {
        control = this;
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
