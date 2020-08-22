using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Recipe
{
    public readonly int id;
    public readonly string name;
    public TileType[,] inputs;
    public readonly TileType output;
    public readonly int outputAmount;

    public Recipe(int ID, string Name, TileType[,] Inputs, TileType Output, int OutputAmount)
    {
        id = ID;
        name = Name;
        inputs = Inputs;
        output = Output;
        outputAmount = OutputAmount;
    }
}
