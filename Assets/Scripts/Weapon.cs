using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Creature creature;

    public float vertical;
    public float horizontal;

    public float bulletSpeed = 200f;
    public float bulletSpread = 0.1f;
    public float bulletDamage = 10f;
    public float bulletKnockback = 3f;
    public float recoil = 10f;
    public float range = 2f;

    public float maxCooldown = 0.1f;
    public float cooldown = 0f;

    // Use this for initialization
    public virtual void Start()
    {
        creature = GetComponent<Creature>();
    }

    // Update is called once per frame
    public virtual void Update()
    {

        //bullet cooldown
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

    }

    public virtual void Fire(Vector3 aim)
    {
        if (cooldown <= 0f)
        {
            Vector3 normalAim = Vector3.Normalize(aim);

            //spawn bullet
            GameObject bulletObj = Instantiate(Resources.Load("Bullet") as GameObject);
            bulletObj.transform.position = transform.position + new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0f);
            bulletObj.GetComponent<Rigidbody2D>().AddForce(normalAim * bulletSpeed);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.damage = bulletDamage;
            bullet.knockback = bulletKnockback;
            bullet.lifetime = range;
            bullet.sourceCollider = creature.collider;

            creature.Move(-normalAim * recoil);

            cooldown = maxCooldown;
        }
    }
}
