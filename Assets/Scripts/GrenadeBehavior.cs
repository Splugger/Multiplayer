using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{

    public float movementTime = 2f;
    public float detonationTime = 3f;

    float maxSpeed;
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
        rb = GetComponent<Rigidbody2D>();

        timeInstantiated = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        //speed by lifetime
        timeSinceInstantiated = Time.timeSinceLevelLoad - timeInstantiated;
        maxSpeed = -(timeSinceInstantiated + movementTime) * (timeSinceInstantiated - movementTime);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.AddForce(-rb.velocity);
        }

        if (timeSinceInstantiated >= detonationTime)
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        GameObject explosion = Instantiate(Resources.Load("Explosion") as GameObject);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
