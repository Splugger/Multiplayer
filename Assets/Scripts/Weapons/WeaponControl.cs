using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{

    public List<Weapon> weapons;
    public Weapon primary;
    public Weapon secondary;
    public Weapon thrown;
    public Creature creature;

    // Use this for initialization
    public virtual void Start()
    {
        creature = GetComponent<Creature>();
        WeaponSetup();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (creature != null)
        {
            if (creature.dead)
            {
                foreach (Weapon weapon in weapons)
                {
                    DropWeapon(weapon);
                }
            }
        }
    }

    public void DropWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
        //create dropped object on floor
        GameObject obj = Instantiate(Resources.Load("WeaponObject") as GameObject);
        obj.name = weapon.GetType().ToString();
        obj.transform.position = transform.position;
        //add weapon to dropped object
        weapon.CopyComponent(obj, true);
        WeaponObject weaponObj = obj.GetComponent<WeaponObject>();
        weaponObj.weapon = weapon;
        weapon.Destroy();
    }

    public virtual void WeaponSetup()
    {
        if (weapons.Any(q => q is GrenadeWeapon))
        {
            thrown = weapons.OfType<GrenadeWeapon>().ToList()[0];
        }
        if (weapons.Count > 0)
        {
            if (!weapons[0].GetType().IsAssignableFrom(typeof(GrenadeWeapon)))
            {
                primary = weapons[0];
            }
        }
        if (weapons.Count > 1)
        {
            if (!weapons[1].GetType().IsAssignableFrom(typeof(GrenadeWeapon)))
            {
                secondary = weapons[1];
            }
        }
        if (weapons.Count > 2)
        {
            if (!weapons[2].GetType().IsAssignableFrom(typeof(GrenadeWeapon)))
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
