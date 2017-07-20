using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MindControlType { none, hostile, wander }

public class Trap : MonoBehaviour
{
    public float damage = 20f;

    public MindControlType controlType;
    public float mindControlDuration = 5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Creature creature = collision.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            Activate(creature);
        }
    }

    public virtual void Activate(Creature creature)
    {
        //damage
        creature.Damage(20f, Vector2.zero);
        //mind control
        if (controlType != MindControlType.none)
        {
            MindControl mindControl = creature.gameObject.AddComponent<MindControl>();
            mindControl.controlType = controlType;
            mindControl.duration = mindControlDuration;
        }

        //create effect and destroy
        GameObject effect = Instantiate(Resources.Load("TrapActivated") as GameObject);
        effect.transform.position = creature.transform.position;
        Destroy(gameObject);
    }
}
