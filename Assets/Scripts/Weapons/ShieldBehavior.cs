using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour {

    public float stability;

    Creature creature;

	// Use this for initialization
	void Start () {
        creature = GetComponentInParent<Creature>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (creature != null)
        {
            float staminaCost = 0;
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null && creature.stamina > bullet.damage / stability)
            {
                //ensure you can't shoot your own shield
                if (!transform.IsChildOf(bullet.sourceCollider.transform))
                {
                    staminaCost = bullet.damage;
                    bullet.Impact();
                }
            }
            Slash slash = collision.gameObject.GetComponent<Slash>();
            if (slash != null && creature.stamina > slash.damage / stability)
            {
                staminaCost = slash.damage;
                slash.damage = 0;
            }
            creature.UseStamina(staminaCost / stability);
        }
    }
}
