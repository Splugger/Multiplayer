using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Creature {

    public GameObject[] targets;
    public Vector3 target;

    float touchDamage = 5f;
    float touchKnockback = 100f;

    float viewDist = 10f;

    public float moveTimer = 1f;
    public float maxMoveTimer = 1f;

    public float searchRate = 0.1f;

    // Use this for initialization
    public override void Start () {
        base.Start();
        StartCoroutine(TargetSearch());
    }

    // Update is called once per frame
    public override void Update ()
    {
        if (targets.Length > 0)
        {
            target = targets.FindNearestGameObject(transform.position).transform.position;
            MoveToTarget(target);
        }
        else
        {
            RandomMovement();
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

    IEnumerator TargetSearch()
    {
        while (gameObject.activeInHierarchy)
        {
            targets = FindPlayerObjs();
            yield return new WaitForSeconds(Random.value * searchRate);
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
                    Creature creature = hit.transform.gameObject.GetComponent<Creature>();
                    if (!creature.dead) visiblePlayers.Add(col.gameObject);
                }
            }
        }

        return visiblePlayers.ToArray();
    }

    public void MoveToTarget(Vector3 target)
    {
        horizontal = target.x - transform.position.x;
        vertical = target.y - transform.position.y;
    }

    public void RandomMovement()
    {
        if (moveTimer <= 0f)
        {
            horizontal = Random.Range(-1f, 1f);
            vertical = Random.Range(-1f, 1f);

            moveTimer = maxMoveTimer;
        }
        else if (moveTimer > 0f)
        {
            moveTimer -= Time.deltaTime;
        }
    }

}
