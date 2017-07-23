using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{

    Vector3 aimOffset;
    int aimDir = 1;
    public float staminaCost = 10f;
    public float slashWidth;

    // Use this for initialization
    public override void Awake()
    {
        weaponObj = Instantiate(Resources.Load("Sword") as GameObject);

        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Fire(Vector3 aim)
    {

        if (creature.stamina > staminaCost)
        {
            base.Fire(aim);

            //move sword
            weaponObj.transform.localScale = -new Vector2(weaponObj.transform.localScale.x, -weaponObj.transform.localScale.y);

            //use stamina
            creature.UseStamina(staminaCost);

            source.PlayOneShot(Resources.Load("Sound_Slash") as AudioClip);

            //spawn slash
            GameObject slashObj = Instantiate(Resources.Load("Slash") as GameObject);
            slashObj.transform.position = transform.position + normalAim * 0.2f;
            slashObj.transform.LookAt2D(aim);
            slashObj.transform.parent = transform;

            Slash slash = slashObj.GetComponent<Slash>();
            slash.damage = damage;
            slash.knockback = knockback;
            slash.height = range;
            slash.width = slashWidth;
            slash.sourceCollider = creature.collider;
        }
    }
}
