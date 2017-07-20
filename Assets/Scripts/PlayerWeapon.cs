using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon {

    Player player;

    // Use this for initialization
    public override void Start ()
    {
        player = GetComponent<Player>();
        aim = Vector2.up;

        base.Start();
    }

    // Update is called once per frame
    public override void Update ()
    {
        vertical = Input.GetAxis("Player " + player.playerNum + " Aim Vertical");
        horizontal = Input.GetAxis("Player " + player.playerNum + " Aim Horizontal");

        Vector3 aimInput = new Vector3(horizontal, vertical, 0f);
        if (aimInput.magnitude > 0.5f)
        {
            aim = Vector3.Normalize(aimInput * 0.2f);
        }

        base.Update();

        if (Input.GetButton("Player " + player.playerNum + " Fire1") && cooldown <= 0f)
        {
            Fire(aim);
        }

        if (Input.GetButtonDown("Player " + player.playerNum + " Grenade"))
        {
            ThrowGrenade();
        }
    }
}
