using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallAppearance : MonoBehaviour {

    SpriteRenderer sprite;

    public Tile tile;
    public Texture2D texture;
    public Sprite singleSprite;
    Sprite[] sprites;

    public BoxCollider2D collider;

    // Use this for initialization
    void Awake () {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(texture.name);
        sprite.sprite = sprites[4];
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetAppearance()
    {
        /*bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;


        for (int x = tile.x - 1; x <= tile.x + 1; x++)
        {
            for (int y = tile.y - 1; y <= tile.y + 1; y++)
            {
                if (x >= 0 && x < Game.control.levelGenerator.mapWidth && y >= 0 && y < Game.control.levelGenerator.mapWidth)
                {
                    if (x != tile.x || y != tile.y)
                    {
                        if (Game.control.levelGenerator.tileMap[x, y] == TileType.floor)
                        {
                            if (x > tile.x && !(y > tile.y || y < tile.y)) up = true;
                            if (x < tile.x && !(y > tile.y || y < tile.y)) down = true;
                            if (y > tile.y && !(x > tile.y || x < tile.x)) right = true;
                            if (y < tile.y && !(x > tile.y || x < tile.x)) left = true;
                        }
                    }
                }
            }
        }

        if (up)
        {
            sprite.sprite = sprites[1];
        }
        if (left)
        {
            sprite.sprite = sprites[3];
        }
        if (right)
        {
            sprite.sprite = sprites[5];
        }
        if (down)
        {
            sprite.sprite = sprites[7];
        }
        if (up && left)
        {
            sprite.sprite = sprites[0];
        }
        if (up && right)
        {
            sprite.sprite = sprites[2];
        }
        if (left && down)
        {
            sprite.sprite = sprites[6];
        }
        if (down && right)
        {
            sprite.sprite = sprites[8];
        }
        if (down && up && left && right)
        {
            sprite.sprite = singleSprite;
        }*/

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.16f);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 0.16f);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.16f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.16f);

        if (!hitUp)
        {
            sprite.sprite = sprites[1];
        }
        if (!hitLeft)
        {
            sprite.sprite = sprites[3];
        }
        if (!hitRight)
        {
            sprite.sprite = sprites[5];
        }
        if (!hitDown)
        {
            sprite.sprite = sprites[7];
        }
        if (!hitUp && !hitLeft)
        {
            sprite.sprite = sprites[0];
        }
        if (!hitUp && !hitRight)
        {
            sprite.sprite = sprites[2];
        }
        if (!hitLeft && !hitDown)
        {
            sprite.sprite = sprites[6];
        }
        if (!hitDown && !hitRight)
        {
            sprite.sprite = sprites[8];
        }
        if (!hitDown && !hitUp && !hitLeft && !hitRight)
        {
            sprite.sprite = singleSprite;
        }
    }
}
