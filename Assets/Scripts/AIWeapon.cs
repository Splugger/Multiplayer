using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : Weapon
{
    AI ai;

    float range = 3f;

    // Use this for initialization
    public override void Start()
    {
        ai = GetComponent<AI>();

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {

        Vector3 aim = ai.target - transform.position;
        float distance = aim.magnitude;

        if (distance < range)
        {
            Fire(aim);
        }

        base.Update();
    }
}
