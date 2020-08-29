using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDefs : MonoBehaviour
{
    public List<TileType> tileTypes;

    public Tile grassTile;
    public TileType grass;

    public Tile rockTile;
    public TileType rock;

    public Tile logTile;
    public TileType log;

    public Tile leafTile;
    public TileType leaf;

    public Tile waterTile;
    public TileType water;

    public Tile acornTile;
    public TileType acorn;

    public Tile steelTile;
    public TileType steel;

    public Tile sandTile;
    public TileType sand;

    public Tile darkRockTile;
    public TileType darkRock;

    public Tile magmaTile;
    public TileType magma;

    public Tile doorTile;
    public TileType door;

    public Tile openDoorTile;
    public TileType openDoor;

    public Tile woodFlooringTile;
    public TileType woodFlooring;

    public Tile lampTile;
    public TileType lamp;

    public Tile circutBoardTile;
    public TileType circutBoard;

    public Tile computerTile;
    public TileType computer;

    public Tile cannonTile;
    public TileType cannon;

    public Tile campfireTile;
    public TileType campfire;

    public Tile rawMeatTile;
    public TileType rawMeat;

    public Tile cookedMeatTile;
    public TileType cookedMeat;


    public AudioClip rockSound;
    public AudioClip woodSound;
    public AudioClip waterSound;
    public AudioClip steelSound;
    public AudioClip magmaSound;
    public AudioClip meatSound;


    public TileType GetTileFromName(string name)
    {
        foreach (TileType tileType in tileTypes) {
            if (tileType.name.Equals(name))
            {
                return tileType;
            }
        }
        Debug.LogError("Tile not found in List tileTypes of name " + name);
        return null;
    }
    public TileType GetTileFromID(int id)
    {
        foreach (TileType tileType in tileTypes)
        {
            if (tileType.id == id)
            {
                return tileType;
            }
        }
        Debug.LogError("Tile not found in List tileTypes of id " + id);
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        tileTypes = new List<TileType>();
        grass = new TileType(1, "grass", grassTile, 0.15f, 0.65f, null);
        tileTypes.Add(grass);
        rock = new TileType(2, "rock", rockTile, 0.38f, 2, rockSound);
        tileTypes.Add(rock);
        log = new TileType(3, "log", logTile, 0.28f, 1.5f, woodSound);
        tileTypes.Add(log);
        leaf = new TileType(4, "leaf", leafTile, 0, 0.3f, null);
        tileTypes.Add(leaf);
        water = new TileType(5, "water", waterTile, 0.12f, 1000, waterSound);
        tileTypes.Add(water);
        acorn = new TileType(6, "acorn", acornTile, 0, 0.35f, null);
        tileTypes.Add(acorn);
        steel = new TileType(7, "steel", steelTile, 0.75f, 1000, steelSound);
        tileTypes.Add(steel);
        sand = new TileType(8, "sand", sandTile, 0.1f, 0.8f, null);
        tileTypes.Add(sand);
        darkRock = new TileType(9, "dark rock", darkRockTile, 0.3f, 2, rockSound);
        tileTypes.Add(darkRock);
        magma = new TileType(10, "magma", magmaTile, 0.6f, 1000, magmaSound);
        tileTypes.Add(magma);

        door = new TileType(11, "door", doorTile, 0.75f, 1000, steelSound);
        tileTypes.Add(door);
        openDoor = new TileType(12, "open door", openDoorTile, 0.75f, 1000, steelSound);
        tileTypes.Add(openDoor);

        woodFlooring = new TileType(13, "wood flooring", woodFlooringTile, 0.31f, 1000, woodSound);
        tileTypes.Add(woodFlooring);
        lamp = new TileType(14, "lamp", lampTile, 0.3f, 4, steelSound);
        tileTypes.Add(lamp);

        circutBoard = new TileType(15, "circut board", circutBoardTile, 0.5f, 1.75f, steelSound);
        tileTypes.Add(circutBoard);
        computer = new TileType(16, "computer", computerTile, 0.6f, 5, steelSound);
        tileTypes.Add(computer);
        cannon = new TileType(17, "cannon", cannonTile, 0.38f, 1000, rockSound);
        tileTypes.Add(cannon);

        campfire = new TileType(18, "campfire", campfireTile, 0.31f, 1.65f, woodSound);
        tileTypes.Add(campfire);    
        rawMeat = new TileType(19, "raw meat", rawMeatTile, 0.20f, 1f, meatSound);
        tileTypes.Add(rawMeat);
        cookedMeat = new TileType(20, "cooked meat", cookedMeatTile, 0.25f, 1.25f, meatSound);
        tileTypes.Add(cookedMeat);
    }


    private void Update()
    {
        if (tileTypes == null)
        {
            Start();
        }
    }
}
