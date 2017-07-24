using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{

    float health = 10f;

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (player.health >= player.maxHealth) return;
            player.Heal(health);
            Destroy(gameObject);
        }
    }
}
