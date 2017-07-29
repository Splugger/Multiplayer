using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemType { pickup, weapon }

public class Game : MonoBehaviour
{

    public static Game control;

    public bool debug = false;

    public GameObject camObj;
    public ScreenShake screenShake;

    public LevelGenerator levelGenerator;

    public List<GameObject> playerObjs = new List<GameObject>();

    public int floorNumber = 0;

    public Color memoryColorOffset = new Color(0.1f, 0.1f, 0.1f, 0f);

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

        Init();

        if (floorNumber == 0)
        {
            playerObjs = GameObject.FindGameObjectsWithTag("Player").ToList();

            foreach (GameObject playerObj in playerObjs)
            {
                DontDestroyOnLoad(playerObj);
            }
        }
    }

    public void Init()
    {
        camObj = GameObject.FindWithTag("MainCamera");
        screenShake = camObj.GetComponent<ScreenShake>();

        levelGenerator = GameObject.Find("Level Generator").GetComponent<LevelGenerator>();
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
        switch (Random.Range(0, 6))
        {
            case 0:
                weapon = itemObj.AddComponent<Grenade>();
                break;
            case 1:
                weapon = itemObj.AddComponent<Melee>();
                break;
            case 2:
                weapon = itemObj.AddComponent<Shield>();
                break;
            default:
                weapon = itemObj.AddComponent<Gun>();
                break;
        }
        itemObj.GetComponent<WeaponObject>().weapon = weapon;
        int weaponLevel = Random.Range(floorNumber - 4, floorNumber);
        if (weaponLevel < 1) weaponLevel = 1;
        weapon = GenerateRandomWeapon(weapon, weaponLevel);
    }

    public Weapon GenerateRandomWeapon(Weapon weapon, int level)
    {
        //set general weapon parameters
        weapon.range = Random.Range(1f, 3f);
        weapon.recoil = Random.Range(1f, 5f);
        weapon.knockback = Random.Range(1f, 5f);
        switch (Random.Range(0, 2))
        {
            case 0:
                weapon.maxCooldown = Random.Range(0.1f, 0.6f) / level;
                break;
            case 1:
                weapon.maxCooldown = Random.Range(0.6f, 2f) / level;
                break;
        }
        weapon.damage = Random.Range(5f, 20f) * weapon.maxCooldown * level;
        //gun
        if (weapon.GetType().IsAssignableFrom(typeof(Gun)))
        {
            weapon.maxAmmo = (int)(Random.Range(10, 201) * weapon.maxCooldown * level);
        }
        //melee
        if (weapon.GetType().IsAssignableFrom(typeof(Melee)))
        {
            ((Melee)weapon).staminaCost = Random.Range(1f, 3f) * weapon.damage * level;
            ((Melee)weapon).slashWidth = Random.Range(1f, 2f) * level;
            weapon.damage *= 1.5f;
        }
        //grenade
        if (weapon.GetType().IsAssignableFrom(typeof(Grenade)))
        {
            ((Grenade)weapon).throwStaminaCost = Random.Range(5f, 10f);
            weapon.maxAmmo = Random.Range(3, 21) * level;
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
            ((Shield)weapon).stability = Random.Range(0.1f, 0.9f) * level;
        }
        weapon.ammo = weapon.maxAmmo;
        weapon.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        weapon.SetSprite();

        return weapon;
    }

    public void NewLevel()
    {
        floorNumber++;
        SceneManager.LoadScene("Procedural");
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Init();
        DestroyExtraPlayers();
    }
}
