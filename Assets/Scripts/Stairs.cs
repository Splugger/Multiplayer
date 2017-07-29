using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (Input.GetButtonDown("Player " + player.playerNum + " Interact"))
            {
                Game.control.NewLevel();
            }
        }
    }
}
