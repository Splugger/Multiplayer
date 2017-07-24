using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public float health = 100f;
    public float maxHealth = 100f;
    public bool dead = false;

    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaRegen = 60f;
    public float staminaCooldownTime = 1.5f;
    public float staminaCooldown = 0f;

    public bool canDamage = true;

    public float moveSpeed = 2f;
    public float acceleration = 5f;

    public Vector2 desiredTranslation;

    float dodgeSpeed = 100f;
    float dodgeTimer = 0f;
    float maxDodgeTimer = 0.2f;
    float dodgeCooldownTimer = 0f;
    float maxdodgeCooldownTimer = 0.6f;
    float dodgeStaminaCost = 15f;

    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator anim;
    public Shield shield;
    public BoxCollider2D collider;
    AudioSource source;

    Color color = Color.white;

    public float horizontal;
    public float vertical;

    // Use this for initialization
    public virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shield = GetComponent<Shield>();
        source = gameObject.AddComponent<AudioSource>();

        //set random color
        SetColor(new Color(Random.Range(0.4f, 1f), Random.Range(0.4f, 1f), Random.Range(0.4f, 1f), 1f));
    }

    // Update is called once per frame
    public virtual void Update ()
    {
        if (dead)
        {
            rb.drag = 20f;
            return;
        }

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

        //regenerate stamina
        if (staminaCooldown > 0)
        {
            staminaCooldown -= Time.deltaTime;
        }
        else
        {
            RegenerateStamina(staminaRegen * Time.deltaTime);
        }

        Move(desiredTranslation);
        anim.SetFloat("Horizontal", horizontal);
        if (horizontal < 0f)
        {
            sprite.flipX = true;
        }
        anim.SetFloat("Vertical", vertical);
    }

    public void Move(Vector2 translation)
    {
        if (rb != null) rb.AddForce((translation - rb.velocity) * acceleration);
    }

    public void Dodge()
    {
        if (dodgeCooldownTimer <= 0f && stamina > dodgeStaminaCost)
        {
            rb.AddForce(desiredTranslation * dodgeSpeed);
            dodgeTimer = maxDodgeTimer;
            dodgeCooldownTimer = maxdodgeCooldownTimer;
            UseStamina(dodgeStaminaCost);
        }
    }

    public virtual void RegenerateStamina(float amount)
    {
        float shieldMultiplier = 1f;
        if (shield != null)
            if (shield.weaponObj.activeInHierarchy) shieldMultiplier = 0.3f;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
            return;
        }
        if (stamina + amount > maxStamina)
        {
            amount = maxStamina - stamina;
        }
        stamina += amount * shieldMultiplier;
    }

    public virtual void Heal(float amount)
    {
        health += amount;
        if (health >= maxHealth) health = maxHealth;
    }

    public virtual void Damage(float amount, Vector2 knockback)
    {
        health -= amount;
        if (health <= 0f) Die();
        if (knockback != null) Move(knockback);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(Flash(0.1f, Color.white));
        }
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(Resources.Load("Sound_Hurt") as AudioClip);
    }

    public virtual void Die()
    {
        anim.SetTrigger("Dead");
        dead = true;
    }

    public virtual void UseStamina(float amount)
    {
        stamina -= amount;
        staminaCooldown = staminaCooldownTime;
    }

    public void AddColor(Color addedColor)
    {
        color += addedColor;
        sprite.color = color;
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        sprite.color = color;
    }

    IEnumerator Flash(float time, Color flashColor)
    {
        sprite.color = flashColor;

        time += Time.deltaTime;
        yield return new WaitForSeconds(time);
        sprite.color = color;
    }
}
