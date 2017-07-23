using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour
{

    public float followSpeed = 0.1f;

    float minCamSize = 1f;
    float screenEdgeDist = 1f;
    float enemyFocusDist = 1f;

    Vector3 target;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float minX = 0f;
        float minY = 0f;
        float maxX = 0f;
        float maxY = 0f;

        List<float> playerPositionsX = new List<float>();
        List<float> playerPositionsY = new List<float>();

        foreach (GameObject playerObj in Game.control.playerObjs)
        {
            foreach (GameObject obj in FindNearbyEnemies())
            {
                playerPositionsX.Add(obj.transform.position.x);
                playerPositionsY.Add(obj.transform.position.y);
            }

            //find min and max x and y values
            playerPositionsX.Add(playerObj.transform.position.x);
            playerPositionsY.Add(playerObj.transform.position.y);
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
            cam.orthographicSize = Mathf.Max(sizeY + screenEdgeDist, sizeX + screenEdgeDist);
        }
    }

    GameObject[] FindNearbyEnemies()
    {
        List<GameObject> nearbyEnemies = new List<GameObject>();

        Collider2D[] nearbyCols = Physics2D.OverlapCircleAll(transform.position, enemyFocusDist);
        foreach (Collider2D col in nearbyCols)
        {
            if (col.gameObject.GetComponent<Creature>() != null && col.gameObject.tag != "Player")
            {
                nearbyEnemies.Add(col.gameObject);
            }
        }
        return nearbyEnemies.ToArray();
    }
}
