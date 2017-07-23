using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponControl : WeaponControl
{

    Player player;

    float pickupRange = 0.1f;

    GameObject ammoSliderObj;
    Slider ammoSlider;

    // Use this for initialization
    public override void Start()
    {

        player = GetComponent<Player>();
        foreach (Weapon weapon in weapons)
        {
            weapon.aim = Vector2.up;
        }

        ammoSliderObj = transform.FindDeepChild("ammoSlider").gameObject;
        ammoSlider = ammoSliderObj.GetComponent<Slider>();

        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.vertical = Input.GetAxis("Player " + player.playerNum + " Aim Vertical");
            weapon.horizontal = Input.GetAxis("Player " + player.playerNum + " Aim Horizontal");

            Vector3 aimInput = new Vector3(weapon.horizontal, weapon.vertical, 0f);
            if (aimInput.magnitude > 0.5f)
            {
                weapon.aim = Vector3.Normalize(aimInput * 0.2f);
            }

            if (Input.GetButton("Player " + player.playerNum + " " + weapon.useButton) && weapon.cooldown <= 0f)
            {
                print(weapon);
                weapon.Fire(weapon.aim);
            }
        }

        //swap primary and secondary
        if (Input.GetButtonDown("Player " + player.playerNum + " Change Weapons"))
        {
            ChangeWeapons();
        }

        //pick up weapon
        if (Input.GetButtonDown("Player " + player.playerNum + " Pick Up"))
        {
            PickUpWeapon();
        }

        //set ammo slider
        if (primary.GetType().IsAssignableFrom(typeof(Gun)))
        {
            ammoSlider.value = (float)(((Gun)primary).ammo) / (float)((Gun)primary).maxAmmo;
        }

    }

    public void PickUpWeapon()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange);

        foreach (Collider2D col in colliders)
        {
            WeaponObject weaponObj = col.GetComponent<WeaponObject>();
            if (weaponObj != null)
            {
                if (weapons.Count >= 2 || weapons.Count == 3 && weapons.Any(q => q is GrenadeWeapon))
                {
                    if (weaponObj.weapon.GetType().IsAssignableFrom(typeof(GrenadeWeapon)))
                    {
                        DropWeapon(thrown);
                    }
                    else
                    {
                        DropWeapon(primary);
                    }
                }
                Weapon playerWeapon = weaponObj.weapon.CopyComponent(gameObject, true);
                Destroy(col.gameObject);
                if (weaponObj.weapon.GetType().IsAssignableFrom(typeof(GrenadeWeapon)))
                {
                    thrown = playerWeapon;
                }
                else
                {
                    primary = playerWeapon;
                }
                ResetWeaponControls();
            }
        }
    }

    public void ChangeWeapons()
    {
        Weapon temp = primary;
        primary = secondary;
        secondary = temp;
        ResetWeaponControls();
        ResetWeaponPositions();
    }

    public void ResetWeaponControls()
    {
        if (weapons.Count > 0)
        {
            primary.useButton = "Fire1";
        }
        if (weapons.Count > 1)
        {
            secondary.useButton = "Fire2";
        }
        if (weapons.Count > 2)
        {
            thrown.useButton = "Grenade";
        }
    }
}
