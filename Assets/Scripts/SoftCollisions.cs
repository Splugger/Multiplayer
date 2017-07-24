using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftCollisions : MonoBehaviour {

    BoxCollider2D collider;
    Vector2 originalColliderSize;
    float collisionTimer = 0f;
    float maxCollisionTimer = 0.1f;

    // Use this for initialization
    void Start () {
        collider = GetComponent<BoxCollider2D>();
        originalColliderSize = collider.size;
    }

    // Update is called once per frame
    void Update () {
        //collision timer
        if (collisionTimer > 0)
        {
            collisionTimer -= Time.deltaTime;
        }
        else
        {
            collider.size = originalColliderSize;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Creature creature = collision.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            if (collider.size.x > 0 && collider.size.y > 0)
            {
                collider.size = new Vector2(collider.size.x - Time.deltaTime, collider.size.y - Time.deltaTime);
                collisionTimer = maxCollisionTimer;
            }
        }
    }
}
