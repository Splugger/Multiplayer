using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour
{
    public float followSpeed = 0.1f;

    public bool zoom = false;

    bool followPlayer1 = false;

    float minCamSize = 1f;
    float screenEdgeDist = 1f;
    float enemyFocusDist = 1f;

    Vector3 target;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        //start camera focused on player
        Vector3 player1Pos = Game.control.playerObjs[0].transform.position;
        transform.position = new Vector3(player1Pos.x, player1Pos.y, transform.position.z);

        cam.backgroundColor = Game.control.levelGenerator.wallColor - Game.control.memoryColorOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.control.playerObjs.Count == 0) return;

        float minX = 0f;
        float minY = 0f;
        float maxX = 0f;
        float maxY = 0f;

        List<float> playerPositionsX = new List<float>();
        List<float> playerPositionsY = new List<float>();

        for (int i = 0; i < Game.control.playerObjs.Count; i++)
        {
            //calculate midpoint

            // add nearby enemies
            foreach (GameObject obj in FindNearbyEnemies())
            {
                playerPositionsX.Add(obj.transform.position.x);
                playerPositionsY.Add(obj.transform.position.y);
            }

            if (followPlayer1 && i != 0) break;

            //find min and max x and y values
            playerPositionsX.Add(Game.control.playerObjs[i].transform.position.x);
            playerPositionsY.Add(Game.control.playerObjs[i].transform.position.y);
        }

        minX = Mathf.Min(playerPositionsX.ToArray());
        maxX = Mathf.Max(playerPositionsX.ToArray());
        minY = Mathf.Min(playerPositionsY.ToArray());
        maxY = Mathf.Max(playerPositionsY.ToArray());

        float sizeX = (Mathf.Abs(maxX - minX) / 2);
        float sizeY = (Mathf.Abs(maxY - minY) / 2);

        float newX = minX + sizeX;
        float newY = minY + sizeY;

        target = new Vector3(newX, newY, 0f);

        transform.position = new Vector3(transform.position.x + (target.x - (transform.position.x)) * followSpeed, transform.position.y + (target.y - (transform.position.y)) * followSpeed, transform.position.z);

        //zoom
        if (sizeY + screenEdgeDist > minCamSize || sizeX + screenEdgeDist > minCamSize)
        {
            if (zoom)
            {
                cam.orthographicSize = Mathf.Max(sizeY + screenEdgeDist, sizeX + screenEdgeDist);
            }
            else
            {
                followPlayer1 = true;
            }
        }
    }

    GameObject[] FindNearbyEnemies()
    {
        List<GameObject> nearbyEnemies = new List<GameObject>();

        Collider2D[] nearbyCols = Physics2D.OverlapCircleAll(transform.position, enemyFocusDist);
        foreach (Collider2D col in nearbyCols)
        {
            Creature creature = col.gameObject.GetComponent<Creature>();
            if (creature != null && col.gameObject.tag != "Player")
            {
                if (!creature.dead) nearbyEnemies.Add(col.gameObject);
            }
        }
        return nearbyEnemies.ToArray();
    }
}
