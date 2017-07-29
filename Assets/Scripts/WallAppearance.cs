using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallAppearance : MonoBehaviour {

    SpriteRenderer sprite;

    public Tile tile;
    public string texture;
    Sprite[] sprites;

    public BoxCollider2D collider;

    // Use this for initialization
    void Awake () {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetAppearance()
    {
        sprites = Resources.LoadAll<Sprite>(texture);

        sprite.sprite = sprites[4];

        tile.CheckAdjacentTiles();

        if (tile.up)
        {
            sprite.sprite = sprites[1];
        }
        if (tile.left)
        {
            sprite.sprite = sprites[3];
        }
        if (tile.right)
        {
            sprite.sprite = sprites[5];
        }
        if (tile.down)
        {
            sprite.sprite = sprites[7];
        }
        if (tile.up && tile.left)
        {
            sprite.sprite = sprites[0];
        }
        if (tile.up && tile.right)
        {
            sprite.sprite = sprites[2];
        }
        if (tile.left && tile.down)
        {
            sprite.sprite = sprites[6];
        }
        if (tile.down && tile.right)
        {
            sprite.sprite = sprites[8];
        }
        if (tile.down && tile.up && tile.left && tile.right)
        {
            sprite.sprite = sprites[9];
        }
        if (tile.down && tile.up && tile.left && tile.right)
        {
            sprite.sprite = sprites[9];
        }
        //if there are no adjacent floor tiles
        if (!tile.right && !tile.left && !tile.up && !tile.down)
        {
            if (tile.downRight)
            {
                sprite.sprite = sprites[10];
            }
            if (tile.downLeft)
            {
                sprite.sprite = sprites[11];
            }
            if (tile.upRight)
            {
                sprite.sprite = sprites[12];
            }
            if (tile.upLeft)
            {
                sprite.sprite = sprites[13];
            }
        }
    }
}
