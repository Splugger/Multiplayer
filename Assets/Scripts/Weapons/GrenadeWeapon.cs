using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeapon : Weapon
{

    public float throwStaminaCost = 10f;

    // Use this for initialization
    public override void Awake()
    {
        weaponObj = Instantiate(Resources.Load("GrenadeWeapon") as GameObject);

        weaponObj.SetActive(false);

        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (creature == null)
        {
            weaponObj.SetActive(true);
        }
    }

    public override void Fire(Vector3 aim)
    {
        base.Fire(aim);

        if (creature.stamina > throwStaminaCost)
        {
            normalAim = Vector3.Normalize(aim);

            GameObject grenadeObj = Instantiate(Resources.Load("Grenade") as GameObject);
            grenadeObj.transform.position = transform.position + normalAim * 0.2f;
            grenadeObj.GetComponent<Rigidbody2D>().AddForce(normalAim * range);

            creature.UseStamina(throwStaminaCost);
        }
    }
}
