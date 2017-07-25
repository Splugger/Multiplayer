using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public float bulletSpeed = 200f;
    public float bulletSpread = 0.1f;

    // Use this for initialization
    public override void Awake()
    {
        weaponObj = Instantiate(Resources.Load("Gun") as GameObject);

        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (aim.x < 0)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }

        base.Update();
    }

    public override void Fire(Vector3 aim)
    {
        base.Fire(aim);

        if (ammo > 0)
        {
            source.PlayOneShot(Resources.Load("Sound_Bullet") as AudioClip);

            //spawn bullet
            GameObject bulletObj = Instantiate(Resources.Load("Bullet") as GameObject);
            bulletObj.transform.position = spriteTransform.position;//transform.position + normalAim * 0.2f + new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0f);
            bulletObj.GetComponent<Rigidbody2D>().AddForce(normalAim * bulletSpeed);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.damage = damage;
            bullet.knockback = knockback;
            bullet.lifetime = range;
            bullet.sourceCollider = creature.collider;

            UseAmmo(1);
        }
    }

    public override void SetSprite()
    {
        if (maxCooldown < 0.5f)
        {
            if (damage < 15f || maxCooldown > 0.35f)
            {
                sprite = Resources.Load<Sprite>("AssaultRifle");
            }
            else
            {
                sprite = Resources.Load<Sprite>("MachineGun");
            }
        }
        else
        {
            if (damage < 15f || range < 2)
            {
                sprite = Resources.Load<Sprite>("Pistol");
            }
            else
            {
                sprite = Resources.Load<Sprite>("Rifle");
            }
        }
        base.SetSprite();
    }
}
