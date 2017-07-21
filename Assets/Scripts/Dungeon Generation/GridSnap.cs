using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnap : MonoBehaviour {

    public float gridSize = 0.16f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float newX = transform.position.x - (transform.position.x % gridSize);
        float newY = transform.position.y - (transform.position.y % gridSize);

        transform.position = new Vector2(newX, newY);
    }
}
