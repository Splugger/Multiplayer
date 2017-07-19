using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon {

    Player player;

    GameObject reticle;

    // Use this for initialization
    public override void Start ()
    {
        player = GetComponent<Player>();
        reticle = Instantiate(Resources.Load("Reticle") as GameObject);
        reticle.SetActive(false);

        base.Start();
    }

    // Update is called once per frame
    public override void Update ()
    {
        vertical = Input.GetAxis("Player " + player.playerNum + " Aim Vertical");
        horizontal = Input.GetAxis("Player " + player.playerNum + " Aim Horizontal");

        Vector3 aim = Vector3.Normalize(new Vector3(horizontal, vertical, 0f)) * 0.2f;

        if (aim.magnitude > 0f)
        {
            reticle.SetActive(true);
            reticle.transform.position = transform.position + aim;

            if (Input.GetButton("Player " + player.playerNum + " Fire1") && cooldown <= 0f)
            {
                Fire(aim);
            }
        }
        else
        {
            reticle.SetActive(false);
        }

        base.Update();
    }
}
