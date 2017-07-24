using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemType { pickup, weapon }

public class Game : MonoBehaviour
{

    public static Game control;

    public GameObject camObj;
    public ScreenShake screenShake;

    public LevelGenerator levelGenerator;

    public List<GameObject> playerObjs = new List<GameObject>();

    int levelsCleared = 0;

    // Use this for initialization
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }

        camObj = GameObject.FindWithTag("MainCamera");
        screenShake = camObj.GetComponent<ScreenShake>();

        levelGenerator = GameObject.Find("Level Generator").GetComponent<LevelGenerator>();
        print(levelsCleared);

        if (levelsCleared == 0)
        {
            playerObjs = GameObject.FindGameObjectsWithTag("Player").ToList();

            foreach (GameObject playerObj in playerObjs)
            {
                DontDestroyOnLoad(playerObj);
            }
        }
    }

    public void DestroyExtraPlayers()
    {
        //destroy all but original players
        List<GameObject> newPlayerObjs = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject newPlayerObj in newPlayerObjs)
        {
            if (!playerObjs.Contains(newPlayerObj))
            {
                Destroy(newPlayerObj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateRandomItem(Vector3 position, ItemType type = ItemType.weapon, string objName = null)
    {
        if (string.IsNullOrEmpty(objName))
        {
            if (type == ItemType.pickup)
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        objName = "Ammo";
                        break;
                    case 1:
                        objName = "Medkit";
                        break;
                }
            }
            else
            {
                objName = "Weapon";
            }
        }
        GameObject itemObj = Instantiate(Resources.Load("Item_" + objName) as GameObject);
        itemObj.transform.position = position;

        if (objName != "Weapon") return;

        Weapon weapon = null;
        switch (Random.Range(0, 4))
        {
            case 0:
                weapon = itemObj.AddComponent<Grenade>();
                break;
            case 1:
                weapon = itemObj.AddComponent<Gun>();
                break;
            case 2:
                weapon = itemObj.AddComponent<Melee>();
                break;
            case 3:
                weapon = itemObj.AddComponent<Shield>();
                break;
        }
        itemObj.GetComponent<WeaponObject>().weapon = weapon;
        weapon = GenerateRandomWeapon(weapon);
    }

    public Weapon GenerateRandomWeapon(Weapon weapon)
    {
        //set general weapon parameters
        weapon.range = Random.Range(1f, 3f);
        weapon.recoil = Random.Range(1f, 5f);
        weapon.knockback = Random.Range(1f, 5f);
        switch (Random.Range(0, 2))
        {
            case 0:
                weapon.maxCooldown = Random.Range(0.1f, 0.6f);
                break;
            case 1:
                weapon.maxCooldown = Random.Range(0.6f, 2f);
                break;
        }
        weapon.damage = Random.Range(5f, 20f) * weapon.maxCooldown;
        //gun
        if (weapon.GetType().IsAssignableFrom(typeof(Gun)))
        {
            weapon.maxAmmo = (int)(Random.Range(10, 201) * weapon.maxCooldown);
        }
        //melee
        if (weapon.GetType().IsAssignableFrom(typeof(Melee)))
        {
            ((Melee)weapon).staminaCost = Random.Range(1f, 3f) * weapon.damage;
            ((Melee)weapon).slashWidth = Random.Range(1f, 2f);
            weapon.damage *= 2f;
        }
        //grenade
        if (weapon.GetType().IsAssignableFrom(typeof(Grenade)))
        {
            ((Grenade)weapon).throwStaminaCost = Random.Range(5f, 10f);
            weapon.maxAmmo = Random.Range(3, 21);
            switch (Random.Range(0, 5))
            {
                case 0:
                    weapon.range = 0f;
                    break;
                default:
                    break;
            }
            weapon.damage *= 3f;
        }
        //shield
        if (weapon.GetType().IsAssignableFrom(typeof(Shield)))
        {
            ((Shield)weapon).stability = Random.Range(0.1f, 0.9f);
        }
        weapon.ammo = weapon.maxAmmo;
        weapon.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        return weapon;
    }

    public void NewLevel()
    {
        levelsCleared++;
        SceneManager.LoadScene("Procedural");
        DestroyExtraPlayers();
    }
}
