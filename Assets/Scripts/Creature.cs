using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public float health = 100f;
    public float maxHealth = 100f;
    public bool dead = false;

    public bool canDamage = true;

    public float moveSpeed = 3f;
    public float acceleration = 5f;
    public float maxSpeed = 100f;

    public Vector2 desiredTranslation;

    float dodgeSpeed = 100f;
    float dodgeTimer = 0f;
    float maxDodgeTimer = 0.2f;
    float dodgeCooldownTimer = 0f;
    float maxdodgeCooldownTimer = 0.6f;

    public Rigidbody2D rb;
    public Collider2D collider;
    public SpriteRenderer sprite;

    public float horizontal;
    public float vertical;

    // Use this for initialization
    public virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public virtual void Update () {

        desiredTranslation = Vector3.Normalize(new Vector3(horizontal, vertical)) * moveSpeed;

        //dodge
        if (dodgeTimer > 0f)
        {
            dodgeTimer -= Time.deltaTime;
            canDamage = false;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        }
        else
        {
            canDamage = true;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        if (dodgeCooldownTimer > 0f)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }

        Move(desiredTranslation);
    }

    public void Move(Vector2 translation)
    {
        if (rb.velocity.magnitude < maxSpeed) rb.AddForce((translation - rb.velocity) * acceleration);
    }

    public void Dodge()
    {
        if (dodgeCooldownTimer <= 0f)
        {
            rb.AddForce(desiredTranslation * dodgeSpeed);
            dodgeTimer = maxDodgeTimer;
            dodgeCooldownTimer = maxdodgeCooldownTimer;
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health >= maxHealth) health = maxHealth;
    }

    public void Damage(float amount, Vector2? knockback)
    {
        health -= amount;
        if (health <= 0f) Die();
        if (knockback != null) Move(knockback.Value);
    }

    public void Die()
    {
        dead = true;
        gameObject.SetActive(false);
    }
}
