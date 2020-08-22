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

    public Tile shellTile;
    public TileType shell;


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
        grass = new TileType(0, "grass", grassTile, 0.15f, 0.65f);
        tileTypes.Add(grass);
        rock = new TileType(1, "rock", rockTile, 0.38f, 2);
        tileTypes.Add(rock);
        log = new TileType(2, "log", logTile, 0.28f, 1.5f);
        tileTypes.Add(log);
        leaf = new TileType(3, "leaf", leafTile, 0, 0.3f);
        tileTypes.Add(leaf);
        water = new TileType(4, "water", waterTile, 0.1f, 1000);
        tileTypes.Add(water);
        acorn = new TileType(5, "acorn", acornTile, 0.05f, 0.5f);
        tileTypes.Add(acorn);
        steel = new TileType(6, "steel", steelTile, 0.75f, 1000);
        tileTypes.Add(steel);
        sand = new TileType(7, "sand", sandTile, 0.1f, 0.8f);
        tileTypes.Add(sand);
        darkRock = new TileType(8, "dark rock", darkRockTile, 0.3f, 2);
        tileTypes.Add(darkRock);
        magma = new TileType(9, "magma", magmaTile, 0.6f, 1000);
        tileTypes.Add(magma);
        door = new TileType(10, "door", doorTile, 0.75f, 1000);
        tileTypes.Add(door);
        openDoor = new TileType(11, "open door", openDoorTile, 0.75f, 1000);
        tileTypes.Add(openDoor);

        woodFlooring = new TileType(12, "wood flooring", woodFlooringTile, 0.31f, 1000);
        tileTypes.Add(woodFlooring);
        lamp = new TileType(13, "lamp", lampTile, 0.3f, 4);
        tileTypes.Add(lamp);

        circutBoard = new TileType(14, "circut board", circutBoardTile, 0.5f, 1.75f);
        tileTypes.Add(circutBoard);
        computer = new TileType(15, "computer", computerTile, 0.6f, 5);
        tileTypes.Add(computer);
        cannon = new TileType(16, "cannon", cannonTile, 0.38f, 2);
        tileTypes.Add(cannon);

        campfire = new TileType(17, "campfire", campfireTile, 0.28f, 1.5f);
        tileTypes.Add(campfire);
        rawMeat = new TileType(18, "raw meat", rawMeatTile, 0.20f, 1f);
        tileTypes.Add(rawMeat);
        cookedMeat = new TileType(19, "cooked meat", cookedMeatTile, 0.25f, 1.25f);
        tileTypes.Add(cookedMeat);
        shell = new TileType(20, "shell", shellTile, 0.3f, 1.65f);
        tileTypes.Add(shell);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
