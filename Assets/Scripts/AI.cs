using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Creature {

    public GameObject[] targets;
    public Vector3 target;

    float touchDamage = 5f;
    float touchKnockback = 100f;

    float viewDist = 10f;

    float moveTimer = 1f;
    float maxMoveTimer = 1f;

    // Use this for initialization
    public override void Start () {
        base.Start();
    }

    // Update is called once per frame
    public override void Update ()
    {
        targets = FindPlayerObjs();

        if (targets.Length > 0)
        {
            target = targets.FindNearestGameObject(transform.position).transform.position;

            horizontal = target.x - transform.position.x;
            vertical = target.y - transform.position.y;
        }
        else if (moveTimer <= 0f)
        {
            horizontal = Random.Range(-1f, 1f);
            vertical = Random.Range(-1f, 1f);

            moveTimer = maxMoveTimer;
        }
        else if (moveTimer > 0f)
        {
            moveTimer -= Time.deltaTime;
        }

        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(touchDamage, rb.velocity * touchKnockback);
        }
    }

    GameObject[] FindPlayerObjs()
    {
        List<GameObject> visiblePlayers = new List<GameObject>();
        Collider2D[] nearbyCols = Physics2D.OverlapCircleAll(transform.position, viewDist);

        foreach (Collider2D col in nearbyCols)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, col.transform.position);
            if (hit)
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    print(target);
                    Creature creature = hit.transform.gameObject.GetComponent<Creature>();
                    if (!creature.dead) visiblePlayers.Add(col.gameObject);
                }
            }
        }

        return visiblePlayers.ToArray();
    }

}
