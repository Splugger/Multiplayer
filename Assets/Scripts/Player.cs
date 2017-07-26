using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Creature
{

    public int playerNum = 1;

    GameObject healthSliderObj;
    Slider healthSlider;
    GameObject staminaSliderObj;
    Slider staminaSlider;

    // Use this for initialization
    public override void Start()
    {
        healthSliderObj = transform.FindDeepChild("healthSlider").gameObject;
        healthSlider = healthSliderObj.GetComponent<Slider>();
        staminaSliderObj = transform.FindDeepChild("staminaSlider").gameObject;
        staminaSlider = staminaSliderObj.GetComponent<Slider>();

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {

        //movement
        horizontal = Input.GetAxis("Player " + playerNum + " Horizontal");
        vertical = Input.GetAxis("Player " + playerNum + " Vertical");

        if (Input.GetButtonDown("Player " + playerNum + " Dodge"))
        {
            Dodge();
        }

        //stamina slider
        staminaSlider.value = stamina / maxStamina;

        base.Update();
    }

    public override void Damage(float amount, Vector2 knockback)
    {
        base.Damage(amount, knockback);
        healthSlider.value = health / maxHealth;
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        healthSlider.value = health / maxHealth;
    }

    public override void Die()
    {
        Game.control.playerObjs.Remove(gameObject);

        base.Die();
    }

    public void UpdateVision()
    {
        foreach (GameObject tile in Game.control.levelGenerator.floorTiles)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, tile.transform.position);
            if (hit)
            {
                GameObject fogOfWar = Instantiate(Resources.Load("Tile_Dark") as GameObject);
                fogOfWar.transform.position = tile.transform.position;
            }
        }
    }
}
