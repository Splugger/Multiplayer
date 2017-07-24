using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool open = false;
    SpriteRenderer sprite;

    public ItemType type;
    public Sprite openSprite;
    public Sprite closedSprite;

    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = closedSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !open)
        {
            Open();
        }
    }

    void Open()
    {
        Game.control.GenerateRandomItem(transform.position, type);
        open = true;
        sprite.sprite = openSprite;
    }
}
