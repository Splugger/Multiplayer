using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour
{

    public float followSpeed = 0.1f;

    float minCamSize = 5f;
    float screenEdgeDist = 1f;

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

        float[] playerPositionsX = new float[Game.control.playerObjs.Length];
        float[] playerPositionsY = new float[Game.control.playerObjs.Length];

        int i = 0;

        foreach (GameObject playerObj in Game.control.playerObjs)
        {

            playerPositionsX[i] = playerObj.transform.position.x;
            playerPositionsY[i] = playerObj.transform.position.y;

            i++;
        }

        minX = Mathf.Min(playerPositionsX);
        maxX = Mathf.Max(playerPositionsX);
        minY = Mathf.Min(playerPositionsY);
        maxY = Mathf.Max(playerPositionsY);

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
}
