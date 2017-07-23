using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour {

    public Weapon weapon;

	// Use this for initialization
	void Awake () {
        weapon = GetComponent<Weapon>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
