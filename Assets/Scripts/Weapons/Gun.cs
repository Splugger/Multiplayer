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
}
