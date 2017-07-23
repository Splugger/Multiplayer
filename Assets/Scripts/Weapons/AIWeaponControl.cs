using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponControl : WeaponControl
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
        foreach (Weapon weapon in weapons)
        {
            if (weapon is Melee) fireRange = weapon.range;

            if (ai.targets.Length > 0)
            {
                weapon.aim = ai.target - transform.position;
                float distance = weapon.aim.magnitude;

                if (distance < fireRange && weapon.cooldown <= 0)
                {
                    weapon.Fire(weapon.aim);
                }
            }
        }
        base.Update();
    }
}
