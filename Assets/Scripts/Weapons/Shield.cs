using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon
{

    float stability = 0.4f;

    float holdTimer = 0f;

    // Use this for initialization
    public override void Awake()
    {
        weaponObj = Instantiate(Resources.Load("Shield") as GameObject);
        ShieldBehavior behavior = weaponObj.GetComponentInChildren<ShieldBehavior>();
        behavior.stability = stability;

        weaponObj.SetActive(false);

        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (holdTimer > 0f)
        {
            holdTimer -= Time.deltaTime;
        }
        else
        {
            weaponObj.SetActive(false);
        }
        if (creature == null)
        {
            weaponObj.SetActive(true);
        }

        base.Update();
    }

    public override void Fire(Vector3 aim)
    {
        weaponObj.SetActive(true);
        holdTimer = 0.1f;
    }
}
