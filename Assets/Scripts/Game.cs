using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour {

    public static Game control;

    GameObject camObj;
    public ScreenShake screenShake;

    public List<GameObject> playerObjs = new List<GameObject>();

	// Use this for initialization
	void Awake () {
        control = this;

        camObj = GameObject.FindWithTag("MainCamera");
        screenShake = camObj.GetComponent<ScreenShake>();

        playerObjs = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
