using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float radius = 2f;
    public float damage = 30f;
    public float knockback = 50f;

    AudioSource source;

    // Use this for initialization
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 1.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Creature creature = collision.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            Vector2 knockbackDir = Vector3.Normalize(creature.transform.position - transform.position);

            creature.Damage(30f, knockbackDir * knockback);
        }
    }
}
