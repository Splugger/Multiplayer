using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftCollisions : MonoBehaviour {

    BoxCollider2D collider;
    Vector2 originalColliderSize;
    float collisionTimer = 0f;
    float maxCollisionTimer = 0.1f;
    float minSize = 0.02f;

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
        SoftCollisions softCollisions = collision.gameObject.GetComponent<SoftCollisions>();
        if (softCollisions != null)
        {
            if (collider.size.x > minSize && collider.size.y > minSize)
            {
                collider.size = new Vector2(collider.size.x - Time.deltaTime, collider.size.y - Time.deltaTime);
                collisionTimer = maxCollisionTimer;
            }
        }
    }
}
