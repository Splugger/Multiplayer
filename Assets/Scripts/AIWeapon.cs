using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : Weapon
{
    AI ai;

    float fireRange = 5f;

    // Use this for initialization
    public override void Start()
    {
        ai = GetComponent<AI>();

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {

        if (ai.targets.Length > 0)
        {
            aim = ai.target - transform.position;
            float distance = aim.magnitude;

            if (distance < fireRange)
            {
                Fire(aim);
            }
        }
        base.Update();
    }
}
