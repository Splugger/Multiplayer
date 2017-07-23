using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Open();
        }
    }

    void Open()
    {

    }

    void GenerateContents()
    {
        string objName = string.Empty;
        switch (Random.Range(0, 2))
        {
            case 0:
                objName = "Melee";
                break;
            case 1:
                objName = "Gun";
                break;
            case 2:
                objName = "Shield";
                break;
        }
        GameObject baseObj = Instantiate(Resources.Load("Item_" + objName) as GameObject);
        Weapon weapon = baseObj.GetComponent<Weapon>();
        //set general weapon parameters
        weapon.range = Random.Range(1f, 3f);
        weapon.recoil = Random.Range(1f, 5f);
        weapon.knockback = Random.Range(1f, 5f);
        weapon.maxCooldown = Random.Range(0.1f, 2f);
        weapon.damage = Random.Range(5f, 20f);
        //gun
        if (weapon.GetType().IsAssignableFrom(typeof(Gun)))
        {
            ((Gun)weapon).maxAmmo = Random.Range(10, 200);
            ((Gun)weapon).ammo = ((Gun)weapon).maxAmmo;
        }
        //melee
        if (weapon.GetType().IsAssignableFrom(typeof(Melee)))
        {
            ((Melee)weapon).staminaCost = Random.Range(1f, 3f) * weapon.damage;
            ((Melee)weapon).slashWidth = Random.Range(1f, 2f);
            weapon.damage *= 2f;
        }
        //grenade
        if (weapon.GetType().IsAssignableFrom(typeof(GrenadeWeapon)))
        {
            ((GrenadeWeapon)weapon).throwStaminaCost = Random.Range(5f, 10f);
            weapon.damage *= 3f;
        }
    }

}
