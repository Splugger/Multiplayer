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

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;
    public bool upRight = false;
    public bool upLeft = false;
    public bool downRight = false;
    public bool downLeft = false;

    bool revealed = false;

    bool visible;

    public Tile(int x, int y, TileType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    void Init()
    {
        if (type == TileType.wall)
        {
            appearance = tileObj.GetComponent<WallAppearance>();
        }

        spriteRenderer = tileObj.GetComponent<SpriteRenderer>();
        unseenSprite = Resources.Load<Sprite>("Tile_Unknown");
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
            //is in memory
            if (revealed)
            {
                spriteRenderer.sprite = sprite;
                spriteRenderer.color = color - Game.control.memoryColorOffset;
            }
            //has never been seen
            else
            {
                if (Game.control.debug)
                    spriteRenderer.sprite = sprite;
                else
                    spriteRenderer.sprite = unseenSprite;
                spriteRenderer.color = Game.control.levelGenerator.wallColor - Game.control.memoryColorOffset;
            }
            spriteRenderer.sortingOrder = 4;
        }
        else
        {
            revealed = true;

            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = -1;
        }
        visible = state;
    }

    public void CheckAdjacentTiles()
    {
        for (int x = this.x - 1; x <= this.x + 1; x++)
        {
            for (int y = this.y - 1; y <= this.y + 1; y++)
            {
                if (x >= 0 && x < Game.control.levelGenerator.mapWidth && y >= 0 && y < Game.control.levelGenerator.mapWidth)
                {
                    if (x != this.x || y != this.y)
                    {
                        if (Game.control.levelGenerator.tileMap[x, y] != TileType.wall)
                        {
                            //if the tile is not on the left or right
                            if (!(x > this.x || x < this.x))
                            {
                                if (y > this.y) up = true;
                                if (y < this.y) down = true;
                            }
                            else
                            {
                                //if the tile is above, check if it is left or right
                                if (y > this.y)
                                {
                                    if (x > this.x) upRight = true;
                                    if (x < this.x) upLeft = true;
                                }

                                //if the tile is below, check if it is left or right
                                if (y < this.y)
                                {
                                    if (x > this.x) downRight = true;
                                    if (x < this.x) downRight = true;
                                }
                            }
                            //if the tile is not above or below
                            if (!(y > this.y || y < this.y))
                            {
                                if (x > this.x) right = true;
                                if (x < this.x) left = true;
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
        }
    }

    public void ReplaceTile()
    {
        SpawnTile();
        type = TileType.floor;

        spriteRenderer = tileObj.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Game.control.levelGenerator.levelColor;

        GameObject.Destroy(tileObj);
    }

    public void SpawnTile()
    {
        int mapWidth = Game.control.levelGenerator.mapWidth;
        int mapHeight = Game.control.levelGenerator.mapHeight;
        Color levelColor = Game.control.levelGenerator.levelColor;
        Color wallColor = Game.control.levelGenerator.wallColor;
        float gridSize = Game.control.levelGenerator.gridSize;
        Transform transform = Game.control.levelGenerator.transform;
        string wallName = Game.control.levelGenerator.wallName;
        string tileName = null;

        switch (type)
        {
            case TileType.wall:
                tileName = "Wall";
                break;
            case TileType.floor:
                tileName = "Floor";
                break;
            case TileType.trap:
                tileName = "Trap";
                break;
            case TileType.stairs:
                tileName = "Stairs";
                break;
            case TileType.door:
                tileName = "Door";
                break;
        }

        //spawn floor tiles in all positions
        GameObject floor = GameObject.Instantiate(Resources.Load("Tile_Floor") as GameObject);
        floor.transform.position = new Vector2(x * gridSize, y * gridSize);
        floor.transform.parent = transform;
        SpriteRenderer floorSprite = floor.GetComponent<SpriteRenderer>();
        floorSprite.color = levelColor;

        //add floor tile to list of floor tiles
        Game.control.levelGenerator.floorTiles.Add(floor);

        tileObj = GameObject.Instantiate(Resources.Load("Tile_" + tileName) as GameObject);
        tileObj.transform.position = new Vector2(x * gridSize, y * gridSize);
        tileObj.transform.parent = transform;

        spriteRenderer = tileObj.GetComponent<SpriteRenderer>();
        if (tileName != "Wall")
        {
            if (tileName == "Trap")
            {
                tileObj.GetComponent<Trap>().tile = this;
            }
            if (tileName == "Door")
            {
                Door door = tileObj.GetComponent<Door>();
                door.tile = this;
                door.SetAppearance();
            }
            if (tileName == "Door" || tileName == "Stairs")
            {
                spriteRenderer.color = wallColor;
            }
            else
            {
                spriteRenderer.color = levelColor;
            }
            SetSpriteProperties();
        }
        else
        {
            WallAppearance appearance = tileObj.GetComponent<WallAppearance>();
            appearance.tile = this;
            appearance.texture = "Wall_" + wallName;
            spriteRenderer.color = wallColor;
        }
        Init();
    }
}
