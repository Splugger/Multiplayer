using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { wall, floor, trap, stairs }

public class LevelGenerator : MonoBehaviour
{

    public int minNumRooms = 3;
    public int maxNumRooms = 6;
    public int minRoomSize = 5;
    public int maxRoomSize = 30;
    public int mapWidth = 100;
    public int mapHeight = 100;

    public bool doubleHallwayWidth = true;

    public int numWalkers = 10;
    public int numSwordsmen = 6;
    public int numGunners = 3;

    public int numPickupChests = 5;
    public int numWeaponChests = 2;
    public int numAmmoDrops = 4;
    public int numMedkitDrops = 4;

    public Color levelColor = Color.white;

    List<Vector4> rooms = new List<Vector4>();
    bool[] roomConnected;

    float gridSize = 0.16f;
    int numRooms;
    TileType[,] tileMap;

    CompositeCollider2D collider;

    // Use this for initialization
    public void Start()
    {
        collider = GetComponent<CompositeCollider2D>();

        levelColor = new Color(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f), 1f);

        tileMap = new TileType[mapWidth, mapHeight];

        numRooms = Random.Range(minNumRooms, maxNumRooms);
        for (int i = 0; i < numRooms; i++)
        {
            GenerateRoom();
        }
        GenerateHallways();
        GenerateExit();
        DrawMap();
        collider.GenerateGeometry();

        //spawn resources
        SpawnOnMap(numWalkers, "Walker");
        SpawnOnMap(numGunners, "Gunner");
        SpawnOnMap(numSwordsmen, "Swordsman");
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
                ai.maxHealth = Random.Range(10f, 100f);
                ai.health = ai.maxHealth;
                ai.moveSpeed = Random.Range(0.5f, 2f);
            }
        }
    }

    void GenerateRoom()
    {
        int width = Random.Range(minRoomSize, maxRoomSize);
        int height = Random.Range(minRoomSize, maxRoomSize);
        Vector2 position = RandomMapPoint(0);

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

        rooms.Add(new Vector4((int)position.x, (int)position.y, width, height));
    }

    void DrawMap()
    {
        List<GameObject> wallTiles = new List<GameObject>();
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject tile = null;
                string tileName = null;
                switch (tileMap[x, y])
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
                }
                tile = Instantiate(Resources.Load("Tile_" + tileName) as GameObject);
                tile.transform.position = new Vector2(x * gridSize, y * gridSize);
                tile.transform.parent = transform;
                SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
                if (tileName != "Wall")
                {
                    sprite.color = levelColor;
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
        for (int i = 0; i < numRooms; i++)
        {
            //find position of room
            int x1 = (int)rooms[i].x;
            int y1 = (int)rooms[i].y;
            int w1 = (int)rooms[i].z;
            int h1 = (int)rooms[i].w;

            int minDist = mapWidth + mapHeight;
            int closestRoomIndex = 0;

            //look through all rooms
            for (int r = 0; r < numRooms - 1; r++)
            {
                int x = (int)rooms[r].x;
                int y = (int)rooms[r].y;

                //check if room is nearby
                if (Mathf.Abs(x1 - x) + Mathf.Abs(y1 - y) < minDist && r != i && !roomConnected[r])
                {
                    minDist = Mathf.Abs(x1 - x) + Mathf.Abs(y1 - y);
                    closestRoomIndex = r;
                }
            }

            //compare to nearest room
            int x2 = (int)rooms[closestRoomIndex].x;
            int y2 = (int)rooms[closestRoomIndex].y;
            int w2 = (int)rooms[closestRoomIndex].z;
            int h2 = (int)rooms[closestRoomIndex].w;

            roomConnected[i] = true;
            roomConnected[closestRoomIndex] = true;

            //calculate hallway positions
            int hallY1 = Random.Range(y1 + 1, y1 + h1 - 1);
            int hallY2 = Random.Range(y2 + 1, y2 + h2 - 1);
            int midX = Random.Range(x1 + (w1 / 2), x2 + (w2 / 2));

            //generate horizontal hallway
            if (x1 != x2)
            {
                int startX = x1 + (w1 / 2);
                int endX = x2 + (w2 / 2);
                int startY = hallY1;
                int endY = hallY2;
                //switch to go from smaller to larger
                if (endX < startX)
                {
                    int temp = endX;
                    endX = startX;
                    startX = temp;

                    temp = endY;
                    endY = startY;
                    startY = temp;
                }
                for (int pathX = startX; pathX <= endX; pathX++)
                {
                    int pathY = startY;
                    if (pathX >= midX) pathY = endY;

                    tileMap[pathX, pathY] = RandomTileType();
                    if (doubleHallwayWidth) tileMap[pathX, pathY - 1] = RandomTileType();
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
}

