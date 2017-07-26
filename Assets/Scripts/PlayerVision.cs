using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{

    public float playerViewDist = 1f;

    int wallsMask;

    List<GameObject> darkTiles = new List<GameObject>();

    // Use this for initialization
    public void Start()
    {
        wallsMask = LayerMask.GetMask("Walls");
        InvokeRepeating("CalculateVision", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CalculateVision()
    {
        //calculate player vision
        foreach (GameObject playerObj in Game.control.playerObjs)
        {
            for (int x = 0; x < Game.control.levelGenerator.mapWidth; x++)
            {
                for (int y = 0; y < Game.control.levelGenerator.mapHeight; y++)
                {
                    Tile tile = Game.control.levelGenerator.tiles.SingleOrDefault(q => q.x == x && q.y == y);
                    if (tile != null)
                    {
                        if (tile.tileObj != null)
                        {
                            Vector3 playerPos = playerObj.transform.position;
                            Vector3 tilePos = tile.tileObj.transform.position;
                            float distance = (playerPos - tilePos).magnitude;
                            if (distance < playerViewDist)
                            {
                                BoxCollider2D collider = tile.tileObj.GetComponent<BoxCollider2D>();
                                RaycastHit2D hit = Physics2D.Linecast(playerPos, tilePos);
                                if (hit)
                                {
                                    Bounds checkBox = new Bounds(tile.tileObj.transform.position, Vector3.one * 0.2f);
                                    //disable tile at current search position if the hit tile is not the tile you're looking for
                                    if (!(checkBox.Contains(hit.point)))
                                    {
                                        SetTileVisibleAtPoint(x, y, false);
                                    }
                                    else
                                    {
                                        SetTileVisibleAtPoint(x, y, true);
                                    }
                                }
                                else
                                {
                                    SetTileVisibleAtPoint(x, y, true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    void SetTileVisibleAtPoint(int x, int y, bool state)
    {
        Game.control.levelGenerator.tiles.SingleOrDefault(q => q.x == x && q.y == y).CanSee(state);
    }
}
