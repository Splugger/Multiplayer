using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControl : AI
{
    public float duration;
    public MindControlType controlType;
    public Player player;

    // Use this for initialization
    public override void Start()
    {
        Destroy(this, duration);
        player = GetComponent<Player>();
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        switch (controlType)
        {
            case MindControlType.hostile:
                base.Update();
                break;
            case MindControlType.wander:
                RandomMovement();
                break;
        }

        Vector2 addedMovement = new Vector2(horizontal, vertical);
        Move(addedMovement);
    }
}
