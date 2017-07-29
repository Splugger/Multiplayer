using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public int x;
    public int y;
    public int w;
    public int h;
    public string wallType;

    public Room(int x, int y, int w, int h)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }
}
