using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDefs : MonoBehaviour
{
    public TileDefs tileDefs;

    public List<Recipe> recipes = new List<Recipe>();

    // Start is called before the first frame update
    void Start()
    {
        recipes.Add(new Recipe(0, "wood flooring", new TileType[2, Crafting.size] {
            { tileDefs.log, tileDefs.log,  null,        null },
            { null,         null,         tileDefs.log, tileDefs.log }
        }, tileDefs.woodFlooring, 4));

        recipes.Add(new Recipe(1, "door", new TileType[2, Crafting.size] {
            { tileDefs.steel, null,           tileDefs.steel, null },
            { null,           tileDefs.steel, null,           tileDefs.steel },
        }, tileDefs.door, 1));

        recipes.Add(new Recipe(2, "lamp", new TileType[2, Crafting.size] {
            { null,           tileDefs.magma, null,         tileDefs.steel },
            { tileDefs.magma, null,           tileDefs.steel, null }
        }, tileDefs.lamp, 2));
        
        recipes.Add(new Recipe(3, "computer", new TileType[4, Crafting.size] {
            { tileDefs.circutBoard, tileDefs.steel,       tileDefs.steel,       tileDefs.steel },
            { tileDefs.steel,       tileDefs.circutBoard, tileDefs.steel,       tileDefs.steel },
            { tileDefs.steel,       tileDefs.steel,       tileDefs.circutBoard, tileDefs.steel },
            { tileDefs.steel,       tileDefs.steel,       tileDefs.steel,       tileDefs.circutBoard }
        }, tileDefs.computer, 1));

        recipes.Add(new Recipe(4, "cannon", new TileType[4, Crafting.size] {
            { tileDefs.circutBoard, tileDefs.rock,        tileDefs.rock,        tileDefs.rock },
            { tileDefs.rock,        tileDefs.circutBoard, tileDefs.rock,        tileDefs.rock },
            { tileDefs.rock,        tileDefs.rock,        tileDefs.circutBoard, tileDefs.rock },
            { tileDefs.rock,        tileDefs.rock,        tileDefs.rock,        tileDefs.circutBoard }
        }, tileDefs.cannon, 1));

        recipes.Add(new Recipe(5, "campfire", new TileType[2, Crafting.size] {
            { null,           tileDefs.magma, null,         tileDefs.log },
            { tileDefs.magma, null,           tileDefs.log, null }
        }, tileDefs.campfire, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
