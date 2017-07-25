using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum StartEquipment { swordAndShield, gun }

public class PlayerWeaponControl : WeaponControl
{

    public StartEquipment startEquipment = StartEquipment.gun;
    public bool startWithGrenade = true;

    Player player;

    float pickupRange = 0.1f;

    GameObject ammoSliderObj;
    Slider ammoSlider;

    private void Awake()
    {
        if (startEquipment == StartEquipment.gun)
        {
            primary = gameObject.AddComponent<Gun>();
        }
        if (startEquipment == StartEquipment.swordAndShield)
        {
            primary = gameObject.AddComponent<Melee>();
            secondary = gameObject.AddComponent<Shield>();
        }
        if (startWithGrenade)
        {
            thrown = gameObject.AddComponent<Grenade>();
        }

        ResetWeaponControls();
        ResetWeaponPositions();
    }

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

        if (Input.GetKeyDown(KeyCode.L) && player.playerNum == 1)
        {
            Game.control.NewLevel();
        }

        //set ammo slider
        if (primary.GetType().IsAssignableFrom(typeof(Gun)))
        {
            ammoSlider.value = (float)primary.ammo / primary.maxAmmo;
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
                if (secondary != null && primary != null)
                {
                    if (weaponObj.weapon.GetType().IsAssignableFrom(typeof(Grenade)))
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
                if (weaponObj.weapon.GetType().IsAssignableFrom(typeof(Grenade)))
                {
                    thrown = playerWeapon;
                }
                else
                {
                    if (secondary != null)
                        primary = playerWeapon;
                    else
                        secondary = playerWeapon;
                }
                ResetWeaponControls();
            }
        }
    }

    public void ChangeWeapons()
    {
        if (secondary != null)
        {
            Weapon temp = primary;
            primary = secondary;
            secondary = temp;
            ResetWeaponControls();
            ResetWeaponPositions();
        }
    }

    public void ResetWeaponControls()
    {
        if (weapons.Count > 0)
        {
            primary.useButton = "Fire1";
        }
        if (weapons.Count > 1)
        {
            if (secondary != null) secondary.useButton = "Fire2";
            if (thrown != null) thrown.useButton = "Grenade";
        }
        if (weapons.Count > 2)
        {
            thrown.useButton = "Grenade";
        }
    }
}
