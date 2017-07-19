using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature {

    public int playerNum = 1;

	// Use this for initialization
	public override void Start () {
        base.Start();
	}

    // Update is called once per frame
    public override void Update () {

        //movement
        horizontal = Input.GetAxis("Player " + playerNum + " Horizontal");
        vertical = Input.GetAxis("Player " + playerNum + " Vertical");

        if (Input.GetButtonDown("Player " + playerNum + " Dodge"))
        {
            Dodge();
        }

        base.Update();
	}
}
