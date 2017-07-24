using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    float percentMaxAmmo = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerWeaponControl playerWeaponControl = collider.gameObject.GetComponent<PlayerWeaponControl>();
        if (playerWeaponControl != null)
        {
            Weapon weapon = playerWeaponControl.primary;
            int ammoGained = (int)(weapon.maxAmmo * percentMaxAmmo);
            if (weapon.ammo >= weapon.maxAmmo) return;
            weapon.GainAmmo(ammoGained);
            Destroy(gameObject);
        }
    }
}
