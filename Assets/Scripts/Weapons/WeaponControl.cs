using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{

    public List<Weapon> weapons;
    public Weapon primary;
    public Weapon secondary;
    public Weapon special;
    public Creature creature;

    // Use this for initialization
    public virtual void Start()
    {
        if (Time.timeSinceLevelLoad < Time.deltaTime)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i] = Game.control.GenerateRandomWeapon(weapons[i], 1);

                if (weapons[i].spriteRenderer != null)
                {
                    weapons[i].spriteRenderer.color = weapons[i].color;
                }
            }
        }

        creature = GetComponent<Creature>();
        WeaponSetup();
        ResetWeaponPositions();
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public void DropWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
        //create dropped object on floor
        GameObject obj = Instantiate(Resources.Load("Item_Weapon") as GameObject);
        obj.name = weapon.GetType().ToString();
        obj.transform.position = transform.position;
        //add weapon to dropped object
        weapon.CopyComponent(obj, true);
        WeaponObject weaponObj = obj.GetComponent<WeaponObject>();
        weaponObj.weapon = weapon;
        weapon.DestroyWeapon();
    }

    public virtual void WeaponSetup()
    {
        if (weapons.Any(q => q is Grenade))
        {
            special = weapons.OfType<Grenade>().ToList()[0];
        }
        if (weapons.Count > 0)
        {
            if (!weapons[0].GetType().IsAssignableFrom(typeof(Grenade)))
            {
                primary = weapons[0];
            }
        }
        if (weapons.Count > 1)
        {
            if (!weapons[1].GetType().IsAssignableFrom(typeof(Grenade)))
            {
                secondary = weapons[1];
            }
        }
        if (weapons.Count > 2)
        {
            if (!weapons[2].GetType().IsAssignableFrom(typeof(Grenade)))
            {
                if (secondary == null) secondary = weapons[2];
                if (primary == null) primary = weapons[2];
            }
        }
    }

    public void ResetWeaponPositions()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon == primary) weapon.SetPosition(1);
            if (weapon == secondary) weapon.SetPosition(-1);
        }
    }
}
