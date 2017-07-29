using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{

    public float playerViewDist = 3f;
    public float wallRevealDist = 0.3f;

    int wallsMask;

    List<GameObject> darkTiles = new List<GameObject>();

    // Use this for initialization
    public void Start()
    {
        wallsMask = LayerMask.GetMask("Walls");
        InvokeRepeating("CalculateVision", 0f, 0.1f);
    }

    public void CalculateVision()
    {
        //calculate player vision
        List<GameObject> playerObjs = Game.control.playerObjs;
        foreach (Tile tile in Game.control.levelGenerator.tiles)
        {
            tile.SetVisible(false);
            for (int i = 0; i < playerObjs.Count; i++)
            {
                if (tile.tileObj != null)
                {
                    Vector3 playerPos = playerObjs[i].transform.position;
                    Vector3 tilePos = tile.tileObj.transform.position;
                    float distance = (playerPos - tilePos).sqrMagnitude;
                    if (distance < playerViewDist)
                    {
                        BoxCollider2D collider = tile.tileObj.GetComponent<BoxCollider2D>();
                        RaycastHit2D hit = Physics2D.Linecast(playerPos, tilePos, wallsMask);
                        if (hit)
                        {
                            Bounds checkBox = new Bounds(tile.tileObj.transform.position, Vector3.one * wallRevealDist);
                            //disable tile at current search position if the hit tile is not the tile you're looking for
                            if ((checkBox.Contains(hit.point)))
                            {
                                tile.SetVisible(true);
                            }
                        }
                        else
                        {
                            tile.SetVisible(true);
                        }
                    }
                }
            }
        }
    }
}


