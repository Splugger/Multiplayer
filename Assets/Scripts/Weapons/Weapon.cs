using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Creature creature;
    public string useButton = "Fire1";

    public float vertical;
    public float horizontal;

    public float damage = 10f;
    public float knockback = 3f;
    public float recoil = 3f;
    public float range = 2f;

    public int ammo = 100;
    public int maxAmmo = 100;

    public float maxCooldown = 0.1f;
    public float cooldown = 0f;

    public GameObject weaponObj;
    public Vector3 aim;
    public Vector3 normalAim;
    public AudioSource source;

    public Transform spriteTransform;
    public SpriteRenderer spriteRenderer;
    public Sprite sprite;
    public Color color;

    public WeaponControl control;

    Vector3 weaponObjOffset = new Vector3(0.08f, 0f, 0f);

    // Use this for initialization
    public virtual void Awake()
    {
        weaponObj.transform.position = transform.position;
        weaponObj.transform.parent = transform;

        creature = GetComponent<Creature>();
        if (creature != null)
        {
            control = GetComponent<WeaponControl>();
            control.weapons.Add(this);

            source = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        spriteTransform = weaponObj.transform.FindDeepChild("Sprite");
        spriteRenderer = spriteTransform.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = 3;
        spriteRenderer.color = color;

        control.ResetWeaponPositions();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (sprite != null) spriteRenderer.sprite = sprite;

        if (creature == null)
        {
            if (ammo <= 0) DestroyWeapon();
            return;
        }
        if (creature.dead) control.DropWeapon(this);

        if (aim.magnitude > 0f)
        {
            weaponObj.transform.LookAt2D(aim);
        }
        //weapon cooldown
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public virtual void Fire(Vector3 aim)
    {
        if (cooldown <= 0f)
        {
            normalAim = Vector3.Normalize(aim);

            //sound
            source.pitch = Random.Range(0.8f, 1.2f);

            creature.Move(-normalAim * recoil);

            cooldown = maxCooldown;
        }
        else
        {
            return;
        }
    }

    public void DestroyWeapon()
    {
        Destroy(weaponObj);
        Destroy(source);
        Destroy(this);
    }

    public virtual void SetPosition(int side)
    {
        weaponObj.transform.position = transform.position + weaponObjOffset * side;
    }

    public void GainAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maxAmmo) ammo = maxAmmo;
    }

    public void UseAmmo(int amount)
    {
        ammo -= amount;
        if (ammo < 0) ammo = 0;
    }

    public virtual void SetSprite()
    {
        if (sprite != null) spriteRenderer.sprite = sprite;
    }
}
