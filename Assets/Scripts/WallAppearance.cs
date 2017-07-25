using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallAppearance : MonoBehaviour {

    SpriteRenderer sprite;

    public Texture2D texture;
    public Sprite singleSprite;
    Sprite[] sprites;

    public BoxCollider2D collider;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAppearance()
    {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(texture.name);

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.16f);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 0.16f);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.16f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.16f);

        sprite.sprite = sprites[4];
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
