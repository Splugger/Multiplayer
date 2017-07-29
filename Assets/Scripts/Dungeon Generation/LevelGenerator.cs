using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType { wall, floor, trap, stairs, door }

public class LevelGenerator : MonoBehaviour
{

    public int minNumRoomAttempts = 3;
    public int maxNumRoomAttempts = 6;
    public int minRoomSize = 5;
    public int maxRoomSize = 30;
    public int mapWidth = 100;
    public int mapHeight = 100;

    public bool doubleHallwayWidth = true;

    public int numWalkers = 10;
    public int numSwordsmen = 6;
    public int numGunners = 3;
    public bool spawnEnemies = true;

    public int numPickupChests = 5;
    public int numWeaponChests = 2;
    public int numAmmoDrops = 4;
    public int numMedkitDrops = 4;

    public Color levelColor = Color.white;
    public Color wallColor = Color.white;

    public string wallName;

    List<Room> rooms = new List<Room>();
    bool[] roomConnected;

    float gridSize = 0.16f;
    int numRoomAttempts;
    public TileType[,] tileMap;
    public List<GameObject> floorTiles = new List<GameObject>();
    public List<Tile> tiles = new List<Tile>();

    public CompositeCollider2D collider;

    // Use this for initialization
    public void Start()
    {
        collider = GetComponent<CompositeCollider2D>();

        levelColor = new Color(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f), 1f);

        //set wall color to darkened level color or its compliment
        switch (Random.Range(0, 2))
        {
            case 0:
                float levelColorIntensity = Mathf.Max(levelColor.r, levelColor.g, levelColor.b);
                wallColor = new Color(Random.Range(0.1f, levelColorIntensity), Random.Range(0.1f, levelColorIntensity), Random.Range(0.1f, levelColorIntensity), 1f);
                break;
            case 1:
                wallColor = levelColor.Complimentary();
                break;
        }

        switch (Random.Range(0, 2))
        {
            case 0:
                wallName = "Bricks";
                break;
            case 1:
                wallName = "Cobblestone";
                break;
        }

        tileMap = new TileType[mapWidth, mapHeight];

        numRoomAttempts = Random.Range(minNumRoomAttempts, maxNumRoomAttempts);
        for (int i = 0; i < numRoomAttempts; i++)
        {
            GenerateRoom();
        }
        GenerateHallways();
        GenerateExit();
        DrawMap();

        List<Tile> walls = tiles.Where(q => q.type == TileType.wall).ToList();
        foreach (Tile wall in walls)
        {
            if (TileHasNeighborOfType(wall.x, wall.y, TileType.floor))
                wall.tileObj.GetComponent<WallAppearance>().SetAppearance();
            wall.SetSpriteProperties();
        }
        foreach (Tile wall in walls)
        {
            wall.tileObj.GetComponent<WallAppearance>().collider.usedByComposite = true;
        }

        collider.GenerateGeometry();

