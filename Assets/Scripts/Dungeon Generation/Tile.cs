using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{

    public int x;
    public int y;
    public TileType type;
    public GameObject tileObj;
    public SpriteRenderer spriteRenderer;
    public WallAppearance appearance;
    public Sprite unseenSprite;
    Sprite sprite;
    public Color color;

    public Tile(int x, int y, TileType type, GameObject tileObj)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.tileObj = tileObj;

        Init();
    }

    void Init()
    {
        if (type == TileType.wall)
        {
            appearance = tileObj.GetComponent<WallAppearance>();
        }
        spriteRenderer = tileObj.GetComponent<SpriteRenderer>();
        unseenSprite = Resources.Load<Sprite>("16x16");
    }

    public void SetSpriteProperties()
    {
        sprite = spriteRenderer.sprite;
        color = spriteRenderer.color;
    }

    public void SetVisible(bool state)
    {
        if (state == false)
        {
            spriteRenderer.sprite = unseenSprite;
            spriteRenderer.color = Game.control.levelGenerator.wallColor;
            spriteRenderer.sortingOrder = 3;
        }
        else
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = -1;
        }
    }
}
