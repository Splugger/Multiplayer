using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Creature creature;

    public float vertical;
    public float horizontal;

    public float throwDistance = 100f;
    public float throwStaminaCost = 10f;

    public float bulletSpeed = 200f;
    public float bulletSpread = 0.1f;
    public float bulletDamage = 10f;
    public float bulletKnockback = 3f;
    public float recoil = 5f;
    public float range = 2f;

    public float maxCooldown = 0.1f;
    public float cooldown = 0f;

    public float maxGrenadeCooldown = 1f;
    public float grenadeCooldown = 0f;

    public GameObject gun;
    public Vector3 aim;
    public Vector3 normalAim;
    public AudioSource source;

    // Use this for initialization
    public virtual void Start()
    {
        creature = GetComponent<Creature>();
        source = gameObject.AddComponent<AudioSource>();

        gun = Instantiate(Resources.Load("Gun") as GameObject);
        gun.transform.position = transform.position;
        gun.transform.parent = transform;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (aim.magnitude > 0f)
        {
            gun.transform.LookAt2D(aim);

        }
        //bullet cooldown
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
        //grenade cooldown
        if (grenadeCooldown > 0f)
        {
            grenadeCooldown -= Time.deltaTime;
        }

    }

    public virtual void Fire(Vector3 aim)
    {
        if (cooldown <= 0f)
        {
            normalAim = Vector3.Normalize(aim);

            //sound
            source.pitch = Random.Range(0.8f, 1.2f);
            source.PlayOneShot(Resources.Load("Sound_Bullet") as AudioClip);

            //spawn bullet
            GameObject bulletObj = Instantiate(Resources.Load("Bullet") as GameObject);
            bulletObj.transform.position = transform.position + normalAim * 0.2f + new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0f);
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

    public void ThrowGrenade()
    {
        if (creature.stamina > throwStaminaCost)
        {
            normalAim = Vector3.Normalize(aim);

            GameObject grenadeObj = Instantiate(Resources.Load("Grenade") as GameObject);
            grenadeObj.transform.position = transform.position + normalAim * 0.2f;
            grenadeObj.GetComponent<Rigidbody2D>().AddForce(normalAim * throwDistance);

            grenadeCooldown = maxGrenadeCooldown;
            creature.UseStamina(throwStaminaCost);
        }
    }
}
