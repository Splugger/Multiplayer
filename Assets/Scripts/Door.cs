using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour
{

    SpriteRenderer sprite;

    public Tile tile;
    public string texture;
    Sprite[] sprites;

    public BoxCollider2D collider;

    // Use this for initialization
    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Open();
        }
    }

    private void Open()
    {
        gameObject.SetActive(false);
    }

    public void SetAppearance()
    {
        sprites = Resources.LoadAll<Sprite>(texture);

        sprite.sprite = sprites[0];

        tile.CheckAdjacentTiles();

        if ((!tile.up && !tile.down && !tile.left) || (!tile.up && !tile.down && !tile.right) || (!tile.left && !tile.right && !tile.down) || (!tile.left && !tile.right && !tile.up))
        {
            tile.ReplaceTile();
        }

        if (!tile.up && !tile.down)
        {
            sprite.sprite = sprites[1];
        }
    }
}
