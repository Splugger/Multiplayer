using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float damage = 10f;
    public float knockback = 1f;
    public float lifetime = 2f;

    float maxSpeed;
    float timeSinceInstantiated;

    public Collider2D collider;

    public Rigidbody2D rb;

    public Collider2D sourceCollider;

    private float timeInstantiated;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider, sourceCollider);
        rb = GetComponent<Rigidbody2D>();

        Game.control.screenShake.Pulse(0.3f, 0.1f * knockback);
        timeInstantiated = Time.timeSinceLevelLoad;
    }
	
	// Update is called once per frame
	void Update () {
        //rotate towards velocity
        transform.LookAt2D(rb.velocity);

        //speed by lifetime
        timeSinceInstantiated = Time.timeSinceLevelLoad - timeInstantiated;
        maxSpeed = -(timeSinceInstantiated + lifetime) * (timeSinceInstantiated - lifetime);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.AddForce(-rb.velocity);
        }

        if (maxSpeed <= 0f)
        {
            Impact();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Creature creature = collision.gameObject.GetComponent<Creature>();
            if (creature != null)
            {
                if (creature.canDamage)
                {
                    Vector2 knockbackDir = rb.velocity * knockback;
                    creature.Damage(damage, knockbackDir);
                }
                /*else
                {
                    Physics2D.IgnoreCollision(collider, creature.collider);
                    return;
                }*/
            }

            Impact();
        }
    }

    public void Impact()
    {
        GameObject impact = Instantiate(Resources.Load("BulletImpact") as GameObject);
        impact.transform.position = transform.position;
        Destroy(gameObject);
    }
}
