using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{

    public float damage = 10f;
    public float knockback = 1f;
    public float lifetime = 0.4f;
    public float height = 1f;
    public float width = 1f;

    float timeSinceInstantiated;

    public Collider2D collider;

    public Rigidbody2D rb;

    public Collider2D sourceCollider;

    private float timeInstantiated;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider, sourceCollider);

        timeInstantiated = Time.timeSinceLevelLoad;

        //transform.localScale = new Vector2(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        //calculate lifetime
        timeSinceInstantiated = Time.timeSinceLevelLoad - timeInstantiated;
        if (timeSinceInstantiated >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Creature creature = collision.gameObject.GetComponent<Creature>();
            if (creature != null)
            {
                if (creature.canDamage)
                {
                    Vector2 knockbackDir = transform.up * knockback;
                    creature.Damage(damage, knockbackDir);
                }
            }
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Impact();
            }
        }
    }
}
