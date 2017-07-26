using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CellularAutomata
{
    static int[,] map;
    static TileType[,] tileMap;

    static int width;
    static int height;

    static float intitalFillPercent = 55f;
    static int iterations = 3;

    static System.Random prng;

    public static TileType[,] GenerateTileMap(int heightMapWidth, int heightMapHeight, int seed)
    {
        width = heightMapWidth;
        height = heightMapHeight;

        map = new int[width, height];
        tileMap = new TileType[width, height];

        prng = new System.Random(seed);

        RandomFill();

        for (int i = 0; i < iterations; i++)
        {
            SmoothMap();
        }

        return tileMap;
    }

    private static void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborEmptyTiles = getSurroundingEmptyCount(x, y);

                if (neighborEmptyTiles > 4)
                {
                    map[x, y] = 1;
                    tileMap[x, y] = TileType.wall;
                }
                else if (neighborEmptyTiles < 4)
                {
                    map[x, y] = 0;
                    tileMap[x, y] = TileType.floor;
                }
            }
        }
    }

    private static void RandomFill()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                map[x, y] = (prng.Next(0, 100) < intitalFillPercent) ? 1 : 0;
            }
        }
    }

    private static int getSurroundingEmptyCount(int gridX, int gridY)
    {
        int emptyCount = 0;
        for (int x = gridX - 1; x <= gridX + 1; x++)
        {
            for (int y = gridY - 1; y <= gridY + 1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (x != gridX || y != gridY)
                    {
                        emptyCount += map[x, y];
                    }
                }
                else
                {
                    emptyCount++;
                }
            }
        }
        return emptyCount;
    }
}
