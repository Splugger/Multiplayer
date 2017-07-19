using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float damage = 10f;
    public float knockback = 1f;
    public float lifetime = 2f;

    public Collider2D collider;

    public Rigidbody2D rb;

    public Collider2D sourceCollider;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider, sourceCollider);
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Creature>() != null)
        {
            Creature creature = collision.gameObject.GetComponent<Creature>();
            if (creature.canDamage)
            {
                Vector2 knockbackDir = -rb.velocity * knockback;
                creature.Damage(damage, knockbackDir);
            }
            /*else
            {
                Physics2D.IgnoreCollision(collider, creature.collider);
                return;
            }*/
        }
        Destroy(gameObject);
    }
}