        //spawn resources
        if (spawnEnemies)
        {
            SpawnOnMap(numWalkers, "Walker");
            SpawnOnMap(numGunners, "Gunner");
            SpawnOnMap(numSwordsmen, "Swordsman");
        }
        SpawnOnMap(numPickupChests, "Chest_Pickup");
        SpawnOnMap(numWeaponChests, "Chest_Weapon");
        SpawnOnMap(numAmmoDrops, "Item_Ammo");
        SpawnOnMap(numMedkitDrops, "Item_Medkit");

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        Vector2 spawnPos = RandomMapPoint(TileType.floor) * gridSize;
        Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPos, mapWidth);
        foreach (Collider2D col in cols)
        {
            Creature creature = col.gameObject.GetComponent<Creature>();
            if (creature != null)
            {
                RaycastHit2D hit = Physics2D.Linecast(spawnPos, col.transform.position);
                if (hit)
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
        }
        foreach (GameObject obj in Game.control.playerObjs)
        {
            obj.transform.position = spawnPos;
        }
    }

    void SpawnOnMap(int number, string name)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject obj = Instantiate(Resources.Load(name) as GameObject);
            obj.transform.position = RandomMapPoint(TileType.floor) * gridSize;
            if (obj.tag == "Enemy")
            {
                AI ai = obj.GetComponent<AI>();
                ai.level = Random.Range(1, Game.control.floorNumber);
                ai.maxHealth = Random.Range(5f, 15f) * ai.level;
                ai.health = ai.maxHealth;
                ai.moveSpeed = Random.Range(0.3f, 1.5f) * ai.level;
            }
        }
    }

    void GenerateRoom()
    {
        int width = Random.Range(minRoomSize, maxRoomSize);
        int height = Random.Range(minRoomSize, maxRoomSize);
        Vector2 position = RandomMapPoint(0);
        Room room = new Room((int)position.x, (int)position.y, width, height);

        //don't create this room if it's overlapping another room
        if (RoomIsOverlapping(room)) return;

        rooms.Add(room);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x != 0 && y != 0 && x != mapWidth && y != mapHeight)
                {
                    tileMap[(int)position.x + x, (int)position.y + y] = RandomTileType();
                }
            }
        }
    }

    void DrawMap()
    {
        List<GameObject> wallTiles = new List<GameObject>();
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject tileObj = null;
                string tileName = null;
                TileType type = tileMap[x, y];
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
                GameObject floor = Instantiate(Resources.Load("Tile_Floor") as GameObject);
                floor.transform.position = new Vector2(x * gridSize, y * gridSize);
                floor.transform.parent = transform;
                SpriteRenderer floorSprite = floor.GetComponent<SpriteRenderer>();
                floorSprite.color = levelColor;

                //add floor tile to list of floor tiles
                floorTiles.Add(floor);

                tileObj = Instantiate(Resources.Load("Tile_" + tileName) as GameObject);
                tileObj.transform.position = new Vector2(x * gridSize, y * gridSize);
                tileObj.transform.parent = transform;

                if (tileObj != null)
                {
                    //add to list of tiles
                    Tile tile = new Tile(x, y, type, tileObj);
                    tiles.Add(tile);

                    SpriteRenderer sprite = tileObj.GetComponent<SpriteRenderer>();
                    if (tileName != "Wall")
                    {
                        if (tileName == "Trap")
                        {
                            tileObj.GetComponent<Trap>().tile = tile;
                        }
                        if (tileName == "Door")
                        {
                            Door door = tileObj.GetComponent<Door>();
                            door.tile = tile;
                            door.SetAppearance();
                        }
                        if (tileName == "Door" || tileName == "Stairs")
                        {
                            sprite.color = wallColor;
                        }
                        else
                        {
                            sprite.color = levelColor;
                        }
                        tile.SetSpriteProperties();
                    }
                    else
                    {
                        WallAppearance appearance = tileObj.GetComponent<WallAppearance>();
                        appearance.tile = tile;
                        appearance.texture = "Wall_" + wallName;
                        sprite.color = wallColor;
                    }
                }
            }
        }
    }

    Vector2 RandomMapPoint(TileType tileType)
    {
        int x = Random.Range(0, mapWidth - maxRoomSize);
        int y = Random.Range(0, mapHeight - maxRoomSize);
        Vector2 mapPos = new Vector2(x, y);

        if (tileMap[x, y] == tileType)
        {
            return mapPos;
        }
        else
        {
            return RandomMapPoint(tileType);
        }
    }

    void GenerateHallways()
    {
        roomConnected = new bool[rooms.Count];
        for (int i = 0; i < rooms.Count; i++)
        {
            //find position of room
            int x1 = rooms[i].x;
            int y1 = rooms[i].y;
            int w1 = rooms[i].w;
            int h1 = rooms[i].h;

            int minDist = mapWidth + mapHeight;
            int closestRoomIndex = 0;

            //look through all rooms
            for (int r = 0; r < rooms.Count - 1; r++)
            {
                int x = rooms[r].x;
                int y = rooms[r].y;

                //check if room is nearby
                if (Mathf.Abs(x1 - x) + Mathf.Abs(y1 - y) < minDist && r != i && !roomConnected[r])
                {
                    minDist = Mathf.Abs(x1 - x) + Mathf.Abs(y1 - y);
                    closestRoomIndex = r;
                }
            }

            //compare to nearest room
            int x2 = rooms[closestRoomIndex].x;
            int y2 = rooms[closestRoomIndex].y;
            int w2 = rooms[closestRoomIndex].w;
            int h2 = rooms[closestRoomIndex].h;

            roomConnected[i] = true;
            roomConnected[closestRoomIndex] = true;

            if (Mathf.Abs(x1 - x2) > Mathf.Abs(y1 - y2))
            {
                //tunnel horizontally
                Tunnel(x1, y1, w1, h1, x2, y2, w2, h2);
            }
            else
            {
                //tunnel vertically
                Tunnel(x2, y2, w2, h2, x1, y1, w1, h1);
            }
        }
    }

    void Tunnel(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
    {
        //calculate hallway positions
        int hallY1 = Random.Range(y1 + 1, y1 + h1 - 1);
        int hallY2 = Random.Range(y2 + 1, y2 + h2 - 1);
        int midX = 0;

        //generate horizontal hallway
        if (x1 != x2)
        {
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;

            //go from lower x and y to higher x and y
            if (x1 < x2)
            {
                startX = x1 + w1;
                endX = x2;
                midX = Random.Range(x1 + w1, x2);
                startY = hallY1;
                endY = hallY2;
            }
            else
            {
                startX = x2 + w2;
                endX = x1;
                midX = Random.Range(x2 + w2, x1);
                startY = hallY2;
                endY = hallY1;
            }

            for (int pathX = startX; pathX <= endX; pathX++)
            {
                int pathY = startY;
                if (pathX >= midX) pathY = endY;

                if (pathX == startX || pathX == endX)
                {
                    tileMap[pathX, pathY] = TileType.door;
                }
                else
                {
                    tileMap[pathX, pathY] = RandomTileType();
                    if (doubleHallwayWidth) tileMap[pathX, pathY - 1] = RandomTileType();
                }
            }
        }

        //connect vertically
        int minHallY = Mathf.Min(hallY1, hallY2);
        int maxHallY = Mathf.Max(hallY1, hallY2);

        for (int pathVertical = minHallY; pathVertical <= maxHallY; pathVertical++)
        {
            tileMap[midX, pathVertical] = RandomTileType();
            if (doubleHallwayWidth) tileMap[midX + 1, pathVertical] = RandomTileType();
        }
    }

    void GenerateExit()
    {
        Vector2 pos = RandomMapPoint(TileType.floor);

        tileMap[(int)pos.x, (int)pos.y] = TileType.stairs;

    }

    TileType RandomTileType()
    {
        switch (Random.Range(0, 300))
        {
            case 0:
                return TileType.trap;
            default:
                return TileType.floor;
        }
    }

    bool RoomIsOverlapping(Room room)
    {
        int xPos = room.x;
        int yPos = room.y;
        int width = room.w;
        int height = room.h;

        for (int x = xPos - 1; x < xPos + width + 1; x++)
        {
            for (int y = yPos - 1; y < yPos + height + 1; y++)
            {
                if (x != 0 && y != 0 && x != mapWidth && y != mapHeight)
                {
                    if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                    {
                        if (tileMap[x, y] == TileType.floor)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    bool TileHasNeighborOfType(int xPos, int yPos, TileType tile)
    {
        for (int x = xPos - 1; x <= xPos + 1; x++)
        {
            for (int y = yPos - 1; y <= yPos + 1; y++)
            {
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                {
                    if (x != xPos || y != yPos)
                    {
                        if (tileMap[x, y] == tile) return true;
                    }
                }
            }
        }
        return false;
    }
}

